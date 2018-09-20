using LittleWorld.Common;
using System.Collections.Generic;
using UnityEngine;

namespace LittleWorld.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private UIController _uiController;
        [SerializeField]
        private Transform _parentForCell;
        [SerializeField]
        private EnvironmentDatabase _environmentDB;

        public List<Environment> Environments { get; private set; }
        private const float _cellOffset = 1.1f;
        private Cell[,] _matrix;
        private Vector2Int _matrixSize;

        private void Awake()
        {
            Environments = _environmentDB.Environments;
        }

        public void NextStep()
        {
            EventManager.Trigger(Config.NextStep);
        }

        private void Start()
        {
            _uiController.InitUI();
        }

        public void GenerateWorld(int x, int z)
        {
            _matrixSize = new Vector2Int(x, z);
            _matrix = new Cell[_matrixSize.x, _matrixSize.y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < z; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);

                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.SetParent(_parentForCell);

                    Cell cell = go.AddComponent<Cell>();
                    Vector3 cellPos = new Vector3(i * _cellOffset, 0, j * _cellOffset);
                    string cellName = string.Format("Cell [{0}][{1}]", i, j);
                    int randomEnvironmentNumber = Config.GetRandomValue(0, Environments.Count - 1);
                    Environment cellEnvironment = Environments[randomEnvironmentNumber];
                    cell.Init(cellPos, cellName, cellEnvironment, index);
                    
                    _matrix[index.x, index.y] = cell;
                    
                }            
            }
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
            _uiController.InitUI();
        }
    }
}