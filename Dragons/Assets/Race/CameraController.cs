using System.Collections;
using UnityEngine;

namespace AtomosZ.Gambal.Keiba
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform focus;
		//[SerializeField] private float inputSensitivity = 50.0f;
		//[SerializeField] private float clampAngle = 90;
		//[SerializeField]
		//[Range(0.01f, 1.0f)]
		//private float smoothFactor = .5f;
		//public float scrollDamp = 6;
		//public float scrollSpeed = 5;

		private Camera cam;
		private Transform trans;
		private bool follow = false;
		private Coroutine panning;

		private bool rotateAroundFocus = false;

		private Vector3 cameraOffset;
		private float rotY = 0;
		private float rotX = 0;
		private Vector3 prevPos;

		//private float cameraDistance = 10;



		public void Start()
		{
			cam = GetComponent<Camera>();
			trans = transform;
			rotY = trans.localRotation.eulerAngles.y;
			rotX = trans.localRotation.eulerAngles.x;
			
			Cursor.lockState = CursorLockMode.Confined;
			//trans.position = focus.position;
			//trans.localEulerAngles = focus.localEulerAngles;
		}


		public void StartRace()
		{
			StartCoroutine(PanToSmoothly(focus, 10));
		}


		public void Update()
		{
			if (Input.GetMouseButtonDown(1))
				prevPos = cam.ScreenToViewportPoint(Input.mousePosition);
			rotateAroundFocus = Input.GetKey(KeyCode.Mouse1);
		}

		public void LateUpdate()
		{
			if (follow)
			{
				//if (rotateAroundFocus)
				{
					Vector3 dir = prevPos - cam.ScreenToViewportPoint(Input.mousePosition);
					trans.position = focus.localPosition;
					//trans.Rotate(Vector3.right, dir.y * 180);
					//trans.Rotate(Vector3.up, -dir.x * 180, Space.World);
					trans.Translate(new Vector3(0, 0, -10));

					prevPos = cam.ScreenToViewportPoint(Input.mousePosition);
					//Quaternion camTurnAngle = Quaternion.AngleAxis(
					//	Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up)
					//		* Quaternion.AngleAxis(yRot, Vector3.right);
					//cameraOffset = camTurnAngle * cameraOffset;
				}

				//float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
				//if (scrollAmount != 0)
				//{
				//	scrollAmount *= scrollSpeed * cameraDistance * .3f;
				//	cameraDistance += scrollAmount * -1f;
				//	cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 25f);
				//}

				//trans.localPosition = Vector3.Slerp(
				//	transform.position, focus.position + cameraOffset, smoothFactor);
				//trans.LookAt(focus);

			}
		}

		public void SetFocus(Transform newFocus)
		{
			focus = newFocus;
			cameraOffset = transform.position - focus.position;
		}

		public void SetFocusSmooth(Transform newFocus)
		{
			if (this.enabled == false)
			{
				return;
			}

			if (panning != null)
			{
				StopCoroutine(panning);
			}

			follow = false;
			panning = StartCoroutine(PanToSmoothly(newFocus));
		}

		private IEnumerator PanToSmoothly(Transform newFocus, float travelTime = 3)
		{
			focus = newFocus;
			float time = 0;
			float timeToPan = travelTime;

			while (time < timeToPan)
			{
				yield return null;
				time += Time.deltaTime;
				float t = time / timeToPan;

				trans.localPosition = Vector3.Slerp(trans.localPosition,
					newFocus.position + newFocus.forward * -20, t);
				trans.LookAt(
					Vector3.Slerp(trans.localPosition + trans.forward * 10, newFocus.position, t * 2));
			}

			trans.localPosition = newFocus.position + newFocus.transform.right * -20;
			rotY = trans.localRotation.eulerAngles.y;
			rotX = trans.localRotation.eulerAngles.x;
			cam.transform.LookAt(focus);
			follow = true;
			panning = null;
			cameraOffset = trans.position - focus.position;
		}
	}
}