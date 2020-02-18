using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Dragons
{
	public class HandVisualizer : MonoBehaviour
	{
		private static Color turnColor = new Color(255, 0, 255, 100);
		private static Color waitColor = new Color(255, 255, 255, 100);

		[SerializeField] private GameObject cardPrefab = null;
		[SerializeField] private GameObject[] cardPlaceholders = null;
		[SerializeField] private Text score = null;

		private List<CardInHand> heldCards = new List<CardInHand> { null, null, null, null, null };


		public void DisplayNewCard(Card crd, Player player)
		{
			GameObject newcardGO = Instantiate(cardPrefab, this.transform);
			CardInHand card = newcardGO.GetComponent<CardInHand>();
			card.SetCard(crd, player);

			for (int i = 0; i < heldCards.Count; ++i)
			{
				if (heldCards[i] == null)
				{
					heldCards[i] = card;
					card.SetPosition(cardPlaceholders[i].transform.position);
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
			Destroy(inHand.gameObject);
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