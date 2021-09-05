using UnityEditor;
using UnityEngine;

namespace West.Tool.World
{
	[CustomEditor(typeof(Asset.World.Environment))]
	public class EnvironmentEditor : Editor
	{
		Asset.World.Environment path;
		Plane xz = new Plane(new Vector3(0f, 1f, 0f), 0f);
		bool passedOnce = false;
		Vector2 lastCenter;

		void OnScene(SceneView sceneview)
		{
			if (path != null)
			{
				DrawVisual();
			}
			else
			{
				SceneView.duringSceneGui -= OnScene;
			}
		}

		void OnSceneGUI()
		{
			DrawControl();
			DrawVisual();
		}

		void OnEnable()
		{
			if (PrefabUtility.GetCorrespondingObjectFromSource(target) == null && PrefabUtility.GetPrefabInstanceHandle(target) != null)
				return;

			path = (Asset.World.Environment)target;
			lastCenter = ToV2(path.transform.localPosition);

			SceneView.duringSceneGui += OnScene;
		}

        private void OnDisable()
        {
			path = null;
		}

        void DrawVisual()
		{
			Vector2 parentPosition = (path.transform.parent == null) 
				? new Vector2() 
				: new Vector2(path.transform.parent.transform.position.x, path.transform.parent.transform.position.z);
			//draw edges
			var headEnvironment = path;
			while (headEnvironment.transform.parent != null)
			{
				var component = headEnvironment.transform.parent.GetComponent<Asset.World.Environment>();
				if (component != null)
				{
					headEnvironment = component;
				}
				else
				{
					var generator = headEnvironment.transform.parent.GetComponent<Asset.World.Generator>();
					if (generator != null)
					{
						generator.DrawEnvironments();
					}
                    else
					{
						headEnvironment.DrawEnvironmentHierarchy();
					}
					break;
                }
			}
		}

		void DrawControl()
		{
			Event guiEvent = Event.current;
			bool guiEventHandled = false;

			if (path == null)
            {
				return;
            }

			//move all points if moved from main anchor
			Vector2 currentCenter = ToV2(path.transform.localPosition);
			if ((passedOnce || path.justDropped) && (lastCenter != currentCenter))
			{
				Undo.RecordObject(path, "path move");
				for (int i = 0; i < path.NumEdges; ++i)
				{
					path[i].Position += currentCenter - lastCenter;
				}
			}
			if ((passedOnce || path.justDropped) && path.height != path.transform.position.y)
			{
				path.height = path.transform.position.y;
			}
			passedOnce = true;
			path.justDropped = false;

			//plane
			xz.distance = -path.height;

			//define pivot
			Vector2 center = Vector2.zero;
			int e1Candidate = 0;
			int e2Candidate = 0;
			int e3Candidate = 0;
			float candidatesSqrDistanceObserved = 0.0f;
			for (int i = 0; i < path.NumEdges; ++i)
			{
				for (int j = i + 1; j < path.NumEdges; ++j)
				{
					float observedSqrDistance = (path[i].Position - path[j].Position).sqrMagnitude;
					if (observedSqrDistance > candidatesSqrDistanceObserved)
					{
						e1Candidate = i;
						e2Candidate = j;
						candidatesSqrDistanceObserved = observedSqrDistance;
					}
				}
			}
			center = (path[e1Candidate].Position + path[e2Candidate].Position) / 2.0f;
			if (path.NumEdges > 2)
			{
				candidatesSqrDistanceObserved = 0.0f;
				for (int i = 0; i < path.NumEdges; ++i)
				{
					float observedSqrDistance = (path[i].Position - center).sqrMagnitude;
					if (observedSqrDistance > candidatesSqrDistanceObserved && i != e1Candidate && i != e2Candidate)
					{
						e3Candidate = i;
						candidatesSqrDistanceObserved = observedSqrDistance;
					}
				}
				float angle = Vector2.Angle(path[e1Candidate].Position - path[e3Candidate].Position, path[e2Candidate].Position - path[e3Candidate].Position);
				if (angle <= 90.0f)
				{
					Asset.World.EnvironmentEdge e1 = new Asset.World.EnvironmentEdge(path[e1Candidate].Position, path[e2Candidate]);
					Asset.World.EnvironmentEdge e2 = new Asset.World.EnvironmentEdge(path[e2Candidate].Position, path[e3Candidate]);
					center = Path.IntersectionPoint(e1.Center, e1.Center + e1.Normal, e2.Center, e2.Center + e2.Normal);
				}
			}
			//center /= path.NumPoints();
			lastCenter = center;
			path.center = center;
			path.transform.localPosition = new Vector3(center.x, path.transform.localPosition.y, center.y);

			//draw edge type handles
			Handles.color = Color.white;
			Vector2 parentPosition = (path.transform.parent == null)
				? new Vector2()
				: new Vector2(path.transform.parent.transform.position.x, path.transform.parent.transform.position.z);
			for (int i = 0; i < path.NumEdges; ++i)
			{
				Vector2 p1 = parentPosition + path[i].Position;
				Vector2 p2 = parentPosition + path[i + 1].Position;
				float scale = (Camera.current.transform.position - path.ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Handles.color = Color.black;
				bool clicked = Handles.Button(
					path.ToV3((p1 + p2) / 2.0f),
					Quaternion.LookRotation(Vector3.up),
					scale,
					scale,
					Handles.CubeHandleCap);
				if (clicked)
				{
					if (guiEvent.shift)
					{
						Undo.RecordObject(path, "Insert path anchor");
						path.Split(i);
					}
					else if (guiEvent.control)
					{
						Undo.RecordObject(path, "Merge path edge");
						path.Merge(i);
					}
					guiEventHandled = true;
				}
			}

			//draw anchor points
			Handles.color = Color.grey;
			for (int i = 0; i < path.NumEdges; ++i)
			{
				Vector2 p1 = parentPosition + path[i].Position;
				Vector2 p2 = parentPosition + path[i + 1].Position;
				float scale = (Camera.current.transform.position - path.ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Vector3 newPosition = Handles.FreeMoveHandle(path.ToV3(p1), Quaternion.identity, scale, Vector3.zero, Handles.SphereHandleCap);

				float enter;
				Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
				xz.Raycast(worldRay, out enter);
				Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
				Vector2 newPosition2D = ToV2(newPositionOnPlane);
				if (path[i].Position != newPosition2D)
				{
					Undo.RecordObject(path, "Move path anchor");
					path[i].Position = newPosition2D - parentPosition;
					guiEventHandled = true;
				}
			}

			//Input
			if (!guiEventHandled && guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
			{
				float enter;
				Ray worldRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
				xz.Raycast(worldRay, out enter);
				Vector3 mousePos = worldRay.GetPoint(enter);
				Undo.RecordObject(path, "Add path point");
				path.AddSegment(ToV2(mousePos));
			}

			EditorUtility.SetDirty(path);
		}

		Vector2 ToV2(Vector3 v3)
		{
			return new Vector2(v3.x, v3.z);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (path != null && GUILayout.Button("Add Child Environment"))
			{
				var go = new GameObject("environment");
				go.transform.parent = path.transform;
				go.transform.localScale = path.transform.localScale;
				go.transform.localPosition = new Vector3();
				go.AddComponent<Asset.World.Environment>().Init();
			}
		}
	}
}