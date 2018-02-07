using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor {

	PathCreator creator;
	Path path;
	Plane xz = new Plane(new Vector3(0f, 1f, 0f), 0f);
	Vector2 lastCenter;

	void OnSceneGUI()
	{
		if(creator.transform.position.y != xz.distance)
		{
			xz.distance = -creator.transform.position.y;
		}
		Input();
		Draw();
	}

	void Input()
	{
		Event guiEvent = Event.current;

		if(guiEvent.type == EventType.mouseDown && guiEvent.button == 0 && guiEvent.shift)
		{
			float enter;
			Ray worldRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
			xz.Raycast(worldRay, out enter);
			Vector3 mousePos = worldRay.GetPoint(enter);
			Undo.RecordObject(creator, "Add path point");
			path.AddSegment(ToV2(mousePos));
		}
	}

	void OnEnable()
	{
		creator = (PathCreator)target;
		if(creator.path == null)
		{
			creator.CreatePath();
		}
		path = creator.path;
	}

	void Draw()
	{
		//move all points if moved from main anchor
		Vector2 currentCenter = ToV2(creator.transform.position);
		if (lastCenter != null && lastCenter != currentCenter)
		{
			for (int i = 0; i < path.NumPoints(); ++i)
			{
				path[i] += currentCenter - lastCenter;
			}
		}

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
			bool clicked = Handles.Button(ToV3((path[i] + path[i + 1]) / 2.0f), Quaternion.identity, 0.4f, 0.4f, Handles.RectangleHandleCap);
			if (clicked)
			{
				Undo.RecordObject(creator, "Change path edge");
				path.ChangeType(i);
			}
		}

		//draw edges
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			switch(path.GetType(i))
			{
				case Edge.TypeEnum.BlocksMovement: Handles.color = Color.yellow; break;
				case Edge.TypeEnum.BlocksVision: Handles.color = Color.green; break;
				case Edge.TypeEnum.BlocksBoth: Handles.color = Color.black; break;
			}
			Handles.DrawLine(ToV3(path[i]), ToV3(path[i + 1]));
			Handles.ArrowHandleCap(0, ToV3(path[i]), Quaternion.LookRotation(ToV3(path[i + 1] - path[i], false)), 1.2f, EventType.Repaint);
		}

		//draw anchor points
		Handles.color = Color.red;
		for(int i = 0; i < path.NumPoints(); ++i)
		{
			Vector3 newPosition = Handles.FreeMoveHandle(ToV3(path[i]), Quaternion.identity, 0.4f, Vector3.zero, Handles.SphereHandleCap);

			float enter;
			Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
			xz.Raycast(worldRay, out enter);
			Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
			Vector2 newPosition2D = ToV2(newPositionOnPlane);
			if (path[i] != newPosition2D)
			{
				Undo.RecordObject(creator, "Move path anchor");
				path[i] = newPosition2D;
			}
		}

		//draw delete handles
		Handles.color = Color.red;
		for (int i = 0; i < path.NumPoints(); ++i)
		{
			bool clicked = Handles.Button(ToV3(path[i] + (path[i + 1] - path[i]).normalized / 2.0f), Quaternion.identity, 0.1f, 0.2f, Handles.RectangleHandleCap);
			if (clicked)
			{
				Undo.RecordObject(creator, "Remove path anchor");
				path.Remove(i);
				--i;
			}
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
