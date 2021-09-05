using UnityEditor;
using UnityEngine;

namespace West.Tool.World
{
	[CustomEditor(typeof(Asset.World.Generator))]
	public class GeneratorEditor : Editor
	{
		Asset.World.Generator activeGameObject;

		void OnEnable()
		{
			if (PrefabUtility.GetCorrespondingObjectFromSource(target) == null && PrefabUtility.GetPrefabInstanceHandle(target) != null)
				return;

			activeGameObject = (Asset.World.Generator)target;
		}

		void OnDisable()
		{
			activeGameObject = null;
		}

		void OnSceneGUI()
		{
			DrawControl();
			DrawVisual();
		}

		void DrawVisual()
		{
			activeGameObject.DrawEnvironments();
		}

		void DrawControl()
        {

        }

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (activeGameObject == null)
            {
				return;
            }

			if (GUILayout.Button("Add Child Environment"))
			{
				var go = new GameObject("environment");
				go.transform.parent = activeGameObject.transform;
				go.transform.localScale = activeGameObject.transform.localScale;
				go.transform.localPosition = new Vector3();
				go.AddComponent<Asset.World.Environment>().Init();
			}

			if (GUILayout.Button("Validate"))
			{
				activeGameObject.Validate();
			}
		}
	}
}