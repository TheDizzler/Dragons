using UnityEngine;
using UnityEngine.UI;

namespace AtomosZ.UI
{
	public class Spinner : MonoBehaviour
	{
		public int currentValue = 0;
		public int valueChange = 1;

		private InputField inputField;
		private Button upButton;
		private Button downButton;

		public void Start()
		{
			inputField = GetComponentInChildren<InputField>();
			foreach (Button but in GetComponentsInChildren<Button>())
				if (but.name == "UpButton")
					upButton = but;
				else
					downButton = but;
		}

		public void UpButtonPushed()
		{
			currentValue += valueChange;
			inputField.text = "$" + currentValue;
		}
	}
}