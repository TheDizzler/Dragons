using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba.WagerUI
{
	public class Ranking : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI[] rankingLabels = new TextMeshProUGUI[3];
		[SerializeField] private GameObject rankingPanel = null;
		private WagerManager wagerManager = null;

		private List<Horse> ranking = new List<Horse>();
		public List<Color> pizzazz = new List<Color>()
		{
			Color.red,
			new Color(.5f, 0, 1, 1),
			Color.cyan,
			new Color(.5f, 1, 0, 1),
		};


		void Start()
		{
			wagerManager = GetComponent<WagerManager>();
		}


		public void RacerFinished(Horse horse)
		{
			ranking.Add(horse);
			switch (ranking.Count)
			{
				case 1:
					rankingPanel.SetActive(true);
					rankingLabels[0].text = horse.name;
					break;
				case 2:
					rankingLabels[1].text = horse.name;
					break;
				case 3:
					rankingLabels[2].text = horse.name;
					foreach (int winner in wagerManager.Payout(ranking))
					{
						StartCoroutine(ColorPizzazz(rankingLabels[winner]));
					}
					break;
			}
		}

		public void TestPizzazz()
		{
			for (int i = 0; i < rankingLabels.Length; ++i)
			{
				StartCoroutine(ColorPizzazz(rankingLabels[i]));
			}
		}

		
		private IEnumerator ColorPizzazz(TextMeshProUGUI placingText)
		{
			VertexGradient magicalColors = new VertexGradient(pizzazz[0], pizzazz[1], pizzazz[2], pizzazz[3]);
			placingText.enableVertexGradient = true;
			placingText.colorGradient = magicalColors;
			Color color1 = pizzazz[0];
			Color color2 = pizzazz[1];
			Color color3 = pizzazz[2];
			Color color4 = pizzazz[3];
			int cycleStartIndex = 1;

			float t = 0;
			while (true)
			{
				yield return null;
				t += Time.deltaTime;
				if (t > 1)
				{
					t = 0;
					int nextColorIndex = cycleStartIndex;
					SetNextColor(ref color1, ref nextColorIndex);
					SetNextColor(ref color2, ref nextColorIndex);
					SetNextColor(ref color3, ref nextColorIndex);
					SetNextColor(ref color4, ref nextColorIndex);
					if (++cycleStartIndex >= pizzazz.Count)
						cycleStartIndex = 0;
				}

				Color c1 = Color.Lerp(color1, color2, t);
				Color c2 = Color.Lerp(color2, color3, t);
				Color c3 = Color.Lerp(color3, color4, t);
				Color c4 = Color.Lerp(color4, color1, t);
				magicalColors.topLeft = c1;
				magicalColors.topRight = c2;
				magicalColors.bottomRight = c3;
				magicalColors.bottomLeft = c4;
				placingText.colorGradient = magicalColors;
			}
		}

		private void SetNextColor(ref Color color, ref int nextColorIndex)
		{
			if (nextColorIndex >= pizzazz.Count)
				nextColorIndex = 0;
			color = pizzazz[nextColorIndex++];
		}
	}
}