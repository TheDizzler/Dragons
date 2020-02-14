using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Dragons
{
	/// <summary>
	/// Actually a manager class with a clever name.
	/// </summary>
	public class Dealer : MonoBehaviour
	{
		[SerializeField] private Deck deck = null;
		[SerializeField] private List<Player> players = null;

		private Player currentPlayer;
		private List<CardInHand> cardsSelected = new List<CardInHand>();


		public void Start()
		{
			foreach (GameObject playerGO in GameObject.FindGameObjectsWithTag("Player"))
			{
				players.Add(playerGO.GetComponent<Player>());
			}

			deck.Shuffle();

			StartCoroutine(Deal());
		}


		public void ReplaceCards()
		{
			int cardCount = cardsSelected.Count;
			currentPlayer.RemoveCards(cardsSelected);
			cardsSelected.Clear();
			StartCoroutine(DealCardsTo(cardCount, currentPlayer));
		}


		/// <summary>
		/// Return false when too many cards selected. Otherwise adds card too throwaway list and returns true.
		/// </summary>
		/// <param name="cardInHand"></param>
		/// <returns>false when too many cards selected</returns>
		public bool SetCardSelected(CardInHand cardInHand)
		{
			if (cardsSelected.Count >= Rules.MaxCardReplaceAmount)
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
		}

		/// <summary>
		/// Deal a full hand to all players.
		/// </summary>
		private IEnumerator Deal()
		{
			for (int i = 0; i < Rules.MaxCardsInHand; ++i)
			{
				foreach (Player player in players)
				{
					player.AddCardToHand(deck.DrawCard());
					yield return new WaitForSeconds(.3f);
				}
			}

			currentPlayer = players[0];
		}
	}
}