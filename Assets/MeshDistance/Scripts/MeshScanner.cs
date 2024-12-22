using System.Collections.Generic;
using UnityEngine;

namespace MeshDistance
{
    /// <summary>
    /// Scan mesh filter to get triangles. 
    /// </summary>
    public class MeshScanner
    {
        /// <summary>
        /// Scan mesh filter then extract triangles.
        /// </summary>
        /// <param name="meshFilter"></param>
        /// <returns></returns>
        public static Triangle[] ScanTriangls(MeshFilter meshFilter)
        {
            List<Triangle> ret = new List<Triangle>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                Vector3[] vertices = mesh.vertices;
                int[] triangles = mesh.triangles;

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int idx1 = triangles[i];
                    int idx2 = triangles[i + 1];
                    int idx3 = triangles[i + 2];

                    ret.Add(new Triangle(
                        vertices[idx1] + meshFilter.transform.position,
                        vertices[idx2] + meshFilter.transform.position,
                        vertices[idx3] + meshFilter.transform.position
                    ));
                }
                return ret.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}