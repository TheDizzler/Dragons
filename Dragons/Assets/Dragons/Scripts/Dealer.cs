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


		[SerializeField] private Deck deck = null;
		[SerializeField] private Pot pot = null;
		[SerializeField] private Betting betting = null;
		[SerializeField] private TextMeshProUGUI playerNameText = null;
		[SerializeField] private Player[] players = null;

		private int currentPlayerIndex = -1;
		private List<CardInHand> cardsSelected = new List<CardInHand>();
		private PokerRules.TurnPhase phase = PokerRules.TurnPhase.Dealer;
		private Dictionary<Player, int> amountMatchedThisRound = new Dictionary<Player, int>();
		private int totalRaiseAmount;


		public void Start()
		{
			pokerRules = GetComponent<PokerRules>();

			GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
			players = new Player[playersGO.Length];
			foreach (GameObject playerGO in playersGO)
			{
				int.TryParse(playerGO.name.Substring(6), out int playerNum);
				Player player = playerGO.GetComponent<Player>();
				players[playerNum - 1] = player;
				amountMatchedThisRound[player] = 0;
			}

			deck.CreateDeck(pokerRules.useJokers);
			for (int i = 0; i < Random.Range(2, 6); ++i)
				deck.Shuffle();

			StartCoroutine(StartGame());
		}


		private IEnumerator StartGame()
		{
			yield return StartCoroutine(Deal());
			yield return StartCoroutine(CollectAnte());

			phase = PokerRules.TurnPhase.Bet;
			currentPlayerIndex = 0;
			players[currentPlayerIndex].StartTurn(betting, 0);
		}

		/// <summary>
		/// Deal a full hand to all players.
		/// </summary>
		private IEnumerator Deal()
		{
			for (int i = 0; i < pokerRules.MaxCardsInHand; ++i)
			{
				foreach (Player player in players)
				{
					player.AddCardToHand(deck.DrawCard());
					yield return new WaitForSeconds(.1f);
				}
			}
		}

		private IEnumerator CollectAnte()
		{
			foreach (var player in players)
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
			players[currentPlayerIndex].RemoveCards(cardsSelected);
			cardsSelected.Clear();
			StartCoroutine(DealCardsTo(cardCount, players[currentPlayerIndex]));
		}

		public bool IsPlayersTurn(Player player)
		{
			if (currentPlayerIndex != -1)
				return player == players[currentPlayerIndex];
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


		private IEnumerator DealCardsTo(int cardCount, Player player)
		{
			for (int i = 0; i < cardCount; ++i)
			{
				yield return new WaitForSeconds(.5f);
				player.AddCardToHand(deck.DrawCard());
			}

			NextPlayerBet();
		}

		private void NextPlayerBet()
		{
			players[currentPlayerIndex++].EndTurn();
			if (currentPlayerIndex >= players.Length)
				currentPlayerIndex = 0;

			int amtMatched = amountMatchedThisRound[players[currentPlayerIndex]];

			players[currentPlayerIndex].StartTurn(betting, totalRaiseAmount - amtMatched);
		}

		public void RaiseBet(Player raisingPlayer, int amtRaised, int matched)
		{
			totalRaiseAmount += amtRaised;
			pot.DisplayRaiseAmount(totalRaiseAmount);
			amountMatchedThisRound[raisingPlayer] += amtRaised + matched;
			pot.AddToPot(amtRaised + matched);
			StartCoroutine(DelayedFunction());
		}

		private IEnumerator DelayedFunction()
		{
			yield return new WaitForSeconds(.5f);
			NextPlayerBet();
		}
	}
}