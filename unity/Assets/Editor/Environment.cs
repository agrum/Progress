using UnityEditor;
using UnityEngine;

namespace West.Tool
{
	[CustomEditor(typeof(Asset.Environment))]
	public class EnvironmentEditor : Editor
	{
		Asset.Environment path;
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

			path = (Asset.Environment)target;
			if (path.edgeList == null)
				path.Init();
			lastCenter = ToV2(path.transform.localPosition);

			SceneView.duringSceneGui += OnScene;
		}

		void DrawVisual()
		{
			Vector2 parentPosition = (path.transform.parent == null) 
				? new Vector2() 
				: new Vector2(path.transform.parent.transform.position.x, path.transform.parent.transform.position.z);
			//draw edges
			var headEnvironment = path;
			while (headEnvironment.transform.parent != null && headEnvironment.transform.parent.TryGetComponent<Asset.Environment>(out headEnvironment)) ;
			DrawEnvironmentHierarchy(headEnvironment);
			foreach (var environment in headEnvironment.GetComponentsInChildren< Asset.Environment>())
			{
				for (int i = 0; i < path.NumEdges; ++i)
				{
					Vector2 p1 = parentPosition + path[i].Position;
					Vector2 p2 = parentPosition + path[i + 1].Position;
					float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
					Handles.color = Color.black;
					Handles.ArrowHandleCap(
						0,
						ToV3((p1 + p2) / 2.0f),
						Quaternion.LookRotation(ToV3(new Vector2((p2 - p1).y, -(p2 - p1).x), false)),
						scale,
						EventType.Repaint);
				}
			}

			for (int i = 0; i < path.NumEdges; ++i)
			{
				Vector2 p1 = parentPosition + path[i].Position;
				Vector2 p2 = parentPosition + path[i + 1].Position;
				float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Handles.color = Color.black;
				Handles.DrawLine(ToV3(p1), ToV3(p2));
				Handles.ArrowHandleCap(
					0,
					ToV3((p1 + p2) / 2.0f),
					Quaternion.LookRotation(ToV3(new Vector2((p2 - p1).y, -(p2 - p1).x), false)),
					scale,
					EventType.Repaint);
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
					Asset.EnvironmentEdge e1 = new Asset.EnvironmentEdge(path[e1Candidate].Position, path[e2Candidate]);
					Asset.EnvironmentEdge e2 = new Asset.EnvironmentEdge(path[e2Candidate].Position, path[e3Candidate]);
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
				float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Handles.color = Color.black;
				bool clicked = Handles.Button(
					ToV3((p1 + p2) / 2.0f),
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
				float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Vector3 newPosition = Handles.FreeMoveHandle(ToV3(p1), Quaternion.identity, scale, Vector3.zero, Handles.SphereHandleCap);

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

		Vector3 ToV3(Vector2 v2, bool applyCreatorHeight = true)
		{
			return new Vector3(v2.x, applyCreatorHeight ? path.height : 0.0f, v2.y);
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
				go.AddComponent<Asset.Environment>().Init();
			}
		}

		public bool CollidesWithOtherEnvironments()
        {
			var parentEnvironment = path.transform.parent.GetComponentInParent<Asset.Environment>();
			if (parentEnvironment != null)
			{
				//check it's contain within its parent's boundaries
				if (path.CollidesWith(parentEnvironment))
				{
					return true;
				}

				//check it doesn't collide with its peers
				bool collided = false;
				foreach (var peerEnvironment in parentEnvironment.gameObject.GetComponentsInChildren<Asset.Environment>())
				{
					if (peerEnvironment != null && peerEnvironment != path && path.CollidesWith(peerEnvironment))
					{
						return true;
					}
				}
			}

			//check it doesn't collide with its children
			foreach (var childEnvironment in path.gameObject.GetComponentsInChildren<Asset.Environment>())
			{
				if (childEnvironment != null && childEnvironment != path && path.CollidesWith(childEnvironment))
				{
					return true;
				}
			}

			return false;
		}

		private void DrawEnvironmentHierarchy(Asset.Environment parent)
		{
			foreach (var environment in parent.GetComponentsInChildren<Asset.Environment>())
			{
				if (environment == null)
                {
					continue;
                }
				Vector2 parentPosition = (environment.transform.parent == null)
					? new Vector2()
					: new Vector2(environment.transform.parent.transform.position.x, environment.transform.parent.transform.position.z);
				for (int i = 0; i < environment.NumEdges; ++i)
				{
					Vector2 p1 = parentPosition + environment[i].Position;
					Vector2 p2 = parentPosition + environment[i + 1].Position;
					float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
					Handles.color = Color.black;
					Handles.DrawLine(ToV3(p1), ToV3(p2));
				}
			}
		}
	}
}