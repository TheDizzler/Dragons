using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambale.Poker
{
	public class HandVisualizer : MonoBehaviour
	{
		private static Color turnColor = new Color(255, 0, 255, 100);
		private static Color waitColor = new Color(255, 255, 255, 100);

		[SerializeField] private GameObject[] cardPlaceholders = null;
		[SerializeField] private Text score = null;

		private List<CardInHand> heldCards = new List<CardInHand> { null, null, null, null, null };


		public void Start()
		{
			cardPlaceholders = new GameObject[5];
			for (int i = 0; i < transform.GetChild(0).childCount; ++i)
			{
				cardPlaceholders[i] = transform.GetChild(0).GetChild(i).gameObject;
			}
		}

		public void AddCardToHand(Card crd, Player player)
		{
			for (int i = 0; i < heldCards.Count; ++i)
			{
				if (heldCards[i] == null)
				{
					CardInHand card = cardPlaceholders[i].GetComponent<CardInHand>();
					card.SetCard(crd, player);
					heldCards[i] = card;
					break;
				}
			}
		}

		public void RemoveCard(CardInHand inHand)
		{
			if (!heldCards.Contains(inHand))
			{
				Debug.LogError("HandVisualizer.RemoveCard()::Error removing card from heldCards.");
				return;
			}

			int i = heldCards.IndexOf(inHand);
			heldCards[i] = null;
			inHand.NullifyCard();
		}

		public void SetScore(int best)
		{
			score.text = best.ToString();
		}

		public void SetActiveTurn(bool isTurn)
		{
			if (isTurn)
				GetComponent<Image>().color = turnColor;
			else
				GetComponent<Image>().color = waitColor;
		}
	}
}