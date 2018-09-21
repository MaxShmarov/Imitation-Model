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
        private GameController _gameController;
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

        private int _sizeX;
        private int _sizeZ;
        private Database _database;

        private void Start()
        {
            _database = Database.Instance;
            _inputX.contentType = InputField.ContentType.IntegerNumber;
            _inputZ.contentType = InputField.ContentType.IntegerNumber;
        }

        public void InitUI()
        {
            ClearScreen();
            var _environments = _database.GetEnvironmentsData();

            foreach (var environment in _environments)
            {
                var convention = Instantiate(_conventionPrefab, _conventions.content);
                convention.Init(environment.Color, environment.Name);
            }           
            ShowStartScreen(true);
        }

        public void GenerateWorldHandler()
        {
            if (_inputX.text == string.Empty && _inputZ.text == string.Empty)
                return;

            _sizeX = int.Parse(_inputX.text);
            _sizeZ = int.Parse(_inputZ.text);

            if (_sizeX < Config.MinCellCount || _sizeX > Config.MaxCellCount)
            {
                _inputX.text = string.Empty;
                return;
            }
                
            if (_sizeZ < Config.MinCellCount || _sizeZ > Config.MaxCellCount)
            {
                _inputZ.text = string.Empty;
                return;
            }

            _gameController.GenerateWorld(_sizeX, _sizeZ);
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
            _nextStepButton.gameObject.SetActive(!enabled);
            _backStepButton.gameObject.SetActive(!enabled);
        }

        public void NextStepClickHandler()
        {
            if (_gameController == null)
                return;
            _gameController.NextStep();
        }
    }
}
