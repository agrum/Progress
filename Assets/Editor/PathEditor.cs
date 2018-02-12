using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor {
	
	Path path;
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
			SceneView.onSceneGUIDelegate -= OnScene;
		}
	}

	void OnSceneGUI()
	{
		DrawControl();
		DrawVisual();
	}

	void OnEnable()
	{
		if(PrefabUtility.GetPrefabParent(target) == null && PrefabUtility.GetPrefabObject(target) != null)
			return;

		path = (Path)target;
		//if (path.points == null)
		//	path.InitPath();
		
		SceneView.onSceneGUIDelegate += OnScene;
	}

	Color ColorFrom(Edge.TypeEnum type)
	{
		switch (type)
		{
			case Edge.TypeEnum.BlocksMovement: return Color.yellow;
			case Edge.TypeEnum.BlocksVision: return Color.green;
			case Edge.TypeEnum.BlocksBoth: return Color.red;
		}
		return Color.black;
	}

	void DrawVisual()
	{
		//draw edges
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			float scale = (Camera.current.transform.position - ToV3((path[i] + path[i+1])/2.0f)).magnitude/50.0f;
			Handles.color = ColorFrom(path.GetType(i));
			Handles.DrawLine(ToV3(path[i]), ToV3(path[i + 1]));
			Handles.ArrowHandleCap(
				0, 
				ToV3((path[i] + path[i + 1]) / 2.0f), 
				Quaternion.LookRotation(ToV3(new Vector2((path[i + 1] - path[i]).y, -(path[i + 1] - path[i]).x), false)), 
				scale, 
				EventType.Repaint);
		}

		//draw unity collider radius
		Handles.color = Color.gray;
		Handles.DrawWireArc(path.transform.position, Vector3.up, Vector3.forward, 360, path.circleCollider.radius);
	}

	void DrawControl()
	{
		Event guiEvent = Event.current;
		bool guiEventHandled = false;

		//move all points if moved from main anchor
		Vector2 currentCenter = ToV2(path.transform.position);
		if ((passedOnce || path.justDropped) && (lastCenter != currentCenter))
		{
			Undo.RecordObject(path, "path move");
			for (int i = 0; i < path.NumPoints(); ++i)
			{
				path[i] += currentCenter - lastCenter;
			}
		}
		if((passedOnce || path.justDropped) && path.height != path.transform.position.y)
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
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			for (int j = i+1; j < path.NumPoints(); ++j)
			{
				float observedSqrDistance = (path[i] - path[j]).sqrMagnitude;
				if (observedSqrDistance > candidatesSqrDistanceObserved)
				{
					e1Candidate = i;
					e2Candidate = j;
					candidatesSqrDistanceObserved = observedSqrDistance;
				}
			}
		}
		center = (path[e1Candidate] + path[e2Candidate]) / 2.0f;
		if (path.NumPoints() > 2)
		{
			candidatesSqrDistanceObserved = 0.0f;
			for (int i = 0; i < path.NumPoints(); ++i)
			{
				float observedSqrDistance = (path[i] - center).sqrMagnitude;
				if (observedSqrDistance > candidatesSqrDistanceObserved && i != e1Candidate && i != e2Candidate)
				{
					e3Candidate = i;
					candidatesSqrDistanceObserved = observedSqrDistance;
				}
			}
			float angle = Vector2.Angle(path[e1Candidate] - path[e3Candidate], path[e2Candidate] - path[e3Candidate]);
			if (angle <= 90.0f)
			{
				Vector2 p1, q1, p2, q2;
				p1 = (path[e1Candidate] + path[e2Candidate]) / 2;
				q1 = p1 + (new Vector2(path[e1Candidate].y, -path[e1Candidate].x) - new Vector2(path[e2Candidate].y, -path[e2Candidate].x));
				p2 = (path[e2Candidate] + path[e3Candidate]) / 2;
				q2 = p2 + (new Vector2(path[e2Candidate].y, -path[e2Candidate].x) - new Vector2(path[e3Candidate].y, -path[e3Candidate].x));
				center = Path.IntersectionPoint(p1, q1, p2, q2);
			}
		}
		//center /= path.NumPoints();
		lastCenter = center;
		path.center = center;
		path.transform.position = ToV3(center);
		path.circleCollider.transform.position = new Vector3(center.x, center.y, 0);

		//define radius
		float radius = 0.0f;
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			radius = Mathf.Max(radius, (path[i] - center).magnitude);
		}
		path.circleCollider.radius = radius;

		//draw edge type handles
		Handles.color = Color.white;
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			float scale = (Camera.current.transform.position - ToV3((path[i] + path[i + 1]) / 2.0f)).magnitude / 50.0f;
			Handles.color = ColorFrom(path.GetType(i));
			bool clicked = Handles.Button(
				ToV3((path[i] + path[i + 1]) / 2.0f), 
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
				else
				{
					Undo.RecordObject(path, "Change path edge");
					path.ChangeType(i);
				}
				guiEventHandled = true;
			}
		}

		//draw anchor points
		Handles.color = Color.grey;
		for(int i = 0; i < path.NumPoints(); ++i)
		{
			float scale = (Camera.current.transform.position - ToV3((path[i] + path[i + 1]) / 2.0f)).magnitude / 50.0f;
			Vector3 newPosition = Handles.FreeMoveHandle(ToV3(path[i]), Quaternion.identity, scale, Vector3.zero, Handles.SphereHandleCap);

			float enter;
			Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
			xz.Raycast(worldRay, out enter);
			Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
			Vector2 newPosition2D = ToV2(newPositionOnPlane);
			if (path[i] != newPosition2D)
			{
				Undo.RecordObject(path, "Move path anchor");
				path[i] = newPosition2D;
				guiEventHandled = true;
			}
		}

		//Input
		if (!guiEventHandled && guiEvent.type == EventType.mouseDown && guiEvent.button == 0 && guiEvent.shift)
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
}
