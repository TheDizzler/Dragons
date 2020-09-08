using System.Collections.Generic;
using UnityEngine;

namespace AtomosZ.Gambal.Keiba
{
	public class EchoEffect : MonoBehaviour
	{
		public float startTimeBetweenSpawns;
		public int numEchoSpawns = 3;

		public GameObject echoPrefab;


		private float timeBetweenSpawns;
		private List<SpriteRenderer> echos = new List<SpriteRenderer>();
		private int nextEcho = 0;
		private SpriteRenderer spriteRenderer;



		public void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			for (int i = 0; i < numEchoSpawns; ++i)
				echos.Add(Instantiate(echoPrefab).GetComponent<SpriteRenderer>());
			timeBetweenSpawns = startTimeBetweenSpawns;
		}

		public void Update()
		{
			if (timeBetweenSpawns <= 0)
			{
				if (++nextEcho >= echos.Count)
				{
					nextEcho = 0;
				}

				echos[nextEcho].sprite = spriteRenderer.sprite;
				echos[nextEcho].transform.position = transform.position;
				echos[nextEcho].transform.rotation = transform.rotation;
				timeBetweenSpawns = startTimeBetweenSpawns;
			}
			else
				timeBetweenSpawns -= Time.deltaTime;

		}
	}
}