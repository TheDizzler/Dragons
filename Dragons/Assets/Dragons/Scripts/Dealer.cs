using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AtomosZ.Gambal.Poker
{
	/// <summary>
	/// Actually a manager class with a clever name.
	/// </summary>
	public class Dealer : MonoBehaviour
	{
		public PokerRules pokerRules = null;
		public int playerNum = 3;


		[SerializeField] private Deck deck = null;
		[SerializeField] private Pot pot = null;
		[SerializeField] private Betting betting = null;
		[SerializeField] private GameObject drawButton = null;
		[SerializeField] private TextMeshProUGUI playerNameText = null;
		[SerializeField] private GameObject handHolder = null;
		[SerializeField] private GameObject playerPrefab = null;
		[SerializeField] private GameObject playerHandPrefab = null;
		private Player[] players = null;
		private List<Player> activePlayers = null;

		private int currentPlayerIndex = -1;
		private List<CardInHand> cardsSelected = new List<CardInHand>();
		public PokerRules.TurnPhase phase = PokerRules.TurnPhase.Dealer;
		private Dictionary<Player, int> amountMatchedThisRound = new Dictionary<Player, int>();

		private int totalRaiseAmount;
		private Player lastRaiser = null;
		private Player firstPlayerToDrawCards = null;
		private int drawPhaseCount = 0;


		public void Start()
		{
			pokerRules = GetComponent<PokerRules>();


			players = new Player[playerNum];
			for (int i = 0; i < playerNum; ++i)
			{
				GameObject pgo = Instantiate(playerPrefab);
				pgo.name = "Player " + (i + 1);
				Player player = pgo.GetComponent<Player>();
				players[i] = player;

				GameObject handPanelgo = Instantiate(playerHandPrefab, handHolder.transform);
				player.handPanel = handPanelgo.GetComponent<HandVisualizer>();
			}


			StartCoroutine(StartGame());
		}


		private IEnumerator StartGame()
		{
			List<Player> p = new List<Player>();
			for (int i = 0; i < players.Length; ++i)
			{
				if (players[i].funds <= 0)
				{ // boot the player
					players[i].RemoveFromGame();
					Destroy(players[i].gameObject);
					continue;
				}

				p.Add(players[i]);
				players[i].ResetHand();
			}

			players = p.ToArray();
			if (players.Length <= 1)
			{
				throw new Exception("Insufficient players");
			}

			deck.CreateDeck(pokerRules.useJokers);
			for (int i = 0; i < Random.Range(2, 6); ++i)
				deck.Shuffle();

			activePlayers = new List<Player>(players);
			pot.ResetPot();
			yield return new WaitForSeconds(.1f); // make sure all objects have run their Start() methods
			yield return StartCoroutine(Deal());
			yield return StartCoroutine(CollectAnte());

			drawPhaseCount = 0;
			StartBetPhase();
		}




		/// <summary>
		/// Deal a full hand to all players.
		/// </summary>
		private IEnumerator Deal()
		{
			for (int i = 0; i < pokerRules.MaxCardsInHand; ++i)
			{
				foreach (Player player in activePlayers)
				{
					player.AddCardToHand(deck.DrawCard());
					yield return new WaitForSeconds(0);
				}
			}
		}

		private IEnumerator CollectAnte()
		{
			foreach (var player in activePlayers)
			{
				yield return new WaitForSeconds(.5f);
				pot.AddToPot(player.SubtractFunds(pokerRules.ante));
			}
		}


		public PokerRules.TurnPhase CurrentPhase()
		{
			return phase;
		}


		public void ReplaceCards()
		{
			int cardCount = cardsSelected.Count;
			activePlayers[currentPlayerIndex].RemoveCards(cardsSelected);
			cardsSelected.Clear();
			StartCoroutine(DealNewCardsTo(cardCount, activePlayers[currentPlayerIndex]));
		}

		public bool IsPlayersTurn(Player player)
		{
			if (currentPlayerIndex != -1)
				return player == activePlayers[currentPlayerIndex];
			return false;
		}


		/// <summary>
		/// Return false when too many cards selected. Otherwise adds card too throwaway list and returns true.
		/// </summary>
		/// <param name="cardInHand"></param>
		/// <returns>false when too many cards selected</returns>
		public bool SetCardSelected(CardInHand cardInHand)
		{
			if (cardsSelected.Count >= pokerRules.MaxCardDrawAmount)
			{
				return false;
			}

			int cardCount = cardsSelected.Count;
			if (deck.InsufficientCards(cardCount))
			{
				Debug.Log("Not enough cards left in deck!");
				return false;
			}

			cardsSelected.Add(cardInHand);
			return true;
		}

		public bool UnsetCardSelected(CardInHand cardInHand)
		{
			if (!cardsSelected.Remove(cardInHand))
			{
				Debug.LogError("Dealer.UnsetCardSelected::coud not unselect card?");
				return false;
			}

			return true;
		}


		private IEnumerator DealNewCardsTo(int cardCount, Player player)
		{
			for (int i = 0; i < cardCount; ++i)
			{
				yield return new WaitForSeconds(.5f);
				player.AddCardToHand(deck.DrawCard());
			}

			EndPlayerTurn();
			StartPlayerTurn();
		}

		private void StartPlayerTurn()
		{
			playerNameText.text = activePlayers[currentPlayerIndex].name;

			switch (phase)
			{
				case PokerRules.TurnPhase.Draw:
					if (firstPlayerToDrawCards == null)
					{
						firstPlayerToDrawCards = activePlayers[currentPlayerIndex];
					}
					else if (firstPlayerToDrawCards == activePlayers[currentPlayerIndex])
					{
						EndPlayerTurn();
						StartBetPhase();
						break;
					}

					drawButton.SetActive(true);
					activePlayers[currentPlayerIndex].StartDraw();

					break;

				case PokerRules.TurnPhase.Bet:
					if (lastRaiser == null)
					{ // this is the first bet
						lastRaiser = activePlayers[currentPlayerIndex];
					}
					else if (lastRaiser == activePlayers[currentPlayerIndex])
					{ // betting phase is done
						EndPlayerTurn();
						StartDrawPhase();
						break;
					}

					int amtMatched = amountMatchedThisRound[activePlayers[currentPlayerIndex]];
					activePlayers[currentPlayerIndex].StartBetting(betting, totalRaiseAmount - amtMatched);

					break;
			}
		}


		public void RaiseBet(Player raisingPlayer, int amtRaised, int matched)
		{
			if (amtRaised > 0)
			{
				totalRaiseAmount += amtRaised;
				lastRaiser = raisingPlayer;
				pot.DisplayRaiseAmount(totalRaiseAmount);
			}

			amountMatchedThisRound[raisingPlayer] += amtRaised + matched;
			pot.AddToPot(amtRaised + matched);
			EndPlayerTurn();
			StartCoroutine(PrepNextPlayerTurn());
		}


		public void Fold(Player currentPlayer)
		{
			EndPlayerTurn();
			currentPlayer.Fold();
			activePlayers.Remove(currentPlayer);
			if (activePlayers.Count == 1)
			{
				Winner(activePlayers[0]);
				return;
			}

			if (--currentPlayerIndex < 0)
				currentPlayerIndex = 0;
			StartCoroutine(PrepNextPlayerTurn());
		}


		private void Winner(Player player)
		{
			player.AddFunds(pot.GetWinnings());
			StartCoroutine(StartGame());
		}


		private IEnumerator PrepNextPlayerTurn()
		{
			yield return new WaitForSeconds(.5f);
			StartPlayerTurn();
		}


		private void EndPlayerTurn()
		{
			activePlayers[currentPlayerIndex++].EndTurn();
			if (currentPlayerIndex >= activePlayers.Count)
				currentPlayerIndex = 0;
			drawButton.SetActive(false);
		}

		private void StartBetPhase()
		{
			phase = PokerRules.TurnPhase.Bet;
			currentPlayerIndex = 0;
			totalRaiseAmount = 0;
			lastRaiser = null;
			amountMatchedThisRound.Clear();
			foreach (var player in activePlayers)
				amountMatchedThisRound[player] = 0;
			StartPlayerTurn();
		}


		private void StartDrawPhase()
		{
			if (++drawPhaseCount >= pokerRules.MaxDrawsAllowed)
			{
				// no more draws. Call game.
				CallGame();
			}
			else
			{
				phase = PokerRules.TurnPhase.Draw;
				currentPlayerIndex = 0;
				firstPlayerToDrawCards = null;
				StartPlayerTurn();
			}
		}

		private void CallGame()
		{
			throw new NotImplementedException();
		}
	}
}