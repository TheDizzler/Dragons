using UnityEditor;
using UnityEngine;

namespace AtomosZ.Gambale.Keiba.CustomEditors
{
	[CustomEditor(typeof(Waypoint))]
	public class WaypointCreator : Editor
	{
		private static bool addingNode = false;
		private bool newIsNext;
		private bool addingBetweenNode;

		void OnSceneGUI()
		{
			if (addingNode)
			{
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
				if (Event.current.type == EventType.MouseDown)
				{
					Transform parent = ((Waypoint)target).transform.parent;
					GameObject waypointPrefab =
						AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Race/Objects/Waypoint/Waypoint.prefab");

					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

					if (Physics.Raycast(worldRay, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Track")))
					{
						Vector3 pos = hitInfo.point;
						GameObject clone =(GameObject)PrefabUtility.InstantiatePrefab(waypointPrefab, parent);
						clone.transform.position = pos;
						clone.name = "waypoint";
						clone.transform.parent = parent;
						if (newIsNext)
						{
							Waypoint current = ((Waypoint)target);
							Waypoint cloneNav = clone.GetComponent<Waypoint>();
							cloneNav.prev = current;
							
							if (addingBetweenNode)
							{
								Waypoint next = current.next;
								cloneNav.next = next;
								next.prev = cloneNav;
								addingBetweenNode = false;
							}

							current.next = cloneNav;
						}
						else
						{
							Waypoint current = ((Waypoint)target);
							Waypoint cloneNav = clone.GetComponent<Waypoint>();
							cloneNav.prev = current;
							current.next = cloneNav;
						}

						Selection.activeGameObject = clone;
						EditorUtility.SetDirty(clone);
						EditorUtility.SetDirty(target);
						addingNode = false;
						HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script:",
				MonoScript.FromMonoBehaviour((Waypoint)target),
				typeof(Waypoint), false);
			GUI.enabled = true;

			SerializedProperty prop = serializedObject.FindProperty("prev");
			if (prop.objectReferenceValue == null)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(prop);
				if (addingNode)
				{
					if (GUILayout.Button("Click in Scene"))
					{
						addingNode = false;
					}
				}
				else if (GUILayout.Button("New NavPoint"))
				{
					addingNode = true;
					newIsNext = false;
				}

				EditorGUILayout.EndHorizontal();
			}
			else
			{
				EditorGUILayout.PropertyField(prop);
			}

			prop = serializedObject.FindProperty("next");
			if (prop.objectReferenceValue == null)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(prop);
				if (addingNode)
				{
					if (GUILayout.Button("Click in Scene"))
					{
						addingNode = false;
						addingBetweenNode = false;
					}
				}
				else if (GUILayout.Button("New Waypoint"))
				{
					addingNode = true;
					newIsNext = true;
				}

				EditorGUILayout.EndHorizontal();
			}
			else
			{
				EditorGUILayout.PropertyField(prop);
				if (addingNode)
				{
					if (GUILayout.Button("Click in Scene"))
					{
						addingNode = false;
						addingBetweenNode = false;
					}
				}
				else if (GUILayout.Button("Add Between Waypoint"))
				{
					addingNode = true;
					newIsNext = true;
					addingBetweenNode = true;
				}

			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}