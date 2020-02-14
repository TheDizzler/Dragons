using UnityEngine;

namespace AtomosZ.Dragons
{
	public class Rules : MonoBehaviour
	{
		public const int MaxCardsInHand = 5;
		public const int MaxCardReplaceAmount = 4;
		// Each player gets 5 cards
		// Player turn order:
		//		Lay their hand down and end their turn
		//			(Move to End Round)
		//		Choose to replace from 1 to 4 cards in their hand
		//		

		// End Round:
		// 1. Each player, in turn starting with player after the player who played their hand,
		// has the opportunity to replace 1 to 4 cards. They must play the hand they have.
		// 2. Points are tallied. Player with highest tally wins that round.

		// Tallying points:
		// Each card is worth its numerical value (except the Dragon cards),
		//	however you can count cards from one suit only.
		// 4 of kind is equal to 7 points?
		// 5 of kind is equal to 10 points?									Odds: 2520/79M
		// 1-4 Dragon cards multiply your total:
		//		1 Dragon	x2 points		Max: 28	((2 + 3 + 4 + 5) * 2	Odds: 210/79M
		//		2 Dragons	x3 points		Max: 36	((3 + 4 + 5) * 3)		Odds: 280/79M
		//		3 Dragons	x4				Max: 36	((4 + 5) * 4)			Odds: 420/79M
		//		4 Dragons	x5				Max: 25 (5 * 5)					Odds: 840/79M
		// 5 Dragons nets you 50 points										Odds: 120/79M
	}
}