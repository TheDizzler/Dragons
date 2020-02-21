using System.Collections;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform focus;
		[SerializeField]
		[Range(0.01f, 1.0f)]
		private float smoothFactor = .5f;
		public float scrollDamp = 6;
		public float scrollSpeed = 5;

		private Camera cam;
		private Transform trans;
		private bool follow = false;
		private Coroutine panning;

		private bool rotateAroundFocus = false;
		private float rotationSpeed = 5.0f;
		private Vector3 cameraOffset;
		private float yRot = 0;
		private float xRot = 0;
		private float cameraDistance = 10;



		public void Start()
		{
			cam = GetComponent<Camera>();
			trans = transform;
			//StartCoroutine(PanToSmoothly(focus));
			trans.position = focus.position;
			trans.localEulerAngles = focus.localEulerAngles;
		}

		public void Update()
		{
			rotateAroundFocus = Input.GetKey(KeyCode.Mouse1);
		}

		public void LateUpdate()
		{
			if (follow)
			{// was trying: https://www.youtube.com/watch?v=bVo0YLLO43s
				if (rotateAroundFocus)
				{
					xRot += Input.GetAxis("Mouse X") * rotationSpeed;
					yRot += Input.GetAxis("Mouse Y") * rotationSpeed;
					yRot = Mathf.Clamp(yRot, 0, 90);

					//Quaternion camTurnAngle = Quaternion.AngleAxis(
					//	Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up)
					//		* Quaternion.AngleAxis(yRot, Vector3.right);
					//cameraOffset = camTurnAngle * cameraOffset;
				}

				float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
				if (scrollAmount != 0)
				{
					scrollAmount *= scrollSpeed * cameraDistance * .3f;
					cameraDistance += scrollAmount * -1f;
					cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 25f);
				}

				//trans.localPosition = Vector3.Slerp(
				//	transform.position, focus.position + cameraOffset, smoothFactor);
				//trans.LookAt(focus);

				Quaternion qt = Quaternion.Euler(yRot, xRot, 0);
				trans.rotation = Quaternion.Lerp(trans.rotation, qt, Time.deltaTime * smoothFactor);

				trans.position = new Vector3(0, 0, Mathf.Lerp(trans.position.z, cameraDistance * -1, Time.deltaTime * scrollDamp));


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
				return;
			if (panning != null)
				StopCoroutine(panning);
			follow = false;
			panning = StartCoroutine(PanToSmoothly(newFocus));
		}

		private IEnumerator PanToSmoothly(Transform newFocus)
		{
			focus = newFocus;
			float time = 0;
			float timeToPan = 3;

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
			cam.transform.LookAt(focus);
			follow = true;
			panning = null;
			cameraOffset = transform.position - focus.position;
		}
	}
}