using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LittleWorld.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameController _gameController;

        public void NextStepClickHandler()
        {
            if (_gameController == null)
                return;
            _gameController.NextStep();
        }
    }
}
