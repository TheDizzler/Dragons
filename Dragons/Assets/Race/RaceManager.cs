using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AtomosZ.Gambal.Keiba
{
	public class RaceManager : MonoBehaviour
	{
		public static Waypoint firstWaypoint = null;
		public bool closedLoopCourse = true;
		public bool useGlobalAngle = true;
		public float globalAngle = 0;

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

			if (firstWaypoint == null)
				firstWaypoint = GameObject.Find("First Waypoint").GetComponent<Waypoint>();
			Waypoint waypoint = firstWaypoint;
			List<Transform> waypoints = new List<Transform>();
			while (waypoint != null)
			{
				waypoints.Add(waypoint.transform);
				waypoint = waypoint.next;

			}

			if (waypoints.Count != 0)
			{
				BezierPath bezierPath = new BezierPath(waypoints, closedLoopCourse, PathSpace.xyz);
				GetComponent<PathCreator>().bezierPath = bezierPath;
			}

			if (useGlobalAngle)
				GetComponent<PathCreator>().bezierPath.GlobalNormalsAngle = globalAngle;
		}


		public List<Horse> GetRacers()
		{

			int i = 0;
			foreach (GameObject racer in racerGOs)
			{
				GameObject newRacer = Instantiate(racer,
					new Vector3(900 * i, 900 * i, 900 * i), Quaternion.AngleAxis(90, Vector3.up));

				Horse newHorse = newRacer.GetComponent<Horse>();
				newHorse.SetNumber(i);
				racers.Add(newHorse);

				GameObject newButton = Instantiate(focusButton, focusPanel.transform);
				newButton.GetComponent<Button>().onClick.AddListener(delegate { FocusOnRider(newRacer); });
				newButton.GetComponentInChildren<Text>().text = newRacer.name;
				++i;
			}

			return racers;
		}

		public void StartRace()
		{
			int i = 0;
			foreach (Horse racer in racers)
			{
				Vector3 startPos = startLine.startPlaceHolders[i].transform.position;
				startPos.y += racer.GetComponent<BoxCollider>().size.y / 2;
				racer.transform.position = startPos;
				racer.gameObject.SetActive(true);
				Destroy(startLine.startPlaceHolders[i]);
				++i;
			}

			mainCamera.StartRace();
			StartCoroutine(Countdown());
			focusPanel.SetActive(true);
		}


		public void LoadNextRace()
		{
			SceneManager.LoadScene("RaceScene");
		}


		public void FocusOnRider(GameObject racer)
		{
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
			{
				racer.StartRace();
			}

			while (time < 6)
			{ // just a little delay so racers aren't considered finished before they start
				yield return null;
				time += Time.deltaTime;
			}

			startLine.started = true;
		}

		public void ManualUpdateRoadMesh()
		{
			if (firstWaypoint == null)
				firstWaypoint = GameObject.Find("First Waypoint").GetComponent<Waypoint>();
			Waypoint waypoint = firstWaypoint;
			List<Transform> waypoints = new List<Transform>();
			while (waypoint != null)
			{
				waypoints.Add(waypoint.transform);
				waypoint = waypoint.next;

			}

			if (waypoints.Count != 0)
			{
				BezierPath bezierPath = new BezierPath(waypoints, closedLoopCourse, PathSpace.xyz);
				GetComponent<PathCreator>().bezierPath = bezierPath;
			}

			if (useGlobalAngle)
				GetComponent<PathCreator>().bezierPath.GlobalNormalsAngle = globalAngle;
		}


	}
}