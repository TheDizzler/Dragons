using UnityEngine;

namespace AtomosZ.Gambale.Keiba
{
	public class TrafficLights : MonoBehaviour
	{
		[SerializeField] private Light[] redLights = new Light[2];
		[SerializeField] private Light[] yellowLights = new Light[2];
		[SerializeField] private Light[] greenLights = new Light[2];

		[SerializeField] private Material redLightMat = null;
		[SerializeField] private Material yellowLightMat = null;
		[SerializeField] private Material greenLightMat = null;

		public void RedLight()
		{
			foreach (Light light in redLights)
				light.enabled = true;
			foreach (Light light in yellowLights)
				light.enabled = false;
			foreach (Light light in greenLights)
				light.enabled = false;
			redLightMat.EnableKeyword("_EMISSION");
			yellowLightMat.DisableKeyword("_EMISSION");
			greenLightMat.DisableKeyword("_EMISSION");
		}

		public void YellowLight()
		{
			foreach (Light light in redLights)
				light.enabled = false;
			foreach (Light light in yellowLights)
				light.enabled = true;
			foreach (Light light in greenLights)
				light.enabled = false;
			redLightMat.DisableKeyword("_EMISSION");
			yellowLightMat.EnableKeyword("_EMISSION");
			greenLightMat.DisableKeyword("_EMISSION");
		}

		public void GreenLight()
		{
			foreach (Light light in redLights)
				light.enabled = false;
			foreach (Light light in yellowLights)
				light.enabled = false;
			foreach (Light light in greenLights)
				light.enabled = true;
			redLightMat.DisableKeyword("_EMISSION");
			yellowLightMat.DisableKeyword("_EMISSION");
			greenLightMat.EnableKeyword("_EMISSION");
		}


		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
				RedLight();
			if (Input.GetKeyDown(KeyCode.X))
				YellowLight();
			if (Input.GetKeyDown(KeyCode.C))
				GreenLight();
		}
	}
}