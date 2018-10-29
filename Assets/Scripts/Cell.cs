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

        public Vector2Int _positionInMatrix { get; private set; }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StartListening("CanvasShow", ShowCanvas);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StopListening("CanvasShow", ShowCanvas);
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
            if (!_knowNeighbours)
            {
                CheckNeighbours();
                AddRabbits();
                _knowNeighbours = true;
            }
            
            var _currentWeather = Config.GetRandomWeather();
            var tempRabbits = Config.UpdateRabbits(_currentGrass, _rabbitCount);
            if (_rabbitCount == -1)
            {

            }
            else
            {
                _rabbitCount = tempRabbits;
            }
            _currentGrass = Config.UpdateGrass(_environment.Type, _currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _waterBeside);
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _rabbitCount);
        }

        private void CheckNeighbours()
        {
            _waterBeside = false;
            int x = _positionInMatrix.x;
            int y = _positionInMatrix.y;
            CheckCell(x + 1, y);
            CheckCell(x, y + 1);
            CheckCell(x - 1, y);
            CheckCell(x, y - 1);
            CheckCell(x + 1, y + 1);
            CheckCell(x + 1, y - 1);
            CheckCell(x - 1, y + 1);
            CheckCell(x - 1, y - 1);
        }

        private void CheckCell(int x, int y)
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
                        if (cell._rabbitCount < 3 && cell._currentGrass > _rabbitCount)
                        {
                            _rabbitCount++;
                        }
                        break;
                }
      
            }
        }

        private void AddRabbits()
        {

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