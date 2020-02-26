using System.Collections.Generic;
using UnityEngine;
using static AtomosZ.Gambale.Keiba.Wagers;

namespace AtomosZ.Gambale.Keiba
{
	public class WagerManager : MonoBehaviour
	{
		[SerializeField] private Wagers wagerType = null;
		[SerializeField] private RacerSelectDisplay rsd = null;
		[SerializeField] private RaceManager raceManager = null;

		public void Start()
		{
			rsd.DisplayRacers(raceManager.GetRacers());
		}

		public void PlaceBet()
		{
			WagerType type = wagerType.GetWagerType();
			List<Horse> picks = rsd.GetSelectedRacers();
			int amount = 100; // temp
			raceManager.StartRace();
			this.gameObject.SetActive(false);
		}
	}
}