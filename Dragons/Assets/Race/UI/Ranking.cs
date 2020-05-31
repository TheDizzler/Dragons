using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba.WagerUI
{
	public class Ranking : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI[] rankingLabels = new TextMeshProUGUI[3];
		[SerializeField] private GameObject wagerPanel = null;
		private WagerManager wagerManager = null;

		private List<Horse> ranking = new List<Horse>();
		private Color color1 = Color.red;
		private Color color2 = new Color(.5f, 0, 1, 1);
		private Color color3 = Color.cyan;
		private Color color4 = new Color(.5f, 1, 0, 1);


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
					wagerPanel.SetActive(true);
					rankingLabels[0].text = horse.name;
					break;
				case 2:
					rankingLabels[1].text = horse.name;
					break;
				case 3:
					rankingLabels[2].text = horse.name;
					foreach (int winner in wagerManager.Payout(ranking))
					{
						StartCoroutine(ColorPizzazz(rankingLabels[winner - 1]));
					}
					break;
			}
		}


		private IEnumerator ColorPizzazz(TextMeshProUGUI placingText)
		{
			VertexGradient magicalColors = new VertexGradient(color1, color2, color3, color4);
			placingText.enableVertexGradient = true;
			placingText.colorGradient = magicalColors;

			while (true)
			{
				yield return null;
				float t = Time.deltaTime;
				Color c1 = Color.Lerp(color1, color2, t);
				Color c2 = Color.Lerp(color2, color3, t);
				Color c3 = Color.Lerp(color3, color4, t);
				Color c4 = Color.Lerp(color4, color1, t);
				magicalColors.topLeft = c1;
				magicalColors.topRight = c2;
				magicalColors.bottomRight = c3;
				magicalColors.bottomLeft = c4;
				color1 = c1;
				color2 = c2;
				color3 = c3;
				color4 = c4;
				placingText.colorGradient = magicalColors;

			}
		}
	}
}