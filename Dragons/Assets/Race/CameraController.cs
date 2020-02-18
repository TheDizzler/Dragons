using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject focus;
	private Camera cam;
	private Transform trans;


	public void Start()
	{
		cam = GetComponent<Camera>();
		trans = transform;
	}

	public void LateUpdate()
	{
		trans.localPosition = focus.transform.localPosition + focus.transform.forward * -10;
		cam.transform.LookAt(focus.transform);
	}
}
