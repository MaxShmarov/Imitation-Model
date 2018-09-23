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

    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private CellUI _cellUI;
        [SerializeField]
        private Outline _outline;

        private Vector3 _defaultCellSize = new Vector3(1f, 1f, 1f);
        private Environment _environment;
        private CurrentWeather _currentWeather;
        private int _currentGrass = 0;
        private Transform _transform;

        public Vector2Int _positionInMatrix { get; private set; }

        private void Awake()
        {
            _currentWeather = new CurrentWeather();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateCellVariables);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateCellVariables);
        }

        public void Init(Vector3 position, string cellName, Vector2Int index)
        {
            _transform = transform;
            _transform.localScale = _defaultCellSize;
            _transform.position = position;
            name = cellName;
            _environment = Database.Instance.GetRandomEnvironment();
            _positionInMatrix = index;
            EnvironmentFromData();
            InitUIVariables();
            _outline.enabled = false;
        }

        private void EnvironmentFromData()
        {
            if (_environment == null || _renderer == null)
                return;
            _renderer.material.color = _environment.Color;
        }

        private void UpdateCellVariables()
        {
            bool waterBeside = CheckNeighbours();
            _currentWeather = Database.Instance.Weather.GetRandomWeather();
            _currentGrass = Database.Instance.Grass.UpdateGrass(_environment.Type, _currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, waterBeside);
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass);
        }

        private bool CheckNeighbours()
        {
            int x = _positionInMatrix.x;
            int y = _positionInMatrix.y;
            if (CheckCell(x + 1, y))
                return true;
            if (CheckCell(x, y + 1))
                return true;
            if (CheckCell(x - 1, y))
                return true;
            if (CheckCell(x, y - 1))
                return true;
            if (CheckCell(x + 1, y + 1))
                return true;
            if (CheckCell(x + 1, y - 1))
                return true;
            if (CheckCell(x - 1, y + 1))
                return true;
            if (CheckCell(x - 1, y - 1))
                return true;
            return false;
        }

        private bool CheckCell(int x, int y)
        {
            var cell = GameController.Instance.GetCellByPosition(x, y);
            if (cell != null && cell._environment.Type == EnvironmentType.Lake)
                return true;
            return false;
        }

        private void InitUIVariables()
        {
            _currentWeather = Database.Instance.Weather.GetRandomWeather();
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass);
        }

        public void Select(bool selected)
        {
            _outline.enabled = selected;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager<Cell>.Trigger("Select", this);     
        }
    }
}