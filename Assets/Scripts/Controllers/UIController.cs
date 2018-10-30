using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LittleWorld.UI;
using LittleWorld.Common;

namespace LittleWorld.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _background;
        [SerializeField]
        private RectTransform _inputPanel;
        [SerializeField]
        private ConventionsItem _conventionPrefab;
        [SerializeField]
        private ScrollRect _conventions;
        [SerializeField]
        private InputField _inputX;
        [SerializeField]
        private InputField _inputZ;
        [SerializeField]
        private Button _nextStepButton;
        [SerializeField]
        private Button _backStepButton;
        [SerializeField]
        private Button _hideCanvasButton;
        [SerializeField]
        private Text _hideButtonText;
        [SerializeField]
        private Image _currentCoord;
        [SerializeField]
        private Text _cellCord;
        [SerializeField]
        private GameObject _gameUI;

        private GameController _gameController;

        [HideInInspector]
        public bool _canvasShow = true;

        private void Start()
        {
            _inputX.contentType = InputField.ContentType.IntegerNumber;
            _inputZ.contentType = InputField.ContentType.IntegerNumber;            
        }

        public void ActiveRabbit()
        {
            EventManager.Trigger("AddRabbits");
        }

        public void ClearCoord()
        {
            _cellCord.text = string.Empty;
        }

        public void SetCoordValue(int x, int z)
        {
            _cellCord.text = string.Format("X= {0} ; Y = {1}", x, z);
        }

        public void InitUI()
        {
            _gameController = GameController.Instance;
            var worldData = GameController.Instance.GetWorldData();
            ClearScreen();
            var _environments = worldData.GetEnvironments();
            var _weathers = worldData.GetWeathers();
            var _grass = worldData.GetGrass();

            foreach (var environment in _environments)
            {
                var convention = Instantiate(_conventionPrefab, _conventions.content);
                convention.Init(environment.Color, environment.Name);
            }

            foreach (var condition in _weathers)
            {
                var convention = Instantiate(_conventionPrefab, _conventions.content);
                convention.Init(condition.Icon, condition.Name);
            }

            if (_grass != null)
            {
                var convention = Instantiate(_conventionPrefab, _conventions.content);
                convention.Init(_grass.Icon, _grass.Name);
            }

            ShowStartScreen(true);
        }

        public void GenerateWorldHandler()
        {
            if (_inputX.text == string.Empty && _inputZ.text == string.Empty)
                return;

            Config.SizeX = int.Parse(_inputX.text);
            Config.SizeY = int.Parse(_inputZ.text);

            if (Config.SizeX < Config.MinCellCount || Config.SizeX > Config.MaxCellCount)
            {
                _inputX.text = string.Empty;
                return;
            }
                
            if (Config.SizeY < Config.MinCellCount || Config.SizeY > Config.MaxCellCount)
            {
                _inputZ.text = string.Empty;
                return;
            }

            GameController.Instance.GenerateWorld(Config.SizeX, Config.SizeY);
            ShowStartScreen(false);
            ClearScreen();
        }

        public void BackButtonClickHandler()
        {
            _gameController.ResetWorld();
            InitUI();
        }

        private void ClearScreen()
        {
            while (_conventions.content.childCount > 0)
            {
                Transform child = _conventions.content.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
            _inputX.text = string.Empty;
            _inputZ.text = string.Empty;
        }

        private void ShowStartScreen(bool enabled)
        {
            _background.gameObject.SetActive(enabled);
            _inputPanel.gameObject.SetActive(enabled);
            _gameUI.SetActive(!enabled);
        }

        public void NextStepClickHandler()
        {
            if (_gameController == null)
                return;
            _gameController.NextStep();
        }

        public void HideCanvasClickhandler()
        {
            _canvasShow = !_canvasShow;
            if (_canvasShow)
            {
                _hideButtonText.text = "Hide all canvases";
            }
            else
            {
                _hideButtonText.text = "Show all canvases";
            }
            EventManager<bool>.Trigger("CanvasShow", _canvasShow);
        }
    }
}
