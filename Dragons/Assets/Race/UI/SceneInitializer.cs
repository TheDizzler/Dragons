using UnityEngine;

namespace AtomosZ.Gambal.Keiba
{
	public class SceneInitializer : MonoBehaviour
	{
		public RaceManager raceManager;
		public GameObject focusPanel;
		public GameObject rankingPanel;
		public GameObject payoutPanel;
		public GameObject wagerPanel;


		void Start()
		{
			focusPanel.SetActive(false);
			rankingPanel.SetActive(false);
			payoutPanel.SetActive(false);
			wagerPanel.SetActive(false);


		}

		void Update()
		{
			raceManager.StartRace();
			this.enabled = false;
		}
	}
}