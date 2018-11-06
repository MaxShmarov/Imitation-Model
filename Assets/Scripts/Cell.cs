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
        private int _wolfCount = 0;
        private int _hunterCount = 0;
        private Transform _transform;
        private bool _waterBeside = false;
        private bool _knowNeighbours = false;
        private bool _showCanvas = true;
        private bool _cellFound = false;
        private bool _wolfKilled = false;
        private bool _hunterKilled = false;

        public Vector2Int _positionInMatrix { get; private set; }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StartListening("CanvasShow", ShowCanvas);
            EventManager.StartListening("AddRabbits", AddRabbitsClickHandler);
            EventManager.StartListening("AddWolfs", AddWolfsClickHandler);
            EventManager.StartListening("AddHunters", AddHuntersClickHandler);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateCellVariables);
            EventManager<bool>.StopListening("CanvasShow", ShowCanvas);
            EventManager.StopListening("AddRabbits", AddRabbitsClickHandler);
            EventManager.StopListening("AddWolfs", AddWolfsClickHandler);
            EventManager.StopListening("AddHunters", AddHuntersClickHandler);
        }

        private void AddRabbitsClickHandler()
        {
            _rabbitCount = GetRandomCount();
            _cellUI.UpdateUI(-1, -1, _currentGrass, _rabbitCount, _wolfCount, _hunterCount);
        }

        private void AddWolfsClickHandler()
        {
            _wolfCount = GetRandomCount();
            _cellUI.UpdateUI(-1, -1, _currentGrass, _rabbitCount, _wolfCount, _hunterCount);
        }

        private void AddHuntersClickHandler()
        {
            _hunterCount = GetRandomCount();
            _cellUI.UpdateUI(-1, -1, _currentGrass, _rabbitCount, _wolfCount, _hunterCount);
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
            CheckWolfsHunters();
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _rabbitCount, _wolfCount, _hunterCount);
        }

        private void UpdateRabbits()
        {
            if (_rabbitCount != 0)
            {
                if (_rabbitCount == 2)
                {
                    _rabbitCount = Config.AddThing(_rabbitCount);
                }
                while (_currentGrass < _rabbitCount)
                {
                    CheckNeighbours();
                }
            }
            if (_hunterCount > 0 && _wolfCount == 0)
            {
                _rabbitCount -= _hunterCount;
            }
            if (_wolfCount > 0 && _hunterCount == 0)
            {
                _rabbitCount -= _wolfCount;
            }
            if (_rabbitCount < 0)
            {
                _rabbitCount = 0;
            }
        }

        private void CheckWolfsHunters()
        {
            _wolfKilled = false;
            _hunterKilled = false;
            if (_wolfCount == _hunterCount && _wolfCount != 0 && _hunterCount != 0)
            {
                _wolfCount = RunFromCell(_wolfCount, true);
                _hunterCount = RunFromCell(_hunterCount, false);
            }
            else if (_wolfCount < _hunterCount && _wolfCount != 0)
            {
                _wolfCount = Config.RemoveThing(_wolfCount);
                _wolfKilled = true;
                _wolfCount = RunFromCell(_wolfCount, true);
            }
            else if (_wolfCount > _hunterCount && _hunterCount != 0)
            {
                _hunterCount = Config.RemoveThing(_hunterCount);
                _hunterKilled = true;
                _hunterCount = RunFromCell(_hunterCount, false);
            }
            if (!_wolfKilled && _hunterCount > 0)
            {
                if (_wolfCount == 0 && _rabbitCount == 0)
                {
                    _hunterCount = RunFromCell(_hunterCount, false);
                }
            }
            if (!_hunterKilled && _wolfCount > 0)
            {
                if (_hunterCount == 0 && _rabbitCount == 0)
                {
                    _wolfCount = RunFromCell(_wolfCount, true);
                }
            }
        }

        private int RunFromCell(int count, bool isWolf)
        {
            int x = _positionInMatrix.x;
            int y = _positionInMatrix.y;

            for (int i = count; i > 0; i--)
            {
                if (CheckCellForOtherThings(ref count, x + 1, y, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x + 1, y, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x, y + 1, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x - 1, y, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x, y - 1, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x + 1, y + 1, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x + 1, y - 1, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x - 1, y + 1, isWolf))
                {
                    continue;
                }
                else if (CheckCellForOtherThings(ref count, x - 1, y - 1, isWolf))
                {
                    continue;
                }
                Debug.Log("No way out!: X-" + x + "Y-" + y);
                break;
            }
            return count;

        }

        private bool CheckCellForOtherThings(ref int count, int x, int y, bool isWolf)
        {
            var cell = GameController.Instance.GetCellByPosition(x, y);
            if (cell != null)
            {
                if (cell._environment.Type == EnvironmentType.Field)
                {
                    if (isWolf)
                    {
                        if (cell._wolfCount < 3)
                        {
                            cell._wolfCount = Config.AddThing(cell._wolfCount);
                            count = Config.RemoveThing(count);
                            cell._cellUI.UpdateUI(-1, -1, cell._currentGrass, cell._rabbitCount, cell._wolfCount, cell._hunterCount);
                            return true;
                        }
                    }
                    else
                    {
                        if (cell._hunterCount < 3)
                        {
                            cell._hunterCount = Config.AddThing(cell._hunterCount);
                            count = Config.RemoveThing(count);
                            cell._cellUI.UpdateUI(-1, -1, cell._currentGrass, cell._rabbitCount, cell._wolfCount, cell._hunterCount);
                            return true;
                        }
                    }
                }
            }
            return false;
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
                            _rabbitCount = Config.RemoveThing(_rabbitCount);
                            cell._rabbitCount = Config.AddThing(cell._rabbitCount);
                            cell._cellUI.UpdateUI(-1, -1, cell._currentGrass, cell._rabbitCount, cell._wolfCount, cell._hunterCount);
                            _cellFound = true;
                        }
                        break;
                }
      
            }
            if (lastTry && !_cellFound)
            {
                _rabbitCount = Config.RemoveThing(_rabbitCount);
            }
        }

        private int GetRandomCount()
        {
            int rand = 0;
            if (_environment.Type == EnvironmentType.Field)
            {
                rand = UnityEngine.Random.Range(1, 4);
                //_rabbitCount = rand;
            }
            return rand;
        }

        private void InitUIVariables()
        {
            var _currentWeather = Config.GetRandomWeather();
            _cellUI.UpdateUI(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity, _currentGrass, _rabbitCount, _wolfCount, _hunterCount);
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