using UnityEngine;
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
	public MeshFilter obstacleMeshFilter;

	Mesh viewMesh;
	Mesh obstacleMesh;

	HashSet<Path> pathHitList = new HashSet<Path>();
	List<Edge> facingEdgeList = new List<Edge>();

	void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		int stepCount = Mathf.RoundToInt(viewAngle * Mathf.Deg2Rad * viewRadius / meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		int vertexCount = stepCount + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[stepCount * 3];

		vertices[stepCount] = Vector3.zero;
		for (int i = 0; i < stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			Vector3 dir = DirFromAngle(angle, true);
			vertices[i + 1] = dir * viewRadius;
			
			triangles[i * 3] = stepCount;
			triangles[i * 3 + 1] = i;
			triangles[i * 3 + 2] = (i + 1) % stepCount;
		}

		viewMesh.Clear();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();

		obstacleMesh = new Mesh();
		obstacleMesh.name = "Obstacle Mesh";
		obstacleMeshFilter.mesh = obstacleMesh;

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

	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * Mathf.Deg2Rad * viewRadius / meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		pathHitList.Clear();
		facingEdgeList.Clear();

		//find intersection paths
		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			Vector3 dir = DirFromAngle(angle, true);
			RaycastHit2D[] hits = Physics2D.RaycastAll(
				pos,
				new Vector2(dir.x, dir.z),
				viewRadius,
				obstacleMask);
			foreach (var hit in hits)
			{
				pathHitList.Add(hit.collider.gameObject.GetComponentInParent<Path>());
			}
		}

		//find facing edges in intersecting paths
		float sqrViewRadius = 2.0f * viewRadius * viewRadius;
		foreach (var path in pathHitList)
		{
			for(int i = 0; i < path.NumEdges; ++i)
			{
				Vector2 posToEdge = path[i].Center - pos;
				if (Vector2.Dot(path[i].Normal, posToEdge) < 0 && posToEdge.sqrMagnitude <= sqrViewRadius)
				{
					facingEdgeList.Add(path[i]);
				}
			}
		}

		//draw quad for each facing edge
		int vertexCount = facingEdgeList.Count * 6;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[facingEdgeList.Count * 4 * 3];
		
		for (int i = 0; i < facingEdgeList.Count; ++i)
		{
			Edge edge = facingEdgeList[i];
			vertices[i * 6 + 0] = transform.InverseTransformPoint(new Vector3(edge.Position.x, 0, edge.Position.y));
			vertices[i * 6 + 1] = transform.InverseTransformPoint(new Vector3(edge.NextEdge.Position.x, 0, edge.NextEdge.Position.y));
			vertices[i * 6 + 2] = vertices[i * 6 + 0].normalized * viewRadius * 2;
			vertices[i * 6 + 3] = vertices[i * 6 + 1].normalized * viewRadius * 2;
			vertices[i * 6 + 4] = vertices[i * 6 + 2] - new Vector3(vertices[i * 6 + 3].z - vertices[i * 6 + 2].z, 0, vertices[i * 6 + 2].x - vertices[i * 6 + 3].x);
			vertices[i * 6 + 5] = vertices[i * 6 + 4] - vertices[i * 6 + 2] + vertices[i * 6 + 3];

			triangles[i * 12 + 0] = i * 6 + 1;
			triangles[i * 12 + 1] = i * 6 + 0;
			triangles[i * 12 + 2] = i * 6 + 2;
			triangles[i * 12 + 3] = i * 6 + 1;
			triangles[i * 12 + 4] = i * 6 + 2;
			triangles[i * 12 + 5] = i * 6 + 3;

			triangles[i * 12 + 6] = i * 6 + 2;
			triangles[i * 12 + 7] = i * 6 + 4;
			triangles[i * 12 + 8] = i * 6 + 5;
			triangles[i * 12 + 9] = i * 6 + 2;
			triangles[i * 12 + 10] = i * 6 + 5;
			triangles[i * 12 + 11] = i * 6 + 3;
		}

		obstacleMesh.Clear();

		obstacleMesh.vertices = vertices;
		obstacleMesh.triangles = triangles;
		obstacleMesh.RecalculateNormals();
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
			0,
			new Vector2(transform.position.x, transform.position.z),
			new Vector2(transform.position.x, transform.position.z) + new Vector2(dir.x, dir.z) * viewRadius);
		foreach (var hit in hits)
		{
			Path path = hit.collider.gameObject.GetComponentInParent<Path>();
			if (path != null)
				path.Raycast(transform.position, dir, ref raycastPathHit, viewRadius, Edge.TypeEnum.BlocksBoth);
			else
			{
				Debug.Log("Collided with non path element");
			}
		}

		return new ViewCastInfo(
			raycastPathHit.path ? raycastPathHit.path : null,
			raycastPathHit.index,
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

	public struct ViewCastInfo
	{
		public Path path;
		public int index;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(Path _path, int _index, Vector3 _point, float _dst, float _angle) {
			path = _path;
			index = _index;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
			pointA = _pointA;
			pointB = _pointB;
		}
	}

}