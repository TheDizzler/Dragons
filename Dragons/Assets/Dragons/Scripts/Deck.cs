using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace AtomosZ.Gambale.Poker
{
	public class Deck : MonoBehaviour
	{
		public static Card nullCard;

		protected List<Card> deck = new List<Card>();
		[SerializeField] private SpriteAtlas deckAtlas = null;


		public void Shuffle()
		{
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

		public void CreateDeck(bool useJokers)
		{
			nullCard = new Card(Suit.Blank, -1, deckAtlas.GetSprite("CardPlaceholder"));
			foreach (Suit suit in (Suit[])System.Enum.GetValues(typeof(Suit)))
			{
				if (suit == Suit.Blank)
					continue;
				if (suit == Suit.Joker && useJokers)
				{
						deck.Add(new Card(suit, 0, deckAtlas.GetSprite(suit + "_0")));
						deck.Add(new Card(suit, 0, deckAtlas.GetSprite(suit + "_1")));
				}
				else
				{
					for (int i = 1; i <= 13; ++i)
					{
						string value = "_";
						switch (i)
						{
							case 1:
								value += "A";
								break;
							case 11:
								value += "J";
								break;
							case 12:
								value += "Q";
								break;
							case 13:
								value += "K";
								break;
							default:
								value += i;
								break;
						}

						deck.Add(new Card(suit, i, deckAtlas.GetSprite(suit + value)));
					}
				}
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
		Blank,
		Hearts, Clubs, Diamonds, Spades, Joker
	}


	[System.Serializable]
	public class Card
	{
		public Suit suit;
		public int value;
		public Sprite sprite;

		public Card(Suit s, int val, Sprite sprt)
		{
			suit = s;
			value = val;
			sprite = sprt;
		}
	};
}