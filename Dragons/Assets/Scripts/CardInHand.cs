using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AtomosZ.Dragons.Deck;
using Color = UnityEngine.Color;

namespace AtomosZ.Dragons
{
	public class CardInHand : MonoBehaviour, IPointerClickHandler
	{
		private const float selectPosYOffset = 25;
		private static Color SelectedColor = Color.cyan;
		private static Color NormalColor = Color.white;

		private Dealer dealer;
		private Card card;
		private float selectSlideSpeed = 50f;
		private float startPosY;
		private float selectPosY;

		private bool isSelected = false;
		private Coroutine sliding;


		public void Start()
		{
			GetComponentInChildren<Text>().text = card.suit + "\n\n" + card.value;
			dealer = GameObject.FindGameObjectWithTag("Dealer").GetComponent<Dealer>();
		}

		public Card GetCard()
		{
			return card;
		}

		public void SetCard(Card crd)
		{
			card = crd;
		}

		/// <summary>
		/// On user select with mouse.
		/// </summary>
		public void Select()
		{
			if (sliding != null)
			{
				StopCoroutine(sliding);
				sliding = null;
			}

			if (!isSelected)
			{
				if (!dealer.SetCardSelected(this))
				{
					// too many cards selected
					return;
				}

				isSelected = true;
				GetComponent<Image>().color = SelectedColor;
				sliding = StartCoroutine(SlideUp());
			}
			else
			{
				isSelected = false;
				GetComponent<Image>().color = NormalColor;
				sliding = StartCoroutine(SlideDown());
				dealer.UnsetCardSelected(this);				
			}

		}

		public void SetPosition(Vector3 newpos)
		{
			transform.position = newpos;
			startPosY = newpos.y;
			selectPosY = startPosY + selectPosYOffset;
		}

		public void Deselect()
		{
			GetComponent<Image>().color = Color.white;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			Select();
		}

		private IEnumerator SlideUp()
		{
			Transform trans = this.transform;
			Vector2 newpos = trans.position;

			while (trans.position.y < selectPosY)
			{
				newpos.y += selectSlideSpeed * Time.deltaTime;
				trans.position = newpos;
				yield return null;
			}

			newpos.y = selectPosY;
			trans.position = newpos;
			sliding = null;
		}

		private IEnumerator SlideDown()
		{
			Transform trans = this.transform;
			Vector2 newpos = trans.position;

			while (trans.position.y > startPosY)
			{
				newpos.y -= selectSlideSpeed * Time.deltaTime * 2;
				trans.position = newpos;
				yield return null;
			}

			newpos.y = startPosY;
			trans.position = newpos;
			sliding = null;
		}
	}
}