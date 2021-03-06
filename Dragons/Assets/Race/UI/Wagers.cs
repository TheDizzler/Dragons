﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambal.Keiba.WagerUI
{
	public class Wagers : MonoBehaviour
	{
		/** Types of bets:
		 *		Show		- pick a horse to finish in 1st, 2nd or 3rd
		 *		Place		- pick a horse to finish in 1st or 2nd
		 *		Win			- pick a horse to finish in 1st
		 *		
		 *		
		 *		Exacta		- pick 1st and 2nd place winners (in order)
		 *		Exacta Box	- pick 1st and 2nd place winners (any order) (same as Quinella?)
		 *		Trifecta	- pick 1st, 2nd and 3rd place winners (in order)
		 *		Trifecta Box- pick 1st, 2nd and 3rd place winners (any order) (also Trio?)
		 *		Superfecta	- pick 1st, 2nd, 3rd and 4th place winners (in order)
		 *		
		 *	Other types?
		 *		Quinella	- pick 2 horses to finish in 1st AND 2nd (any order)
		 *		Bracke Quinella	- unique to Japan
		 *		Across The Board	- pick 1st, 2nd, or 3rd (any order)
		 * */

		public enum WagerType { Win, Place, Show };
		private static Color SelectedColor = Color.black;
		private static Color UnselectedColor = new Color(0f, 0f, 0f, .75f);

		[SerializeField] private Slider wagerTypeSlider = null;
		[SerializeField] private TextMeshProUGUI winLabel = null;
		[SerializeField] private TextMeshProUGUI placeLabel = null;
		[SerializeField] private TextMeshProUGUI showLabel = null;
		[SerializeField]
		private WagerInfoPanel wagerInfoPanel = null;

		private WagerType type = WagerType.Win;


		public void Start()
		{
			WagerTypeSliderChanged();
		}


		public void WagerTypeSliderChanged()
		{
			winLabel.color = UnselectedColor;
			placeLabel.color = UnselectedColor;
			showLabel.color = UnselectedColor;

			switch (wagerTypeSlider.value)
			{
				case 0: // Pick winner
					winLabel.color = SelectedColor;
					break;
				case 1: // Pick horse to place 1st or 2nd
					placeLabel.color = SelectedColor;
					break;
				case 2: // pick horse to place 1st, 2nd or 3rd
					showLabel.color = SelectedColor;
					break;
			}
			type = (WagerType)wagerTypeSlider.value;
			wagerInfoPanel.SetWagerInfoText(type);
		}

		public WagerType GetWagerType()
		{
			return type;
		}
	}
}