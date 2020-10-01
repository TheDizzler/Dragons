using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace AtomosZ.Gambal.Poker
{
	public class CardInHand : MonoBehaviour, IPointerClickHandler
	{
		/// <summary>
		/// This needs to be adaptive to the screen aspect ratio.
		/// .5f works for 5:4
		/// </summary>
		private const float selectPosYOffset = .5f;
		private static Color SelectedColor = Color.cyan;
		private static Color NormalColor = Color.white;

		private Dealer dealer;
		private Player owner;
		[SerializeField] private Card card = null;
		private float selectSlideSpeed = 100f;
		private float startPosY;
		private float selectPosY;

		private bool isSelected = false;
		private Coroutine sliding;


		public void Start()
		{
			dealer = GameObject.FindGameObjectWithTag("Dealer").GetComponent<Dealer>();
		}

		public Card GetCard()
		{
			return card;
		}

		public void SetCard(Card crd, Player player)
		{
			card = crd;
			owner = player;
			GetComponent<Image>().sprite = card.sprite;

			if (isSelected)
				Deselect();
			else
			{
				startPosY = transform.position.y;
				selectPosY = startPosY + selectPosYOffset;
			}
		}

		public void NullifyCard()
		{
			card = Deck.nullCard;
			GetComponent<Image>().sprite = Deck.nullCard.sprite;
			GetComponent<Image>().color = NormalColor;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (dealer.IsPlayersTurn(owner) && dealer.CurrentPhase() == PokerRules.TurnPhase.Draw)
				Select();
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
				Deselect();
				dealer.UnsetCardSelected(this);
			}
		}

		public void SetPosition(Vector3 newpos)
		{
			transform.position = newpos;
			startPosY = newpos.y;
			selectPosY = startPosY + selectPosYOffset;
		}

		private void Deselect()
		{
			isSelected = false;
			GetComponent<Image>().color = NormalColor;
			sliding = StartCoroutine(SlideDown());
		}

		private IEnumerator SlideUp()
		{
			Transform trans = this.transform;
			Vector2 newpos = trans.position;

			while (trans.position.y < selectPosY)
			{
				newpos.y += selectSlideSpeed * Time.deltaTime * 2;
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