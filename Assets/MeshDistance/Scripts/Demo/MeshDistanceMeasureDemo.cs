using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MeshDistance.Demo
{
    public class MeshDistanceMeasureDemo : MonoBehaviour
    {
        [SerializeField]
        MeshFilter _meshFilter0;

        [SerializeField]
        MeshFilter _meshFilter1;

        [SerializeField]
        Text _textDistance;

        [SerializeField]
        Text _textInfo;

        [SerializeField]
        Transform _progress;

        bool _isProcessing = false;
        MeshDistanceMeasureAsync _measure = new MeshDistanceMeasureAsync();

        void Update()
        {
            if (!_isProcessing)
            {
                _isProcessing = true;
                StartCoroutine(CoProcess());
            }

        }

        private IEnumerator CoProcess()
        {
            float distance;
            bool busy;
            if (_measure.GetDistance(_meshFilter0, _meshFilter1, out distance, out busy))
            {
                _textDistance.text = $"Distance: {distance}";
            }
            else if (!busy)
            {
                _textInfo.text = $"---";
                IProgress<float> progress = new Progress<float>(normalizedProgress =>
                {
                    _progress.transform.localScale = new Vector3(normalizedProgress, 1.0f, 1.0f);
                });
                Task<MeshDistanceMeasureAsync.MeshDistance> task = _measure.GetDistance(_meshFilter0, _meshFilter1, progress);
                while (!task.IsCompleted)
                {
                    yield return null;
                }
                _textDistance.text = $"Distance: {task.Result.Distance}";
                _textInfo.text = $"Lap: {task.Result.Lap}({task.Result.NumberOfTriangls0} x {task.Result.NumberOfTriangls1})";


            }
            _isProcessing = false;
            yield break;

        }

    }
}