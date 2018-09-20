using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        private InputField _inputX;
        [SerializeField]
        private InputField _inputZ;
        [SerializeField]
        private Button _nextStepButton;

        private int _sizeX;
        private int _sizeZ;


        private void Start()
        {
            _inputX.contentType = InputField.ContentType.IntegerNumber;
            _inputZ.contentType = InputField.ContentType.IntegerNumber;
            ShowStartScreen(true);           
        }

        public void GenerateWorldHandler()
        {
            if (_inputX.text == string.Empty && _inputZ.text == string.Empty)
                return;

            _sizeX = int.Parse(_inputX.text);
            _sizeZ = int.Parse(_inputZ.text);

            if (_sizeX <= 0 && _sizeZ <= 0)
                return;
            else
            {
                _gameController.GenerateWorld(_sizeX, _sizeZ);
            }
            ShowStartScreen(false);
        }

        private void ShowStartScreen(bool enabled)
        {
            _background.gameObject.SetActive(enabled);
            _inputPanel.gameObject.SetActive(enabled);
            _nextStepButton.gameObject.SetActive(!enabled);
        }

        public void NextStepClickHandler()
        {
            if (_gameController == null)
                return;
            _gameController.NextStep();
        }
    }
}
