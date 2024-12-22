using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshDistance
{
    /// <summary>
    /// Measure distance between meshes. 
    /// </summary>
    public static class MeshDistanceMeasure
    {
        /// <summary>
        /// Get distance between two meshes.
        /// </summary>
        /// <param name="m0">1st mesh</param>
        /// <param name="m1">2nd mesh</param>
        /// <returns>distance</returns>
        public static float GetDistance(MeshFilter m0, MeshFilter m1)
        {
            Triangle[] triangls0 = MeshScanner.ScanTriangls(m0);
            Triangle[] triangls1 = MeshScanner.ScanTriangls(m1);
            return GetDistance(triangls0, triangls1);
        }

        /// <summary>
        /// Get distance between two meshes.
        /// </summary>
        /// <param name="m0">1st mesh</param>
        /// <param name="m1">2nd mesh</param>
        /// <param name="progress">callback to notify progress(0..1)</param>
        /// <returns>distance</returns>
        public static float GetDistance(Triangle[] triangls0, Triangle[] triangls1, IProgress<float> progress = null)
        {
            float minDistance = float.MaxValue;
            for (int i = 0; i < triangls0.Length; i++)
            {
                Triangle triangl0 = triangls0[i];
                for (int j = 0; j < triangls1.Length; j++)
                {
                    Triangle triangl1 = triangls1[j];

                    float distance = GetTriangleDistance(triangl0.p1, triangl0.p2, triangl0.p3,
                        triangl1.p1, triangl1.p2, triangl1.p3);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
                progress?.Report((float)i / triangls0.Length);

            }
            progress?.Report(1.0f);
            return minDistance;

        }



        /// <summary>
        /// Main function to find the shortest distance between triangles
        /// </summary>
        /// <param name="p1">1st vertex in triangle 1</param>
        /// <param name="p2">2nd vertex in triangle 1</param>
        /// <param name="p3">3rd vertex in triangle 1</param>
        /// <param name="q1">1st vertex in triangle 2</param>
        /// <param name="q2">2nd vertex in triangle 2</param>
        /// <param name="q3">3rd vertex in triangle 2</param>
        /// <returns>distance between triangle 1 and triangle 2</returns>
        public static float GetTriangleDistance(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            float minDistance = float.MaxValue;

            // Find the shortest distance between the vertex of triangle 1 and the side of triangle 2
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p1, q1, q2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p1, q2, q3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p1, q3, q1));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p2, q1, q2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p2, q2, q3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p2, q3, q1));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p3, q1, q2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p3, q2, q3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(p3, q3, q1));

            // Find the shortest distance between the vertex of triangle 2 and the side of triangle 1
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q1, p1, p2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q1, p2, p3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q1, p3, p1));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q2, p1, p2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q2, p2, p3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q2, p3, p1));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q3, p1, p2));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q3, p2, p3));
            minDistance = Mathf.Min(minDistance, GetPointToEdgeDistance(q3, p3, p1));

            return minDistance;
        }

        /// <summary>
        /// Helper function to calculate the shortest distance between a point and an edge
        /// </summary>
        /// <param name="point"></param>
        /// <param name="edgeStart"></param>
        /// <param name="edgeEnd"></param>
        /// <returns></returns>
        public static float GetPointToEdgeDistance(Vector3 point, Vector3 edgeStart, Vector3 edgeEnd)
        {
            Vector3 edgeDir = edgeEnd - edgeStart;
            Vector3 pointDir = point - edgeStart;

            float edgeLengthSquared = edgeDir.sqrMagnitude;

            if (edgeLengthSquared == 0.0f)
            {
                return (point - edgeStart).magnitude;
            }

            // Calculate projection ratio (length of straight line perpendicular to side from point)
            float t = Mathf.Clamp01(Vector3.Dot(pointDir, edgeDir) / edgeLengthSquared);

            Vector3 projection = edgeStart + t * edgeDir;

            return (point - projection).magnitude;
        }
    }
}