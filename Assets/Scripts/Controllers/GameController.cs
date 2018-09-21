using LittleWorld.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;

namespace LittleWorld.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private UIController _uiController;
        [SerializeField]
        private Cell _cellPrefab;
        [SerializeField]
        private Transform _parentForCell;

        private Database _database;

        private Stopwatch stopwatch = new Stopwatch();

        private const float _cellOffset = 1.1f;
        private Cell[,] _matrix;
        private Vector2Int _matrixSize;

        public void NextStep()
        {
            EventManager.Trigger(Config.NextStep);
        }

        private void Start()
        {
            _uiController.InitUI();           
            _database = Database.Instance;
        }

        public void GenerateWorld(int x, int z)
        {
            _matrixSize = new Vector2Int(x, z);
            _matrix = new Cell[_matrixSize.x, _matrixSize.y];
            StartCoroutine("GenerateGrid");
        }

        private IEnumerator GenerateGrid()
        {
            yield return null;
            stopwatch.Start();                        
            for (int i = 0; i < _matrixSize.x; i++)
            {
                for (int j = 0; j < _matrixSize.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);

                    var cell = Instantiate(_cellPrefab, _parentForCell);

                    Vector3 cellPos = new Vector3(i * _cellOffset, 0, j * _cellOffset);
                    string cellName = string.Format("Cell [{0}][{1}]", i, j);
                  //  Environment cellEnvironment = _database.GetRandomEnvironment();
                    cell.Init(cellPos, cellName, /*cellEnvironment,*/ index);

                    _matrix[index.x, index.y] = cell;
                }
                yield return null;
            }
            yield return null;

            stopwatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopwatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            UnityEngine.Debug.LogError(elapsedTime);
        }

        public void ResetWorld()
        {
            for (int i = 0; i < _matrixSize.x; i++)
            {
                for (int j = 0; j < _matrixSize.y; j++)
                {
                    Destroy(_matrix[i, j].gameObject);
                }
            }
        }
    }
}