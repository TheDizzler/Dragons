using UnityEngine;

public class Horse : MonoBehaviour
{
	public float currentSpeed = 0;

	private Material horseMat = null;
	private Animator anim;
	private Transform planet;


	public void Start()
	{
		horseMat = GetComponent<SpriteRenderer>().sharedMaterial;
		anim = GetComponent<Animator>();
		planet = GameObject.FindGameObjectWithTag("Planet").transform;
	}

	public void Update()
	{
		anim.speed = currentSpeed + .25f;
		horseMat.SetFloat("_Speed", currentSpeed);

		transform.localPosition += transform.right * currentSpeed * 20 * Time.deltaTime;
		Vector3 dirToPlanet = planet.localPosition - transform.localPosition;
		//transform.LookAt(Vector3.Cross(dirToPlanet, Vector3.up).normalized);
		transform.LookAt(dirToPlanet);
	}
}
