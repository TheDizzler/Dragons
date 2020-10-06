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

		public class HandRank : IComparable<HandRank>
		{
			public Player player;
			public HandRanking ranking = HandRanking.HighCard;
			/// <summary>
			/// Value 1 == Ace.
			/// </summary>
			public CardValue highestValue = CardValue.NullCard;
			/// <summary>
			/// value, count
			/// </summary>
			public Tuple<CardValue, int> firstPair = null;
			/// <summary>
			/// value, count
			/// </summary>
			public Tuple<CardValue, int> secondPair = null;

			private List<Card> hand;


			public HandRank(Player player, List<Card> hand)
			{
				this.hand = hand;
				hand.Sort();
				GetBestHand();
			}

			/// <summary>
			/// @TODO: Jokers not accounted for.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			public int CompareTo(HandRank other)
			{
				if (ranking == other.ranking)
				{
					switch (ranking)
					{
						case HandRanking.FiveOfAKind:
						case HandRanking.FourOfAKind:
						case HandRanking.ThreeOfAKind:
						case HandRanking.Pair:
							// get highest of kind
							int result = firstPair.Item1.CompareTo(other.firstPair.Item1);
							if (result == 0)
								return FindHighest(other);
							return result;
						case HandRanking.TwoPair:
							CardValue highPair;
							CardValue otherHighPair;
							CardValue lowPair;
							CardValue otherLowPair;

							if (firstPair.Item1.CompareTo(secondPair.Item1) == 1)
							{
								highPair = firstPair.Item1;
								lowPair = secondPair.Item1;
							}
							else
							{
								highPair = secondPair.Item1;
								lowPair = firstPair.Item1;
							}

							if (other.firstPair.Item1.CompareTo(other.secondPair.Item1) == 1)
							{
								otherHighPair = other.firstPair.Item1;
								otherLowPair = other.secondPair.Item1;
							}
							else
							{
								otherHighPair = other.secondPair.Item1;
								otherLowPair = other.firstPair.Item1;
							}

							int highPairResult = highPair.CompareTo(otherHighPair);
							if (highPairResult == 0)
							{
								int lowPairResult = lowPair.CompareTo(otherLowPair);
								if (lowPairResult == 0)
									return FindHighest(other); // if both pairs are equal, find the next highest
								return lowPairResult;
							}

							return highPairResult;

						case HandRanking.FullHouse:
							Tuple<CardValue, int> triple;
							Tuple<CardValue, int> otherTriple;
							Tuple<CardValue, int> pair;
							Tuple<CardValue, int> otherPair;

							if (firstPair.Item2 == 3)
							{
								triple = firstPair;
								pair = secondPair;
							}
							else
							{
								triple = secondPair;
								pair = firstPair;
							}

							if (other.firstPair.Item2 == 3)
							{
								otherTriple = other.firstPair;
								otherPair = other.secondPair;
							}
							else
							{
								otherTriple = other.secondPair;
								otherPair = other.firstPair;
							}


							int tripleCompareResult = triple.Item1.CompareTo(otherTriple);
							if (tripleCompareResult == 0)
								return pair.Item1.CompareTo(otherPair);
							return tripleCompareResult;
					}

					return FindHighest(other);
				}

				if (ranking > other.ranking)
					return 1;
				return -1;
			}

			/// <summary>
			/// Go through all cards until the hand with a higher card wins. If all cards equal, return 0;
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			private int FindHighest(HandRank other)
			{
				for (int i = hand.Count - 1; i >= 0; --i)
				{
					int result = hand[i].CompareTo(other.hand[i]);
					if (result == 0)
						continue;
					return result;
				}

				return 0;
			}

			/// <summary>
			/// Note: cards must be ordered numerically for this to function correctly.
			/// @TODO: Account for Jokers & wildcards (currently only supports 5 of a kind).
			/// </summary>
			public void GetBestHand()
			{
				highestValue = hand[4].value;


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
							// check edge case with ace
							// 10, J, Q, K, Ace
							// 2, 3, 4, 5, Ace
							if (!(card.value == CardValue.Ace
									&& (lastCard.value == CardValue.King
										|| lastCard.value == CardValue.Five)))
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
									firstPair = new Tuple<CardValue, int>(card.value, 2);
								else
									secondPair = new Tuple<CardValue, int>(card.value, 2);
								break;

							case 3:
								if (firstPair.Item1 == card.value)
									firstPair = new Tuple<CardValue, int>(card.value, 3);
								else
									secondPair = new Tuple<CardValue, int>(card.value, 3);
								break;

							case 4:
								firstPair = new Tuple<CardValue, int>(card.value, 4);
								break;

							case 5:
								firstPair = new Tuple<CardValue, int>(card.value, 5);
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
						if (hand[3].value == CardValue.King)
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
									Debug.Log("Two pairs of " + firstPair.Item1 + " and " + secondPair.Item1);
								}
								else
								{
									ranking = HandRanking.FullHouse;
									Debug.Log("Full house of " + firstPair.Item1 + " and " + secondPair.Item1);
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
								Debug.Log("Full house of " + firstPair.Item1 + " and " + secondPair.Item1);
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

		public static List<Player> DetermineWinner(List<Player> activePlayers)
		{
			List<Player> winners = new List<Player>();
			winners.Add(activePlayers[0]);
			HandRank bestHand = activePlayers[0].GetHandRank();

			for (int i = 1; i < activePlayers.Count; ++i)
			{
				HandRank hand = activePlayers[i].GetHandRank();
				int result = hand.CompareTo(bestHand);
				if (result == 1)
				{
					bestHand = hand;
					winners.Clear();
					winners.Add(activePlayers[i]);
				}
				else if (result == 0)
				{
					winners.Add(activePlayers[i]);
				}
			}

			return winners;
		}
	}
}