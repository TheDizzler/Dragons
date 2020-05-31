using AtomosZ.Gambale.Keiba.WagerUI;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class SceneInitializer : MonoBehaviour
	{
		public GameObject focusPanel;
		public GameObject rankingPanel;
		public GameObject payoutPanel;
		public GameObject wagerPanel;


		void Start()
		{
			if (focusPanel.activeInHierarchy)
				focusPanel.SetActive(false);
			if (rankingPanel.activeInHierarchy)
				rankingPanel.SetActive(false);
			if (payoutPanel.activeInHierarchy)
				payoutPanel.SetActive(false);

			if (!wagerPanel.activeInHierarchy)
				wagerPanel.SetActive(true);
		}
	}
}