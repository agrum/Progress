using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor {

	PathCreator creator;
	Path path;
	Plane xz = new Plane(new Vector3(0f, 1f, 0f), 0f);

	void OnSceneGUI()
	{
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
			path.AddSegment(new Vector2(mousePos.x, mousePos.z));
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
		Handles.color = Color.yellow;
		for (int i = 0; i < path.NumSegments(); ++i)
		{
			Handles.DrawLine(new Vector3(path[i].x, 0, path[i].y), new Vector3(path[i+1].x, 0, path[i+1].y));
		}

		Handles.color = Color.red;
		for(int i = 0; i < path.NumPoints(); ++i)
		{
			Vector3 newPosition = Handles.FreeMoveHandle(new Vector3(path[i].x, 0, path[i].y), Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);

			float enter;
			Ray worldRay = new Ray(Camera.current.transform.position, newPosition - Camera.current.transform.position);
			xz.Raycast(worldRay, out enter);
			Vector3 newPositionOnPlane = worldRay.GetPoint(enter);
			Vector2 newPosition2D = new Vector2(newPositionOnPlane.x, newPositionOnPlane.z);
			if (path[i] != newPosition2D)
			{
				Undo.RecordObject(creator, "Move path point");
				path[i] = newPosition2D;
			}
		}
	}
}
