using UnityEngine;

public class Planet : MonoBehaviour
{
	public Texture2D planetTex;
	public float rotationSpeed = 1;
	private Transform trans;

	private Vector3 currentRot;


	void Start()
	{
		trans = transform;
		GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", planetTex);
	}


	public void Update()
	{
		trans.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
