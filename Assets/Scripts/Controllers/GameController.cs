using LittleWorld.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;

namespace LittleWorld.Controllers
{
    public class GameController : Helpers.Singleton<GameController>
    {
        [SerializeField]
        private UIController _uiController;
        [SerializeField]
        private Cell _cellPrefab;
        [SerializeField]
        private Transform _parentForCell;

        private Stopwatch stopwatch = new Stopwatch();

        private const float _cellOffset = 1.1f;
        private Cell[,] _matrix;
        private Vector2Int _matrixSize;
        private Cell _currentCell;

        private void OnEnable()
        {
            EventManager<Cell>.StartListening("Select", SelectCell);
        }

        private void OnDisable()
        {
            EventManager<Cell>.StopListening("Select", SelectCell);
        }

        private void SelectCell(Cell cell)
        {
            if (_currentCell != null)
                _currentCell.Select(false);
            _currentCell = cell;
            _currentCell.Select(true);
        }

        private void Start()
        {
            _uiController.InitUI();           
        }

        public void GenerateWorld(int x, int z)
        {
            _matrixSize = new Vector2Int(x, z);
            _matrix = new Cell[_matrixSize.x, _matrixSize.y];
            CameraHandler.Instance.SetLimitsX(new Vector2Int(-2, z + 2));
            CameraHandler.Instance.SetLimitsZ(new Vector2Int(-3, z + 3));
            StartCoroutine("GenerateGrid");
        }

        private IEnumerator GenerateGrid()
        {
            stopwatch.Start();                        
            for (int i = 0; i < _matrixSize.x; i++)
            {
                for (int j = 0; j < _matrixSize.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);

                    var cell = Instantiate(_cellPrefab, _parentForCell);

                    Vector3 cellPos = new Vector3(i * _cellOffset, 0, j * _cellOffset);
                    string cellName = string.Format("Cell [{0}][{1}]", i, j);
                    cell.Init(cellPos, cellName, index);

                    _matrix[index.x, index.y] = cell;
                }
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

        public void NextStep()
        {
            EventManager.Trigger(Config.NextStep);
        }

        public Cell GetCellByPosition(int x, int y)
        {
            if (x < _matrixSize.x && x >= 0
                && y < _matrixSize.y && y >= 0)
                return _matrix[x, y];
            else
                return null;
        }
    }
}