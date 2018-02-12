﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public float meshResolution;
	public int edgeResolveIterations;

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	void Start() {
		viewMesh = new Mesh ();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine ("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void LateUpdate() {
		DrawFieldOfView ();
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					visibleTargets.Add (target);
				}
			}
		}
	}

	void DrawFieldOfView() {
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3> ();
		ViewCastInfo oldViewCast = new ViewCastInfo ();
		for (int i = 0; i <= stepCount; i++) {
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast (angle);

			if (i > 0) {
				if (oldViewCast.collider != newViewCast.collider || (oldViewCast.collider != null && newViewCast.collider != null)) {
					EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero) {
						viewPoints.Add (edge.pointA);
					}
					if (edge.pointB != Vector3.zero) {
						viewPoints.Add (edge.pointB);
					}
				}

			}


			viewPoints.Add (newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 2;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-3) * 6];

		vertices[0] = Vector3.zero;
		vertices[vertexCount - 1] = new Vector3(0, -100, 0);
		for (int i = 0; i < vertexCount - 2; i++) {
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 3)
			{
				triangles[i * 6] = 0;
				triangles[i * 6 + 1] = i + 1;
				triangles[i * 6 + 2] = i + 2;
				
				triangles[i * 6 + 3] = i + 2;
				triangles[i * 6 + 4] = i + 1;
				triangles[i * 6 + 5] = vertexCount - 1;
			}
		}

		viewMesh.Clear ();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}


	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++) {
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);
			
			if (newViewCast.collider == minViewCast.collider) {
				minAngle = angle;
				minPoint = newViewCast.point;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo (minPoint, maxPoint);
	}


	ViewCastInfo ViewCast(float globalAngle) {
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit2D[] hits = Physics2D.RaycastAll(
			new Vector2(transform.position.x, transform.position.z),
			new Vector2(dir.x, dir.z),
			viewRadius,
			obstacleMask);

		RaycastPathHit raycastPathHit = new RaycastPathHit(
			null, 
			new Vector2(transform.position.x, transform.position.z),
			new Vector2(transform.position.x, transform.position.z) + new Vector2(dir.x, dir.z) * viewRadius);
		foreach (var hit in hits)
		{
			Path path = hit.collider.gameObject.GetComponentInParent<Path>();
			if (path != null)
				path.Raycast(transform.position, dir, ref raycastPathHit, viewRadius, Edge.TypeEnum.BlocksBoth);
			else
			{
				return new ViewCastInfo(
					hit.collider,
					new Vector3(hit.point.x, transform.position.y, hit.point.y),
					hit.distance,
					globalAngle);
			}
		}

		return new ViewCastInfo(
			raycastPathHit.path ? raycastPathHit.path.gameObject.GetComponentInChildren<CircleCollider2D>() : null,
			new Vector3(raycastPathHit.collision.x, transform.position.y, raycastPathHit.collision.y),
			(raycastPathHit.collision - raycastPathHit.origin).magnitude,
			globalAngle);
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo {
		public Collider2D collider;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(Collider2D _collider, Vector3 _point, float _dst, float _angle) {
			collider = _collider;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
			pointA = _pointA;
			pointB = _pointB;
		}
	}

}