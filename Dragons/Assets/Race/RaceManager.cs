using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambale.Keiba
{
	public class RaceManager : MonoBehaviour
	{
		public static Waypoint FirstWaypoint = null;


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
			FirstWaypoint = GameObject.Find("First Waypoint").GetComponent<Waypoint>();
			int i = 0;
			foreach (GameObject racer in racerGOs)
			{
				Vector3 startPos = startLine.startPlaceHolders[i].transform.position;
				startPos.y += racer.GetComponent<BoxCollider>().size.y / 2;
				GameObject newRacer = Instantiate(racer,
					startPos, Quaternion.AngleAxis(90, Vector3.up));

				racers.Add(newRacer.GetComponent<Horse>());
				newRacer.name = "Horse " + i;
				GameObject newButton = Instantiate(focusButton, focusPanel.transform);
				newButton.GetComponent<Button>().onClick.AddListener(delegate { FocusOnRider(newRacer); });
				newButton.GetComponentInChildren<Text>().text = newRacer.name;
				++i;
			}

			//mainCamera.SetFocus(racers[0].gameObject);
			StartCoroutine(Countdown());
		}


		public void FocusOnRider(GameObject racer)
		{
			Debug.Log(racer.name);
			mainCamera.SetFocusSmooth(racer.transform);
		}

		private IEnumerator Countdown()
		{

			startLight.RedLight();
			while (time < 3)
			{
				yield return null;
				time += Time.deltaTime;
			}

			// start pan camera
			mainCamera.SetFocusSmooth(racers[0].transform);
			startLight.YellowLight();

			while (time < 5)
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