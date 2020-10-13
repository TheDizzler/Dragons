using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using static AtomosZ.Gambal.Poker.Deck;
using static AtomosZ.Gambal.Poker.PokerRules;

namespace AtomosZ.Gambal.Poker
{
	public class PokerPlayer : NetworkBehaviour
	{
		public int funds = 100;
		public HandVisualizer handPanel = null;

		private List<Card> hand = new List<Card>();


		public override void OnStartLocalPlayer()
		{
			GameObject.FindGameObjectWithTag(Tags.Dealer)
		}

		public void Start()
		{
			handPanel.SetOwner(this);
			handPanel.SetFundsText(funds);
		}


		public void ResetHand()
		{
			handPanel.ResetHand();
			hand.Clear();
		}

		public void AddCardToHand(Card newcard)
		{
			if (hand.Count >= 5)
				throw new Exception("Player has too many cards");
			hand.Add(newcard);
			handPanel.AddCardToHand(newcard, this);
		}

		public HandRank GetHandRank()
		{
			return new HandRank(this, hand);
		}

		public void EndTurn()
		{
			handPanel.SetActiveTurn(false);
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


		public void StartDraw()
		{
			handPanel.SetActiveTurn(true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="betting"></param>
		/// <param name="match">Amount player has to match to stay in game. </param>
		public void StartBetting(Betting betting, int matchNeeded)
		{
			betting.SetPlayer(this, matchNeeded);
			handPanel.SetActiveTurn(true);
		}

		public void AddFunds(int winnings)
		{
			funds += winnings;
			handPanel.SetWinner();
			handPanel.MoneyChanged(winnings);
		}

		public int SubtractFunds(int amountChanged)
		{
			if (amountChanged != 0)
			{
				if (funds < amountChanged)
				{
					Debug.LogWarning(name + " must drop out!");
				}

				funds -= amountChanged;
				handPanel.MoneyChanged(-amountChanged);
			}

			return amountChanged;
		}

		public void Fold()
		{
			handPanel.Fold();
		}

		public void RemoveFromGame()
		{
			Destroy(handPanel.gameObject);
		}
	}
}