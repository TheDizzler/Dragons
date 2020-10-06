using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace AtomosZ.Gambal.Poker
{
	public class Deck : MonoBehaviour
	{
		public static Card nullCard;

		public enum Suit
		{
			Blank,
			Hearts, Clubs, Diamonds, Spades, Joker
		}

		public enum CardValue
		{
			NullCard = -1,
			Ace = 14, // forced to highest value in case of comparing CardValues directly (instead of through Card)
			Two = 2,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten,
			Jack = 11,
			Queen = 12,
			King = 13,
			Joker = 0,
		}


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
			nullCard = new Card(Suit.Blank, CardValue.NullCard, deckAtlas.GetSprite("CardPlaceholder"));
			deck.Clear();
			foreach (Suit suit in (Suit[])System.Enum.GetValues(typeof(Suit)))
			{
				if (suit == Suit.Blank)
					continue;
				if (suit == Suit.Joker)
				{
					if (!useJokers)
						continue;
					deck.Add(new Card(suit, (int)CardValue.Joker, deckAtlas.GetSprite(suit + "_0")));
					deck.Add(new Card(suit, (int)CardValue.Joker, deckAtlas.GetSprite(suit + "_1")));
				}
				else
				{
					foreach (CardValue cardValue in (CardValue[])System.Enum.GetValues(typeof(CardValue)))
					{
						string value = "_";
						switch (cardValue)
						{
							case CardValue.Joker:
							case CardValue.NullCard:
								continue;
							case CardValue.Ace:
								value += "A";
								break;
							case CardValue.Jack:
								value += "J";
								break;
							case CardValue.Queen:
								value += "Q";
								break;
							case CardValue.King:
								value += "K";
								break;
							default:
								value += (int)cardValue;
								break;
						}

						Sprite sprite = deckAtlas.GetSprite(suit + value);
						if (sprite == null)
							throw new System.Exception("No texture found for " + suit + " " + value);

						deck.Add(new Card(suit, cardValue, sprite));
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





		[System.Serializable]
		public class Card : IComparable<Card>
		{
			public Suit suit;
			public CardValue value;
			public Sprite sprite;

			public Card(Suit s, CardValue val, Sprite sprt)
			{
				suit = s;
				value = val;
				sprite = sprt;
			}

			/// <summary>
			/// Aces considered highest value.
			/// @TODO: Need to account for holding Jokers
			/// </summary>
			/// <param name="otherCard"></param>
			/// <returns></returns>
			public int CompareTo(Card otherCard)
			{
				if (value == otherCard.value)
					return 0;
				if (value > otherCard.value)
					return 1;
				return -1;
			}
		}
	}
}