using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AtomosZ.Gambale.Keiba
{
	public class Horse : MonoBehaviour
	{
		private const float timeToRandomize = 2.5f;

		
		public float baseSpeed = 1f;
		public float speedBoost = 1;
		public int luck = 0;
		public Image portrait;

		[SerializeField] private GameObject speedTrailsSmall = null;
		[SerializeField] private GameObject speedTrailsBig = null;
		[SerializeField] private GameObject particles = null;
		[SerializeField] private TextMeshPro numberRightSide = null;
		[SerializeField] private TextMeshPro numberLeftSide = null;


		private EchoEffect echo = null;
		private Material horseMat = null;
		private Animator anim;
		private float timeInRainbow = 0;
		private float randomizeTime = timeToRandomize;
		private int number;

		public Waypoint nextWaypoint;


		public void Start()
		{
			anim = GetComponentInChildren<Animator>();
			echo = GetComponentInChildren<EchoEffect>();
			horseMat = GetComponentInChildren<SpriteRenderer>().sharedMaterial;

			gameObject.SetActive(false);
			this.enabled = false;
			anim.enabled = false;
			nextWaypoint = RaceManager.FirstWaypoint;
		}

		public void SetNumber(int i)
		{
			name = "Horse " + i;
			number = i;
			numberRightSide.text = numberLeftSide.text = i +"";
		}

		public void StartRace()
		{
			this.enabled = true;
			anim.enabled = true;
		}

		public void Update()
		{
			randomizeTime -= Time.deltaTime;
			if (randomizeTime <= 0)
			{
				randomizeTime = timeToRandomize;
				RandomizeMovement();
			}

			float currentSpeed = baseSpeed * speedBoost;

			anim.speed = currentSpeed > 0 ? currentSpeed : 0;
			horseMat.SetFloat("_Speed", currentSpeed);

			CheckForSpeedEffects(currentSpeed);

			transform.localPosition += transform.forward * currentSpeed * 20 * Time.deltaTime;
			if (Vector3.Distance(nextWaypoint.transform.position, transform.position) < 20)
			{
				nextWaypoint = nextWaypoint.next;
				if (nextWaypoint == null)
				{
					// finished!
					enabled = false;
					return;
				}
			}

			Vector3 dirToWaypoint = nextWaypoint.transform.position - transform.position;
			Vector3 moveTowards = nextWaypoint.transform.position;
			moveTowards.y = transform.position.y;
			transform.LookAt(moveTowards);
			Debug.DrawRay(transform.position, dirToWaypoint);
		}

		private void CheckForSpeedEffects(float currentSpeed)
		{
			if (currentSpeed >= 3 && !echo.enabled)
			{
				echo.enabled = true;
			}
			else if (currentSpeed < 3 && echo.enabled)
			{
				echo.enabled = false;
			}

			if (currentSpeed >= 7 && !particles.activeSelf)
			{
				particles.SetActive(true);
			}
			else if (currentSpeed < 7 && particles.activeSelf)
			{
				particles.SetActive(false);
			}

			if (currentSpeed >= 10)
			{
				timeInRainbow += Time.deltaTime;
				horseMat.SetFloat("_TimeInRainbow", timeInRainbow);
			}
			else
			{
				timeInRainbow = 0;
			}
		}


		private void RandomizeMovement()
		{
			if (speedBoost != 1)
			{
				DisengageSpeedBoost();
			}

			switch (Random.Range(0, 20) + luck)
			{
				case -2:
					baseSpeed -= -1f;
					break;
				case -1:
					baseSpeed -= .75f;
					break;
				case 0:
					baseSpeed -= .5f;
					if (baseSpeed <= 0)
						baseSpeed = .1f;
					break;
				case 1:
					baseSpeed -= .4f;
					break;
				case 2:
					baseSpeed -= .3f;
					break;
				case 3:
					baseSpeed -= .25f;
					break;
				case 4:
					baseSpeed -= .2f;
					break;
				case 5:
					baseSpeed -= .15f;
					break;
				case 6:
					baseSpeed -= .1f;
					break;
				case 7:
					baseSpeed -= .05f;
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
					baseSpeed += .05f;
					break;
				case 13:
					baseSpeed += .1f;
					break;
				case 14:
					baseSpeed += .15f;
					break;
				case 15:
					baseSpeed += .2f;
					break;
				case 16:
					baseSpeed += .25f;
					break;
				case 17:
					baseSpeed += .3f;
					break;
				case 18:
					baseSpeed += .4f;
					break;
				case 19:
					baseSpeed += .5f;
					break;
				case 20:
					EngageSpeedBoost(2);
					baseSpeed += .25f;
					break;
				case 21:
					EngageSpeedBoost(3);
					baseSpeed += .5f;
					break;
				case 22:
					EngageSpeedBoost(4);
					baseSpeed += .5f;
					break;
			}
		}

		private void EngageSpeedBoost(int boostMultiplier)
		{
			speedBoost = boostMultiplier;
			speedTrailsSmall.SetActive(true);
			speedTrailsBig.SetActive(true);
		}

		private void DisengageSpeedBoost()
		{
			speedBoost = 1;
			speedTrailsSmall.SetActive(false);
			speedTrailsBig.SetActive(false);
		}
	}
}