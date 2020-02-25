using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class Ranking : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI firstPlace = null;
		[SerializeField] private TextMeshProUGUI secondPlace = null;
		[SerializeField] private TextMeshProUGUI thirdPlace = null;

		private List<Horse> ranking = new List<Horse>();


		public void RacerFinished(Horse horse)
		{
			ranking.Add(horse);
			switch (ranking.Count)
			{
				case 1:
					gameObject.SetActive(true);
					firstPlace.text = horse.name;
					break;
				case 2:
					secondPlace.text = horse.name;
					break;
				case 3:
					thirdPlace.text = horse.name;
					break;
			}
		}
	}
}