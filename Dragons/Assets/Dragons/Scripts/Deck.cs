using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AtomosZ.Dragons
{
	public class Deck : MonoBehaviour
	{
		private List<Card> deck = new List<Card>();
		//{
		//	new Card(Suit.A, 1), new Card(Suit.A, 2),new Card(Suit.A, 3), new Card(Suit.A, 4),new Card(Suit.A, 5),
		//	new Card(Suit.B, 1), new Card(Suit.B, 2),new Card(Suit.B, 3), new Card(Suit.B, 4),new Card(Suit.B, 5),
		//	new Card(Suit.C, 1), new Card(Suit.C, 2),new Card(Suit.C, 3), new Card(Suit.C, 4),new Card(Suit.C, 5),
		//	new Card(Suit.D, 1), new Card(Suit.D, 2),new Card(Suit.D, 3), new Card(Suit.D, 4),new Card(Suit.D, 5),
		//	new Card(Suit.E, 1), new Card(Suit.E, 2),new Card(Suit.E, 3), new Card(Suit.E, 4),new Card(Suit.E, 5),
		//	new Card(Suit.F, 1), new Card(Suit.F, 2),new Card(Suit.F, 3), new Card(Suit.F, 4),new Card(Suit.F, 5),
		//	new Card(Suit.G, 1), new Card(Suit.G, 2),new Card(Suit.G, 3), new Card(Suit.G, 4),new Card(Suit.G, 5),
		//	new Card(Suit.Dragon, 1), new Card(Suit.Dragon, 2),new Card(Suit.Dragon, 3), new Card(Suit.Dragon, 4),new Card(Suit.Dragon, 5),
		//};


		public void Start()
		{
			CreateDeck();
		}

		public void CreateDeck()
		{
			foreach (Suit suit in (Suit[])System.Enum.GetValues(typeof(Suit)))
			{
				for (int i = 1; i <= 5; ++i)
					deck.Add(new Card(suit, i));
			}

		}

		public void Shuffle()
		{
			Debug.Log("Shuffle - " + deck.Count + " cards in deck");

			for (int i = 0; i < deck.Count; i += 2)
			{
				Card moving = deck[i];
				int randomI = Random.Range(0, deck.Count);
				deck[i] = deck[randomI];
				deck[randomI] = moving;
			}

			for (int i = 1; i < deck.Count; i += 2)
			{
				Card moving = deck[i];
				int randomI = Random.Range(0, deck.Count);
				deck[i] = deck[randomI];
				deck[randomI] = moving;
			}
		}

		public Card DrawCard()
		{
			Card topCard = deck[0];
			deck.RemoveAt(0);
			return topCard;
		}

		public bool InsufficientCards(int cardCount)
		{
			return deck.Count <= cardCount;
		}
	}

	public enum Suit
	{
		A, B, C, D, E, F, G, Dragon
	};

	public class Card
	{
		public Suit suit;
		public int value;

		public Card(Suit s, int val)
		{
			suit = s;
			value = val;
		}
	};
}