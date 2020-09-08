using UnityEngine;

namespace AtomosZ.Gambal.Keiba
{
	public class Waypoint : MonoBehaviour
	{
		public Waypoint next;
		public Waypoint prev;


		public void OnDrawGizmos()
		{
			if (next != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, next.transform.position);
			}
		}
	}
}