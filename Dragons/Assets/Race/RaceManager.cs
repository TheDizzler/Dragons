using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambale.Keiba
{
	public class RaceManager : MonoBehaviour
	{
		[SerializeField] private TrafficLights startLight = null;
		[SerializeField] private StartLine startLine = null;
		[SerializeField] private GameObject focusPanel = null;
		[SerializeField] private GameObject focusButton = null;
		[SerializeField] private List<GameObject> racerGOs = new List<GameObject>();
		
		private CameraController mainCamera;
		private List<Horse> racers = new List<Horse>();
		private float time = 0;


		public void OnEnable()
		{
			mainCamera = Camera.main.GetComponent<CameraController>();
			int i = 0;
			foreach (GameObject racer in racerGOs)
			{
				GameObject newRacer = Instantiate(racer,
					startLine.startPlaceHolders[i].transform.position, Quaternion.identity);
				racers.Add(newRacer.GetComponent<Horse>());
				GameObject newButton = Instantiate(focusButton, focusPanel.transform);
				newButton.GetComponent<Button>().onClick.AddListener(delegate { FocusOnRider(newRacer); });
				newButton.GetComponentInChildren<Text>().text = newRacer.name;
				++i;
			}

			mainCamera.SetFocus(racers[0].gameObject);
			StartCoroutine(Countdown());
		}


		public void FocusOnRider(GameObject racer)
		{
			mainCamera.SetFocus(racer);
		}

		private IEnumerator Countdown()
		{
			startLight.RedLight();
			while (time < 3)
			{
				yield return null;
				time += Time.deltaTime;
			}

			startLight.YellowLight();

			while (time < 4)
			{
				yield return null;
				time += Time.deltaTime;
			}

			startLight.GreenLight();
			foreach (Horse racer in racers)
				racer.StartRace();

			while (time < 6)
			{
				yield return null;
				time += Time.deltaTime;
			}

			startLine.started = true;
		}
	}
}