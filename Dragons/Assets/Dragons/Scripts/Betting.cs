using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambal.Poker
{
	public class Betting : MonoBehaviour
	{
		public Player currentPlayer;
		[SerializeField] private GameObject placeBetPanel = null;
		[SerializeField] private Text raiseAmountText = null;
		[SerializeField] private Text matchAmountText = null;
		[SerializeField] private Slider raiseSlider = null;
		[SerializeField] private Text placeBetButtonText = null;
		[SerializeField] private Button foldButton = null;
		[SerializeField] private Button placeBetButton = null;

		private Dealer dealer;

		private int amountToRaise = 0;
		private int match;


		void Start()
		{
			raiseAmountText.text = "$" + $"{amountToRaise:n0}";
			matchAmountText.text = "$" + $"{match:n0}";
			placeBetButtonText.text = "No Bet";
			dealer = GetComponent<Dealer>();
		}


		public void SetPlayer(Player player, int matchNeeded)
		{
			currentPlayer = player;
			match = matchNeeded;
			raiseSlider.minValue = 0;
			raiseSlider.maxValue = Mathf.Min(dealer.pokerRules.maxRaise, player.funds - match);

			DisplayValues();
			placeBetPanel.SetActive(true);
		}

		private void DisplayValues()
		{
			raiseAmountText.text = "$" + $"{(int)raiseSlider.value:n0}";
			matchAmountText.text = "$" + $"{match:n0}";
			if (amountToRaise == 0)
			{
				if (match != 0)
				{
					placeBetButtonText.text = "Match";
				}
				else
				{
					placeBetButtonText.text = "No Raise";
				}
			}
			else
			{
				placeBetButtonText.text = "Raise";
			}
		}

		public void RaiseValueChanged()
		{
			amountToRaise = (int)raiseSlider.value;
			DisplayValues();
		}

		public void OnRaiseButtonClick()
		{
			currentPlayer.SubtractFunds((int)raiseSlider.value + match);
			dealer.RaiseBet(currentPlayer, amountToRaise, match);

			// reset values
			amountToRaise = 0;
			match = 0;
			raiseSlider.value = 0;
			placeBetPanel.SetActive(false);
			currentPlayer = null;
		}

		public void OnFoldButtonClick()
		{

		}
	}
}