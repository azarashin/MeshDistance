using UnityEngine;

namespace MeshDistance
{
    /// <summary>
    /// information for distance between two meshes. 
    /// </summary>
    public class MeshDistance
    {
        /// <summary>
        /// distance between two meshes.
        /// </summary>
        public float Distance;

        /// <summary>
        /// Lap time to calculate distance between two meshes.
        /// </summary>
        public double Lap;

        /// <summary>
        /// number of triangles in 1st mesh.
        /// </summary>
        public int NumberOfTriangles0;

        /// <summary>
        /// number of triangles in 2nd mesh.
        /// </summary>
        public int NumberOfTriangles1;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="distance">distance between two meshes.</param>
        /// <param name="lap">Lap time to calculate distance between two meshes.</param>
        /// <param name="numberOfTriangls0">number of triangles in 1st mesh.</param>
        /// <param name="numberOfTriangls1">number of triangles in 2nd mesh.</param>
        public MeshDistance(float distance, double lap, int numberOfTriangls0, int numberOfTriangls1)
        {
            Distance = distance;
            Lap = lap;
            NumberOfTriangles0 = numberOfTriangls0;
            NumberOfTriangles1 = numberOfTriangls1;
        }
    }
}