using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambale.Poker
{
	/// <summary>
	/// Actually a manager class with a clever name.
	/// </summary>
	public class Dealer : MonoBehaviour
	{
		public int totalBets = 0;
		public int totalRaise = 0;

		[SerializeField] private Deck deck = null;
		[SerializeField] private List<Player> players = null;

		private int currentPlayerIndex;
		private List<CardInHand> cardsSelected = new List<CardInHand>();


		public void Start()
		{
			foreach (GameObject playerGO in GameObject.FindGameObjectsWithTag("Player"))
			{
				players.Add(playerGO.GetComponent<Player>());
			}

			deck.CreateDeck(PokerRules.useJokers);
			for (int i = 0; i < Random.Range(2, 6); ++i)
				deck.Shuffle();

			StartCoroutine(Deal());
		}


		public void ReplaceCards()
		{
			int cardCount = cardsSelected.Count;
			players[currentPlayerIndex].RemoveCards(cardsSelected);
			cardsSelected.Clear();
			StartCoroutine(DealCardsTo(cardCount, players[currentPlayerIndex]));
		}

		public Player GetCurrentPlayer()
		{
			return players[currentPlayerIndex];
		}


		/// <summary>
		/// Return false when too many cards selected. Otherwise adds card too throwaway list and returns true.
		/// </summary>
		/// <param name="cardInHand"></param>
		/// <returns>false when too many cards selected</returns>
		public bool SetCardSelected(CardInHand cardInHand)
		{
			if (cardsSelected.Count >= PokerRules.MaxCardReplaceAmount)
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

			NextPlayer();
		}

		private void NextPlayer()
		{
			if (deck.InsufficientCards(1))
			{
				// end game
				Debug.Log("No more cards; End game.");

				return;
			}

			players[currentPlayerIndex++].EndTurn();
			if (currentPlayerIndex >= players.Count)
				currentPlayerIndex = 0;
			players[currentPlayerIndex].StartTurn();
		}

		/// <summary>
		/// Deal a full hand to all players.
		/// </summary>
		private IEnumerator Deal()
		{
			for (int i = 0; i < PokerRules.MaxCardsInHand; ++i)
			{
				foreach (Player player in players)
				{
					player.AddCardToHand(deck.DrawCard());
					yield return new WaitForSeconds(.3f);
				}
			}

			currentPlayerIndex = 0;
			players[currentPlayerIndex].StartTurn();
		}
	}
}