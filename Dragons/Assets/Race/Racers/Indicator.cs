using UnityEngine;

public class Indicator : MonoBehaviour
{
	[SerializeField] private bool up = true;
	[SerializeField] private float cycleDistance = 0;
	[SerializeField] private float cycleTime = 0;
	private Vector3 centerPos;
	private Vector3 topPos;
	private Vector3 bottomPos;
	private float totalTime = 0;


	public void Start()
	{
		centerPos = transform.localPosition;
		topPos = centerPos;
		topPos.y += cycleDistance;
		bottomPos = centerPos;
		bottomPos.y -= cycleDistance;
	}

	public void Update()
	{
		if (up)
			totalTime += Time.deltaTime;
		else
			totalTime -= Time.deltaTime;

		float t = Mathf.Sign(totalTime) * totalTime * totalTime / cycleTime;
		if (t > 1)
			up = false;
		if (t < 0)
			up = true;
		transform.localPosition = Vector3.Lerp(topPos, bottomPos,  t);
	}
}
