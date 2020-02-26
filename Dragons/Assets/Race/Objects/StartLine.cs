using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class StartLine : MonoBehaviour
	{
		public bool started = false;
		public List<GameObject> startPlaceHolders = new List<GameObject>();
		[SerializeField] private Ranking ranking = null;

		private BoxCollider finishLine;
		


		public void Start()
		{
			finishLine = GetComponent<BoxCollider>();
		}

		public void OnTriggerEnter(Collider other)
		{
			if (started && other.CompareTag("Racer"))
			{
				Debug.Log("Racer found: " + other.GetComponent<Horse>().name);
				ranking.RacerFinished(other.GetComponent<Horse>());
			}
		}
	}
}