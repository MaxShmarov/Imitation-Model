using LittleWorld.Common;
using UnityEngine;

namespace LittleWorld.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Transform _parentForCell;

        private const float _cellOffset = 1.1f;
        private Vector3 _defaultCellSize = new Vector3(1f, 0.1f, 1f);
        private Cell[,] _matrix;

        public void NextStep()
        {
            EventManager.Trigger(Config.NextStep);
        }


        public void GenerateWorld(int x, int z)
        {
            _matrix = new Cell[x, z];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < z; j++)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.SetParent(_parentForCell);
                    var cell = go.AddComponent<Cell>();
                    go.transform.localScale = _defaultCellSize;
                    go.transform.position = new Vector3(i * _cellOffset, 0, j * _cellOffset);
                    go.name = string.Format("Cell [{0}][{1}]", i,j);
                    _matrix[i, j] = cell;
                }            
            }
        }
    }
}