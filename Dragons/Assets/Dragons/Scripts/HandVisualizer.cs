using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambal.Poker
{
	public class HandVisualizer : MonoBehaviour
	{
		private static Color turnColor = new Color(255, 0, 255, 100);
		private static Color waitColor = new Color(255, 255, 255, 100);



		[SerializeField] private GameObject[] cardPlaceholders = null;
		[SerializeField] private Text score = null;
		[SerializeField] private GameObject fundsChangedTextPrefab = null;
		[SerializeField] private Image bgImage = null;

		private List<CardInHand> heldCards = new List<CardInHand> { null, null, null, null, null };
		private Player owner;


		void Start()
		{
			cardPlaceholders = new GameObject[5];
			for (int i = 0; i < transform.GetChild(0).childCount; ++i)
			{
				cardPlaceholders[i] = transform.GetChild(0).GetChild(i).gameObject;
			}
		}

		public void SetOwner(Player player)
		{
			owner = player;
		}

		public void SetFundsText(int funds)
		{
			score.text = "$" + funds;
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

		public void MoneyChanged(int amount)
		{
			score.text = "$" + owner.funds;
			StartCoroutine(MoneyDrain(amount));
		}

		private void SetScore(int newScore)
		{
			score.text = newScore.ToString();
		}

		public void SetActiveTurn(bool isTurn)
		{
			if (isTurn)
				bgImage.color = turnColor;
			else
				bgImage.color = waitColor;
		}


		private IEnumerator MoneyDrain(int amountDrained)
		{
			float timeToDrain = 1;
			GameObject label = Instantiate(fundsChangedTextPrefab, transform.parent.parent.parent);
			label.transform.position = score.transform.position + (Vector3)((RectTransform)score.transform).sizeDelta * .5f;
			label.GetComponent<TextMeshProUGUI>().text = "$" + amountDrained;
			while (timeToDrain > 0)
			{
				yield return null;
				timeToDrain -= Time.deltaTime;
				label.transform.position += new Vector3(0, 50 * Time.deltaTime, 0);
			}

			Destroy(label);
		}
	}
}