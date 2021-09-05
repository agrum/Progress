using UnityEditor;
using UnityEngine;

namespace West.Tool.World
{
	[CustomEditor(typeof(Asset.World.LinearFeature), true)]
	public class LinearFeatureEditor : Editor
	{
		protected Asset.World.LinearFeature linearFeature;
		Plane xz = new Plane(new Vector3(0f, 1f, 0f), 0f);
		bool passedOnce = false;
		protected Vector2 lastCenter;

		protected void OnScene(SceneView sceneview)
		{
			if (linearFeature != null)
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

			linearFeature = (Asset.World.LinearFeature)target;
			lastCenter = ToV2(linearFeature.transform.localPosition);

			SceneView.duringSceneGui += OnScene;
		}

		private void OnDisable()
		{
			linearFeature = null;
		}

		void DrawVisual()
		{
			//draw edges
			var headEnvironment = linearFeature;
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
						generator.Draw();
					}
					else
					{
						headEnvironment.DrawHierarchy();
					}
					break;
				}
			}
		}

		void DrawControl()
		{
			Event guiEvent = Event.current;
			bool guiEventHandled = false;

			if (linearFeature == null)
			{
				return;
			}

			//move all points if moved from main anchor
			Vector2 currentCenter = ToV2(linearFeature.transform.localPosition);
			if ((passedOnce || linearFeature.justDropped) && (lastCenter != currentCenter))
			{
				Undo.RecordObject(linearFeature, "path move");
				for (int i = 0; i < linearFeature.NumEdges; ++i)
				{
					linearFeature[i].Position += currentCenter - lastCenter;
				}
			}
			if ((passedOnce || linearFeature.justDropped) && linearFeature.height != linearFeature.transform.position.y)
			{
				linearFeature.height = linearFeature.transform.position.y;
			}
			passedOnce = true;
			linearFeature.justDropped = false;

			//plane
			xz.distance = -linearFeature.height;

			//define pivot
			Vector2 center = Vector2.zero;
			int e1Candidate = 0;
			int e2Candidate = 0;
			int e3Candidate = 0;
			float candidatesSqrDistanceObserved = 0.0f;
			for (int i = 0; i < linearFeature.NumEdges; ++i)
			{
				for (int j = i + 1; j < linearFeature.NumEdges; ++j)
				{
					float observedSqrDistance = (linearFeature[i].Position - linearFeature[j].Position).sqrMagnitude;
					if (observedSqrDistance > candidatesSqrDistanceObserved)
					{
						e1Candidate = i;
						e2Candidate = j;
						candidatesSqrDistanceObserved = observedSqrDistance;
					}
				}
			}
			center = (linearFeature[e1Candidate].Position + linearFeature[e2Candidate].Position) / 2.0f;
			if (linearFeature.NumEdges > 2)
			{
				candidatesSqrDistanceObserved = 0.0f;
				for (int i = 0; i < linearFeature.NumEdges; ++i)
				{
					float observedSqrDistance = (linearFeature[i].Position - center).sqrMagnitude;
					if (observedSqrDistance > candidatesSqrDistanceObserved && i != e1Candidate && i != e2Candidate)
					{
						e3Candidate = i;
						candidatesSqrDistanceObserved = observedSqrDistance;
					}
				}
				float angle = Vector2.Angle(linearFeature[e1Candidate].Position - linearFeature[e3Candidate].Position, linearFeature[e2Candidate].Position - linearFeature[e3Candidate].Position);
				if (angle <= 90.0f)
				{
					Asset.World.Edge e1 = new Asset.World.Edge(linearFeature[e1Candidate].Position, linearFeature[e2Candidate]);
					Asset.World.Edge e2 = new Asset.World.Edge(linearFeature[e2Candidate].Position, linearFeature[e3Candidate]);
					center = Asset.World.LinearFeature.IntersectionPoint(e1.Center, e1.Center + e1.Normal, e2.Center, e2.Center + e2.Normal);
				}
			}
			//center /= linearFeature.NumPoints();
			lastCenter = center;
			linearFeature.center = center;
			linearFeature.transform.localPosition = new Vector3(center.x, linearFeature.transform.localPosition.y, center.y);

			//draw edge type handles
			Handles.color = Color.white;
			Vector2 parentPosition = linearFeature.ParentPosition;
			for (int i = 0; i < linearFeature.NumEdges; ++i)
			{
				if (linearFeature[i].PreviousEdge == null)
                {
					continue;
                }

				Vector2 p1 = parentPosition + linearFeature[i - 1].Position;
				Vector2 p2 = parentPosition + linearFeature[i].Position;
				float scale = (Camera.current.transform.position - linearFeature.ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Handles.color = Color.black;
				bool clicked = Handles.Button(
					linearFeature.ToV3((p1 + p2) / 2.0f),
					Quaternion.LookRotation(Vector3.up),
					scale,
					scale,
					Handles.CubeHandleCap);
				if (clicked)
				{
					if (guiEvent.shift)
					{
						Undo.RecordObject(linearFeature, "Insert path anchor");
						linearFeature.Split(i);
					}
					else if (guiEvent.control)
					{
						Undo.RecordObject(linearFeature, "Merge path edge");
						linearFeature.Merge(i);
					}
					guiEventHandled = true;
				}
			}

			//draw anchor points
			Handles.color = Color.grey;
			for (int i = 0; i < linearFeature.NumEdges; ++i)
			{
				Vector2 p1 = parentPosition + linearFeature[i].Position;
				Vector2 p2 = parentPosition + linearFeature[i + 1].Position;
				float scale = (Camera.current.transform.position - linearFeature.ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
				Vector3 newPosition = Handles.FreeMoveHandle(linearFeature.ToV3(p1), Quaternion.identity, scale, Vector3.zero, Handles.SphereHandleCap);

				float enter;
				Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
				xz.Raycast(worldRay, out enter);
				Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
				Vector2 newPosition2D = ToV2(newPositionOnPlane);
				if (linearFeature[i].Position != newPosition2D)
				{
					Undo.RecordObject(linearFeature, "Move path anchor");
					linearFeature[i].Position = newPosition2D - parentPosition;
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
				Undo.RecordObject(linearFeature, "Add path point");
				linearFeature.AddSegment(ToV2(mousePos));
			}

			EditorUtility.SetDirty(linearFeature);
		}

		protected Vector2 ToV2(Vector3 v3)
		{
			return new Vector2(v3.x, v3.z);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (linearFeature != null && GUILayout.Button("Add Child Environment"))
			{
				var go = new GameObject("environment");
				go.transform.parent = linearFeature.transform;
				go.transform.localScale = linearFeature.transform.localScale;
				go.transform.localPosition = new Vector3();
				go.AddComponent<Asset.World.Environment>().Init();
			}
		}
	}
}