using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambale.Keiba
{
	public class RacerSelectDisplay : MonoBehaviour
	{
		[SerializeField] private GameObject racerTogglePrefab = null;
		[SerializeField] private Button placeBetButton = null;
		private List<Horse> selections = new List<Horse>();


		public void DisplayRacers(List<Horse> racers)
		{
			ToggleGroup toggleGroup = GetComponent<ToggleGroup>();
			foreach (Horse racer in racers)
			{
				GameObject newToggle = Instantiate(racerTogglePrefab, this.transform);
				RacerListing listing = newToggle.GetComponent<RacerListing>();
				listing.racerName = listing.nameText.text = listing.name = racer.name;
				listing.racerImage = racer.portrait;
				listing.horse = racer;
				newToggle.GetComponent<Toggle>().group = toggleGroup;
				newToggle.GetComponent<Toggle>().onValueChanged.AddListener(listing.ToggleChanged);
			}
		}

		public List<Horse> GetSelectedRacers()
		{
			return selections;
		}

		public void SetSelected(Horse horse, bool toggled)
		{
			if (toggled)
				selections.Add(horse);
			else
				selections.Remove(horse);

			placeBetButton.interactable = selections.Count > 0;
		}
	}
}