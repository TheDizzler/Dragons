using System.Collections.Generic;
using AtomosZ.UI;
using TMPro;
using UnityEngine;
using static AtomosZ.Gambale.Keiba.WagerUI.Wagers;

namespace AtomosZ.Gambale.Keiba.WagerUI
{
	public class WagerManager : MonoBehaviour
	{
		[SerializeField] private Wagers wagers = null;
		[SerializeField] private Spinner spinner = null;
		[SerializeField] private RacerSelectDisplay rsd = null;
		[SerializeField] private GameObject wagerPanel = null;
		[SerializeField] private GameObject payoutPanel = null;
		[SerializeField] private TextMeshProUGUI payoutText = null;
		[SerializeField] private RaceManager raceManager = null;
		

		private WagerType wagerType;
		private List<Horse> picks;
		private int amountWagered;


		public void Start()
		{
			rsd.DisplayRacers(raceManager.GetRacers());
		}

		public void PlaceBet()
		{
			wagerType = wagers.GetWagerType();
			picks = rsd.GetSelectedRacers();
			amountWagered = spinner.currentValue;
			raceManager.StartRace();
			wagerPanel.SetActive(false);
		}

		public List<int> Payout(List<Horse> ranking)
		{
			List<int> winners = new List<int>();
			int payoutAmount = 0;
			
			switch (wagerType)
			{
				case WagerType.Win:
					if (picks[0] == ranking[0])
					{
						Debug.Log("the winningest");
						payoutAmount += 1000;
						winners.Add(0);
					}
					break;
				case WagerType.Place:
					for (int i = 0; i < ranking.Count; ++i)
					{
						if (picks[0] == ranking[i])
						{
							Debug.Log("Place!");
							payoutAmount += 500;
							winners.Add(i);
						}
					}
					break;
				case WagerType.Show:
					for (int i = 0; i < ranking.Count; ++i)
					{
						if (picks[0] == ranking[i])
						{
							Debug.Log("Show!");
							payoutAmount += 20;
							winners.Add(i);
						}
					}
					break;
			}

			payoutPanel.SetActive(true);
			payoutText.text = "$" + payoutAmount;
			return winners;
		}
	}
}