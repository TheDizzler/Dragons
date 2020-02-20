using UnityEngine;
using Random = UnityEngine.Random;

namespace AtomosZ.Gambale.Keiba
{
	public class Horse : MonoBehaviour
	{
		private const float timeToRandomize = 2.5f;

		//public float maxAcceleration = 1f;
		//public float maxSpeed = 25f;
		public float baseSpeed = 1f;
		//public float startAcceleration = .025f;
		//public float currentAcceleration = 0;
		public float speedBoost = 1;
		//public float currentSpeed = 0;
		public int luck = 0;

		[SerializeField] private GameObject speedTrailsSmall = null;
		[SerializeField] private GameObject speedTrailsBig = null;
		[SerializeField] private GameObject particles = null;
		private EchoEffect echo = null;
		private Material horseMat = null;
		private Animator anim;
		private Transform planet;
		private float timeInRainbow = 0;
		private float randomizeTime = timeToRandomize;


		public void Start()
		{
			horseMat = GetComponent<SpriteRenderer>().sharedMaterial;
			anim = GetComponent<Animator>();
			planet = GameObject.FindGameObjectWithTag("Planet").transform;
			echo = GetComponent<EchoEffect>();
			//currentSpeed = 0;
			//currentAcceleration = 0;
			this.enabled = false;
			anim.enabled = false;
		}

		public void StartRace()
		{
			//currentAcceleration = startAcceleration;
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

			transform.localPosition += transform.right * currentSpeed * 20 * Time.deltaTime;
			Vector3 dirToPlanet = planet.position - transform.position;
			transform.LookAt(planet.position);
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