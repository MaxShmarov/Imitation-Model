using System;
using UnityEngine;
using UnityEngine.EventSystems;
using LittleWorld.Common;
using LittleWorld.UI;
using cakeslice;
using LittleWorld.Controllers;

namespace LittleWorld
{
    public class CurrentWeather
    {
        public int RainyIntensity;
        public int SunnyIntensity;
    }

    public class Cell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private CellUI _cellUI;

        private Vector3 _defaultCellSize = new Vector3(1f, 1f, 1f);
        private Environment _environment;
        private int _currentGrass = 0;
        private int _rabbitCount = 0;
        private Transform _transform;
        private bool _waterBeside = false;
        private bool _knowNeighbours = false;
        private bool _showCanvas = true;
        private bool _cellFound = false;

        public Vector2Int _positionInMatrix { get; private set; }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StartListening("CanvasShow", ShowCanvas);
            EventManager.StartListening("AddRabbits", AddRabbitsClickHandler);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StopListening("CanvasShow", ShowCanvas);
            EventManager.StopListening("AddRabbits", AddRabbitsClickHandler);
        }

        private void AddRabbitsClickHandler()
        {
            _rabbitCount = AddRabbits();
            _cellUI.UpdateUI(-1, -1, _currentGrass, _rabbitCount);
        }

        public void Init(Vector3 position, string cellName, Environment environment, Vector2Int index)
        {
            _transform = transform;
            _transform.localScale = _defaultCellSize;
            _transform.position = position;
            name = cellName;
            _environment = environment;
            _positionInMatrix = index;
            SetEnvironmentData();           
            InitUIVariables();
        }

        private void SetEnvironmentData()
        {
            if (_environment == null || _renderer == null)
                return;
            _renderer.material.color = _environment.Color;
        }

        private void UpdateCellVariables()
        {
            UpdateRabbits();
            if (!_knowNeighbours)
            {
                CheckNeighbours();              
                _knowNeighbours = true;
            }

            var _currentWeather = Config.GetRandomWeather();
            _currentGrass -= _rabbitCount;
            if (_currentGrass < 0)
            {
                _currentGrass = 0;
            }
            _currentGrass = Config.UpdateGrass(_environment.Type, _currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _waterBeside);
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _rabbitCount);
        }

        private void UpdateRabbits()
        {
            if (_rabbitCount != 0)
            {
                if (_rabbitCount == 2)
                {
                    _rabbitCount = Config.AddRabbit(_rabbitCount);
                }
                while (_currentGrass < _rabbitCount)
                {
                    CheckNeighbours();
                }
            }
        }

        private void CheckNeighbours()
        {
            _waterBeside = false;
            _cellFound = false;
            int x = _positionInMatrix.x;
            int y = _positionInMatrix.y;
            CheckCell(x + 1, y);
            CheckCell(x, y + 1);
            CheckCell(x - 1, y);
            CheckCell(x, y - 1);
            CheckCell(x + 1, y + 1);
            CheckCell(x + 1, y - 1);
            CheckCell(x - 1, y + 1);
            CheckCell(x - 1, y - 1, true);
        }

        private void CheckCell(int x, int y, bool lastTry = false)
        {
            var cell = GameController.Instance.GetCellByPosition(x, y);
            if (cell != null)
            {
                switch(cell._environment.Type)
                {
                    case EnvironmentType.Lake:
                        _waterBeside = true;
                        break;
                    case EnvironmentType.Mountain:
                        break;
                    case EnvironmentType.Field:
                        if (cell._rabbitCount < 3 && cell._currentGrass > cell._rabbitCount && _rabbitCount > 0)
                        {
                            _rabbitCount = Config.RemoveRabbit(_rabbitCount);
                            cell._rabbitCount = Config.AddRabbit(cell._rabbitCount);
                            cell._cellUI.UpdateUI(-1, -1, cell._currentGrass, cell._rabbitCount);
                            _cellFound = true;
                        }
                        break;
                }
      
            }
            if (lastTry && !_cellFound)
            {
                _rabbitCount = Config.RemoveRabbit(_rabbitCount);
            }
        }

        private int AddRabbits()
        {
            int rand = 0;
            if (_environment.Type == EnvironmentType.Field)
            {
                rand = UnityEngine.Random.Range(1, 4);
                _rabbitCount = rand;
            }
            return rand;
        }

        private void InitUIVariables()
        {
            var _currentWeather = Config.GetRandomWeather();
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _rabbitCount);
        }

        private void ShowCanvas(bool enabled)
        {
            _showCanvas = enabled;
            _cellUI.gameObject.SetActive(_showCanvas);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            ShowCanvas(!_showCanvas);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameController.Instance.UpdateCoordsCell(_positionInMatrix.x, _positionInMatrix.y);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameController.Instance.ClearCoords();
        }
    }
}