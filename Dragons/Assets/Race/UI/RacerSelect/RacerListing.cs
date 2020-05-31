using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.Gambale.Keiba.WagerUI
{
	public class RacerListing : MonoBehaviour
	{
		public string racerName;
		public Text nameText;
		public Image racerImage;
		public Horse horse;


		public void ToggleChanged(bool toggled)
		{
			GetComponentInParent<RacerSelectDisplay>().SetSelected(horse, toggled);
		}
	}
}