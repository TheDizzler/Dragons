using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Dragons
{
	public class HandVisualizer : MonoBehaviour
	{
		[SerializeField] private GameObject cardPrefab = null;
		[SerializeField] private GameObject[] cardPlaceholders = null;

		private List<CardInHand> heldCards = new List<CardInHand> { null, null, null, null, null };


		public void DisplayNewCard(Card crd)
		{
			GameObject newcardGO = Instantiate(cardPrefab, this.transform);
			CardInHand card = newcardGO.GetComponent<CardInHand>();
			card.SetCard(crd);

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
	}
}