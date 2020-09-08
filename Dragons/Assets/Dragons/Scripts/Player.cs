using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambal.Poker
{
	public class Player : MonoBehaviour
	{
		public int funds = 100;
		public int amountPaidOfRaise = 0;

		[SerializeField] private HandVisualizer handPanel = null;

		private List<Card> hand = new List<Card>();



		public void AddCardToHand(Card newcard)
		{
			hand.Add(newcard);
			handPanel.AddCardToHand(newcard, this);
		}

		private void CheckBestHand()
		{
			
		}

		public void EndTurn()
		{
			handPanel.SetActiveTurn(false);
			CheckBestHand();
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

		public void StartTurn()
		{
			handPanel.SetActiveTurn(true);
			CheckBestHand();
		}
	}
}