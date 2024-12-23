using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MeshDistance
{
    /// <summary>
    /// Class for calculating distance between two meshes asynchronously.
    /// </summary>
    public class MeshDistanceMeasureAsync
    {
        /// <summary>
        /// Cache to avoid recalculating distances once calculated.
        /// </summary>
        private class Cache
        {
            public float Distance;
            public bool Active;
            public Vector3 Position0;
            public Quaternion Rotation0;
            public Vector3 Scale0;
            public Vector3 Position1;
            public Quaternion Rotation1;
            public Vector3 Scale1;

            public Cache()
            {
                Active = false;
            }
        }


        ConcurrentDictionary<(MeshFilter, MeshFilter), Cache> _results = new ConcurrentDictionary<(MeshFilter, MeshFilter), Cache>();

        /// <summary>
        /// Calculate distance between two meshes. 
        /// </summary>
        /// <param name="m0">1st mesh</param>
        /// <param name="m1">2nd mesh</param>
        /// <param name="distance">distance between two meshes</param>
        /// <param name="busy">it is true when calculating is under progress.</param>
        /// <returns></returns>
        public bool GetDistance(MeshFilter m0, MeshFilter m1, out float distance, out bool busy)
        {
            distance = 0;
            busy = false;
            if (!_results.ContainsKey((m0, m1)) || _results[(m0, m1)] == null)
            {
                return false;
            }
            Cache cache = _results[(m0, m1)];
            Transform t0 = m0.transform;
            Transform t1 = m1.transform;

            if (!cache.Active)
            {
                busy = true;
                return false;
            }

            if (cache.Position0 != t0.position || cache.Rotation0 != t0.rotation || cache.Scale0 != t0.lossyScale
                || cache.Position1 != t1.position || cache.Rotation1 != t1.rotation || cache.Scale1 != t1.lossyScale)
            {
                return false;
            }

            distance = _results[(m0, m1)].Distance;
            return true;

        }

        /// <summary>
        /// Get distance between two meshes.
        /// </summary>
        /// <param name="m0">1st mesh</param>
        /// <param name="m1">2nd mesh</param>
        /// <param name="progress">callback to notify progress(0..1)</param>
        /// <returns>distance information</returns>
        public Task<MeshDistance> GetDistance(MeshFilter m0, MeshFilter m1, IProgress<float> progress)
        {
            if (_results.ContainsKey((m0, m1)) && !_results[(m0, m1)].Active)
            {
                return null;
            }
            _results[(m0, m1)] = new Cache();
            DateTime lap0 = DateTime.Now;
            Triangle[] triangls0 = MeshScanner.ScanTriangls(m0);
            Triangle[] triangls1 = MeshScanner.ScanTriangls(m1);
            Transform t0 = m0.transform;
            Transform t1 = m1.transform;
            _results[(m0, m1)].Position0 = t0.position;
            _results[(m0, m1)].Rotation0 = t0.rotation;
            _results[(m0, m1)].Scale0 = t0.lossyScale;
            _results[(m0, m1)].Position1 = t1.position;
            _results[(m0, m1)].Rotation1 = t1.rotation;
            _results[(m0, m1)].Scale1 = t1.lossyScale;
            return Task.Run(() =>
            {
                float distance = MeshDistanceMeasure.GetDistance(triangls0, triangls1, progress, true);
                DateTime lap1 = DateTime.Now;
                _results[(m0, m1)].Distance = distance;
                _results[(m0, m1)].Active = true;
                return new MeshDistance(distance, (lap1 - lap0).TotalMilliseconds, triangls0.Length, triangls1.Length);
            });
        }


    }
}