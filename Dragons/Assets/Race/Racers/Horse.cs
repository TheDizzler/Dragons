using UnityEngine;

public class Horse : MonoBehaviour
{
	public float acceleration = .025f;
	public float currentSpeed = 0;

	[SerializeField] private GameObject speedTrailsSmall = null;
	[SerializeField] private GameObject speedTrailsBig = null;
	[SerializeField] private GameObject particles = null;
	private EchoEffect echo = null;
	private Material horseMat = null;
	private Animator anim;
	private Transform planet;
	private float timeInRainbow = 0;


	public void Start()
	{
		horseMat = GetComponent<SpriteRenderer>().sharedMaterial;
		anim = GetComponent<Animator>();
		planet = GameObject.FindGameObjectWithTag("Planet").transform;
		echo = GetComponent<EchoEffect>();
		currentSpeed = 0;
	}

	public void Update()
	{
		currentSpeed += acceleration * Time.deltaTime;

		anim.speed = currentSpeed + .25f;
		horseMat.SetFloat("_Speed", currentSpeed);
		if (currentSpeed > .5)
			echo.enabled = true;
		else
			echo.enabled = false;
		if (currentSpeed > .75)
			particles.SetActive(true);
		else
			particles.SetActive(false);
		if (currentSpeed > 1)
		{
			speedTrailsSmall.SetActive(true);
			speedTrailsBig.SetActive(true);
			timeInRainbow += Time.deltaTime;
			horseMat.SetFloat("_TimeInRainbow", timeInRainbow);
		}
		else
		{
			timeInRainbow = 0;
			speedTrailsSmall.SetActive(false);
			speedTrailsBig.SetActive(false);
		}
		transform.localPosition += transform.right * currentSpeed * 20 * Time.deltaTime;
		Vector3 dirToPlanet = planet.position - transform.position;
		//transform.LookAt(Vector3.Cross(dirToPlanet, Vector3.up).normalized);
		transform.LookAt(dirToPlanet);
	}
}
