using System.Collections.Generic;
using UnityEngine;
using static AtomosZ.Dragons.Deck;

namespace AtomosZ.Dragons
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private HandVisualizer handPanel = null;

		private List<Card> hand = new List<Card>();



		public void AddCardToHand(Card newcard)
		{
			hand.Add(newcard);
			handPanel.DisplayNewCard(newcard);
			CheckScore();
		}

		private void CheckScore()
		{
			Dictionary<Suit, int> scores = new Dictionary<Suit, int>();
			int dragonCount = 1;

			foreach (Card card in hand)
			{
				if (scores.TryGetValue(card.suit, out int value))
				{
					scores[card.suit] = value + card.value;
				}
				else
				{
					if (card.suit == Suit.Dragon)
					{
						++dragonCount;
					}
					else
						scores.Add(card.suit, card.value);
				}
			}

			string valueCheck = "";
			foreach (KeyValuePair<Suit, int> kvp in scores)
			{
				valueCheck += kvp.Key + " = " + (kvp.Value * dragonCount) + ";";
			}

			if (dragonCount == 6)
				valueCheck = "Dragon Strike! 50 points!";

			Debug.Log(valueCheck);
		}

		public bool EndTurn()
		{
			if (hand.Count > Rules.MaxCardsInHand)
			{
				return false;
			}

			return true;
		}

		public void RemoveCards(List<CardInHand> cardsSelected)
		{
			foreach (CardInHand inHand in cardsSelected)
			{
				if (!hand.Remove(inHand.GetCard()))
				{
					Debug.LogError("Player.RemoveCards()::could not remove card from hand."
						+ "\nCard " + inHand.GetCard().suit + " - " + inHand.GetCard().value);
				}

				handPanel.RemoveCard(inHand);
			}
		}
	}
}