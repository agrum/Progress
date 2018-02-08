using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor {

	PathCreator creator;
	Path path;
	Plane xz = new Plane(new Vector3(0f, 1f, 0f), 0f);
	bool passedOnce = false;
	Vector2 lastCenter;

	void OnScene(SceneView sceneview)
	{
		if (creator != null)
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
		if(creator.transform.position.y != xz.distance)
		{
			xz.distance = -creator.transform.position.y;
		}
		DrawControl();
		DrawVisual();
	}

	void OnEnable()
	{
		creator = (PathCreator)target;
		if(creator.path == null)
		{
			creator.CreatePath();
		}
		path = creator.path;
		
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
			float scale = (Camera.current.transform.position - ToV3((path[i] + path[i+1])/2.0f)).magnitude/30.0f;
			scale = Mathf.Min(scale, 1.0f);
			Handles.color = ColorFrom(path.GetType(i));
			Handles.DrawLine(ToV3(path[i]), ToV3(path[i + 1]));
			Handles.ArrowHandleCap(
				0, 
				ToV3((path[i] + path[i + 1]) / 2.0f), 
				Quaternion.LookRotation(ToV3(new Vector2((path[i + 1] - path[i]).y, -(path[i + 1] - path[i]).x), false)), 
				scale, 
				EventType.Repaint);
		}
	}

	void DrawControl()
	{
		Event guiEvent = Event.current;
		bool guiEventHandled = false;

		//move all points if moved from main anchor
		Vector2 currentCenter = ToV2(creator.transform.position);
		if (passedOnce && lastCenter != currentCenter)
		{
			for (int i = 0; i < path.NumPoints(); ++i)
			{
				path[i] += currentCenter - lastCenter;
			}
		}
		passedOnce = true;

		//define pivot
		Vector2 center = Vector2.zero;
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			center += path[i];
		}
		center /= path.NumPoints();
		lastCenter = center;
		creator.transform.position = ToV3(center);

		//draw edge type handles
		Handles.color = Color.white;
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			Handles.color = ColorFrom(path.GetType(i));
			bool clicked = Handles.Button(
				ToV3((path[i] + path[i + 1]) / 2.0f), 
				Quaternion.LookRotation(Vector3.up), 
				0.4f, 
				0.4f, 
				Handles.CubeHandleCap);
			if (clicked)
			{
				if (guiEvent.shift)
				{
					Undo.RecordObject(creator, "Insert path anchor");
					path.Split(i);
				}
				else if (guiEvent.control)
				{
					Undo.RecordObject(creator, "Merge path edge");
					path.Merge(i);
				}
				else
				{
					Undo.RecordObject(creator, "Change path edge");
					path.ChangeType(i);
				}
				guiEventHandled = true;
			}
		}

		//draw anchor points
		Handles.color = Color.grey;
		for(int i = 0; i < path.NumPoints(); ++i)
		{
			float scale = (Camera.current.transform.position - ToV3((path[i] + path[i + 1]) / 2.0f)).magnitude / 45.0f;
			scale = Mathf.Min(scale, 0.6f);
			Vector3 newPosition = Handles.FreeMoveHandle(ToV3(path[i]), Quaternion.identity, scale, Vector3.zero, Handles.SphereHandleCap);

			float enter;
			Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
			xz.Raycast(worldRay, out enter);
			Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
			Vector2 newPosition2D = ToV2(newPositionOnPlane);
			if (path[i] != newPosition2D)
			{
				Undo.RecordObject(creator, "Move path anchor");
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
			Undo.RecordObject(creator, "Add path point");
			path.AddSegment(ToV2(mousePos));
		}
	}

	Vector3 ToV3(Vector2 v2, bool applyCreatorHeight = true)
	{
		return new Vector3(v2.x, applyCreatorHeight ? creator.transform.position.y : 0.0f, v2.y);
	}

	Vector2 ToV2(Vector3 v3)
	{
		return new Vector2(v3.x, v3.z);
	}
}
