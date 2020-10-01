using UnityEngine;

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

		public enum Scoring
		{
			NoPair,
			OnePair,
			TwoPair,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraighFlush,
			/// <summary>
			/// Wild Card games only.
			/// </summary>
			FiveOfAKind,
		}
	}
}