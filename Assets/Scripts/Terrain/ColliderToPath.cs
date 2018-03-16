using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class ColliderToPath : MonoBehaviour
	{
		public Path path;

		public ColliderToPath(Path _path)
		{
			path = _path;
		}
	}
}