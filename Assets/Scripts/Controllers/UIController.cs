﻿using System;
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
        private GameObject _gameUI;
        [SerializeField]
        private Text _cellCord;
        [SerializeField]
        private Text _cellWeather;
        [SerializeField]
        private Text _cellGrass;
        [SerializeField]
        private Text _cellRabbits;
        [SerializeField]
        private Text _cellWolves;
        [SerializeField]
        private Text _cellHunters;
        [SerializeField]
        private InputField _inputField;
        [SerializeField]
        private InputField _inputMountain;
        [SerializeField]
        private InputField _inputLake;
        [SerializeField]
        private InputField _inputStepCount;
        [SerializeField]
        private Text _allRabbits;
        [SerializeField]
        private Text _allWolves;
        [SerializeField]
        private Text _allHunters;

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
            SetAllStatsPanel();
        }

        public void ActiveWolfs()
        {
            EventManager.Trigger("AddWolfs");
            SetAllStatsPanel();
        }

        public void ActiveHunters()
        {
            EventManager.Trigger("AddHunters");
            SetAllStatsPanel();
        }

        public void ClearStats()
        {
            _cellCord.text = string.Empty;
            _cellWeather.text = string.Empty;
            _cellGrass.text = string.Empty;
            _cellRabbits.text = string.Empty;
            _cellWolves.text = string.Empty;
            _cellHunters.text = string.Empty;
        }

        public void SetAllStatsPanel()
        {
            _allRabbits.text = string.Format("Rabbits:" + Config.AllRabbits);
            _allWolves.text = string.Format("Wolves:" + Config.AllWolves);
            _allHunters.text = string.Format("Hunters:" + Config.AllHunters);
        }

        public void SetStatsPanel(int x, int z, int sun, int rain, int grass, int rabbits, int wolves, int hunters)
        {
            _cellCord.text = string.Format("X:{0} Y:{1}", x, z);
            _cellWeather.text = string.Format("Sun:{0} Rain:{1}", sun, rain);
            _cellGrass.text = string.Format("Grass:" + grass);
            _cellRabbits.text = string.Format("Rabbits:" + rabbits);
            _cellWolves.text = string.Format("Wolves:" + wolves);
            _cellHunters.text = string.Format("Hunters:" + hunters);
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

            if (_inputField.text != string.Empty && _inputMountain.text != string.Empty
                && _inputLake.text != string.Empty)
            {
                Config.CalculateEnvironment(int.Parse(_inputField.text), int.Parse(_inputMountain.text), int.Parse(_inputLake.text));
            }


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
            if (_inputStepCount.text == string.Empty)
            {
                _gameController.NextStep(1);
            }
            else
            {
                int stepsCount = int.Parse(_inputStepCount.text);
                if (stepsCount < 1)
                {
                    _gameController.NextStep(1);
                }
                else
                {
                    _gameController.NextStep(stepsCount);
                }
            }
            SetAllStatsPanel();
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
