using UnityEngine;

namespace MeshDistance
{
    /// <summary>
    /// Keep 3 vertics
    /// </summary>
    public struct Triangle
    {
        public Vector3 p1, p2, p3;

        public Triangle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            p1 = point1;
            p2 = point2;
            p3 = point3;
        }
    }
}
