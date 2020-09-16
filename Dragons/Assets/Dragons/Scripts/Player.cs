using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambal.Poker
{
	public class Player : MonoBehaviour
	{
		public int funds = 100;

		[SerializeField] private HandVisualizer handPanel = null;

		private List<Card> hand = new List<Card>();


		public void Start()
		{
			handPanel.SetOwner(this);
			handPanel.SetFundsText(funds);
		}

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="betting"></param>
		/// <param name="match">Amount player has to match to stay in game. </param>
		public void StartTurn(Betting betting, int matchNeeded)
		{
			betting.SetPlayer(this, matchNeeded);
			handPanel.SetActiveTurn(true);
			CheckBestHand();
		}

		public int SubtractFunds(int anteAmount)
		{
			if (funds < anteAmount)
			{
				Debug.LogWarning(name + " must drop out!");
			}

			handPanel.MoneyChanged(-anteAmount);
			funds -= anteAmount;
			return anteAmount;
		}
	}
}