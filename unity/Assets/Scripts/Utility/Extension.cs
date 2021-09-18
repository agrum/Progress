using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public static class Vector2Extension
    {
        public static float ManhattanDistance(this Vector2 v0)
        {
            return Mathf.Abs(v0.x) + Mathf.Abs(v0.y);
        }

        public static bool InCell(this Vector2 v0, float cellHalfSize_)
        {
            return Mathf.Abs(v0.x) <= cellHalfSize_ && Mathf.Abs(v0.y) <= cellHalfSize_;
        }
    }
}