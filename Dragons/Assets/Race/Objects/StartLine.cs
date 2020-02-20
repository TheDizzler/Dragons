using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class StartLine : MonoBehaviour
	{
		public bool started = false;
		public List<GameObject> startPlaceHolders = new List<GameObject>();

		private BoxCollider finishLine;
		private List<Horse> ranking = new List<Horse>();


		public void Start()
		{
			finishLine = GetComponent<BoxCollider>();
			foreach(GameObject placeHolder in startPlaceHolders)
				Destroy(placeHolder);
		}

		public void OnTriggerEnter(Collider other)
		{
			if (started && other.CompareTag("Racer"))
			{
					Debug.Log("Racer found: " + other.GetComponent<Horse>().name);
			}
		}


	}
}