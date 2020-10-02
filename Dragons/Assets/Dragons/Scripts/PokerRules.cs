using System;
using System.Collections.Generic;
using UnityEngine;
using static AtomosZ.Gambal.Poker.Deck;

namespace AtomosZ.Gambal.Poker
{
	public class PokerRules : MonoBehaviour
	{
		public int MinPlayers = 2;
		public int MaxPlayers = 6;
		public int MaxCardsInHand = 5;
		public int MaxCardDrawAmount = 3;
		public int MaxDrawsAllowed = 3;

		public int ante = 1;
		public int maxRaise = 100;

		public bool useJokers = false;

		public enum TurnPhase
		{
			Dealer,
			Bet,
			Draw
		}

		public enum HandRanking
		{
			HighCard,
			Pair,
			TwoPair,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraighFlush,
			RoyalFlush,
			/// <summary>
			/// Wild Card games only.
			/// </summary>
			FiveOfAKind,
		}

		public class HandRank
		{
			public HandRanking ranking = HandRanking.HighCard;
			/// <summary>
			/// Value 1 == Ace.
			/// </summary>
			public int highestValue = 0;

			private List<Card> hand;


			public HandRank(List<Card> hand)
			{
				this.hand = hand;
				hand.Sort();
				GetBestHand();
			}

			/// <summary>
			/// Note: cards must be ordered numerically for this to function correctly.
			/// @TODO: Account for Jokers & wildcards (currently only supports 5 of a kind).
			/// </summary>
			public void GetBestHand()
			{
				highestValue = hand[0].value == (int)CardValue.Ace ? 1 : hand[4].value;

				// value, count
				Tuple<int, int> firstPair = null;
				Tuple<int, int> secondPair = null;

				int sameValueCount = 0;
				bool run = true;
				bool flush = true;
				Card lastCard = null;

				foreach (Card card in hand)
				{
					if (run && lastCard != null)
					{
						if (lastCard.value != (card.value - 1))
						{
							// check edge case with ace (ace is always a start of hand)
							// Ace, 10, J, Q, K
							if (!(lastCard.value == (int)CardValue.Ace && card.value == 10))
								run = false;
						}
					}

					if (flush && lastCard != null)
					{
						if (lastCard.suit != card.suit)
							flush = false;
					}


					if (lastCard != null && lastCard.value == card.value)
					{
						switch (++sameValueCount)
						{
							case 2:
								if (firstPair == null)
									firstPair = new Tuple<int, int>(card.value, 2);
								else
									secondPair = new Tuple<int, int>(card.value, 2);
								break;

							case 3:
								if (firstPair.Item1 == card.value)
									firstPair = new Tuple<int, int>(card.value, 3);
								else
									secondPair = new Tuple<int, int>(card.value, 3);
								break;

							case 4:
								firstPair = new Tuple<int, int>(card.value, 4);
								break;

							case 5:
								firstPair = new Tuple<int, int>(card.value, 5);
								break;
						}
					}
					else
					{
						sameValueCount = 1;
					}



					lastCard = card;
				}


				if (run)
				{
					if (flush)
					{
						if (hand[4].value == 13)
						{
							ranking = HandRanking.RoyalFlush;
							Debug.Log("Royal Flush!!");
						}
						else
						{
							ranking = HandRanking.StraighFlush;
							Debug.Log("Straight flush with " + highestValue + " high");
						}
					}
					else
					{
						ranking = HandRanking.Straight;
						Debug.Log("Run with " + highestValue + " high");
					}
				}
				else if (flush)
				{
					ranking = HandRanking.Flush;
					Debug.Log("Flush " + highestValue + " high");
				}
				else if (firstPair != null)
				{
					switch (firstPair.Item2)
					{
						case 2:
							if (secondPair == null)
							{
								ranking = HandRanking.Pair;
								Debug.Log("One pair of " + firstPair.Item1);
							}
							else
							{
								if (secondPair.Item2 == 2)
								{
									ranking = HandRanking.TwoPair;
									Debug.Log("Two pairs of " + firstPair.Item1 + " and " + secondPair.Item2
										+ " (remaining card is important in case of tie!)");
								}
								else
								{
									ranking = HandRanking.FullHouse;
									Debug.Log("Full house of " + firstPair.Item1 + " and " + secondPair.Item2);
								}
							}
							break;

						case 3:
							if (secondPair == null)
							{
								ranking = HandRanking.ThreeOfAKind;
								Debug.Log("Three of a kind of " + firstPair.Item1);
							}
							else
							{
								ranking = HandRanking.FullHouse;
								Debug.Log("Full house of " + firstPair.Item1 + " and " + secondPair.Item2);
							}

							break;

						case 4:
							ranking = HandRanking.FourOfAKind;
							Debug.Log("4 of a kind of " + firstPair.Item1);
							break;

						case 5:
							ranking = HandRanking.FiveOfAKind;
							Debug.Log("5 of a kind of " + firstPair.Item1);
							break;
					}
				}
				else
					Debug.Log("High card: " + highestValue);
			}
		}
	}
}