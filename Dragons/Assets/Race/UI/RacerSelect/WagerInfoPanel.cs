using TMPro;
using UnityEngine;
using static AtomosZ.Gambale.Keiba.WagerUI.Wagers;

namespace AtomosZ.Gambale.Keiba.WagerUI
{
	public class WagerInfoPanel : MonoBehaviour
	{
		private TextMeshProUGUI infoText = null;


		public void SetWagerInfoText(WagerType wagerType)
		{
			if (infoText == null)
				infoText = GetComponent<TextMeshProUGUI>();

			switch (wagerType)
			{
				case WagerType.Win:
					infoText.text = "Pick 1st place winner";
					break;
				case WagerType.Place:
					infoText.text = "Pick horse to place 1st or 2nd";
					break;
				case WagerType.Show:
					infoText.text = "Pick horse to place 1st, 2nd or 3rd";
					break;
			}
		}

	}
}