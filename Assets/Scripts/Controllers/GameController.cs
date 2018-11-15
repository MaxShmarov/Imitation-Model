﻿using LittleWorld.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Helpers;
using LittleWorld.Data;
using Extensions;

namespace LittleWorld.Controllers
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField]
        private WorldData _worldData;
        [SerializeField]
        private UIController _uiController;
        [SerializeField]
        private Cell _cellPrefab;
        [SerializeField]
        private Transform _parentForCell;

        public Camera _uiCamera;

        private const float _cellOffset = 1.1f;
        private Cell[,] _matrix;
        private Vector2Int _matrixSize;
        private Cell _currentCell;

        private List<Environment> environments;
        private int _countField = 0;
        private int _countMountain = 0;
        private int _countLake = 0;
        private List<int> env = new List<int>();

        private void Start()
        {
            _uiController.InitUI();
            env.Add(0);
            env.Add(1);
            env.Add(2);
            environments = _worldData.GetEnvironments();
        }

        public WorldData GetWorldData()
        {
            return _worldData;
        }

        public void UpdateCoordsCell(int x, int z, int sun, int rain, int grass, int rabbits, int wolves, int hunters)
        {
            _uiController.SetStatsPanel(x, z, sun, rain, grass, rabbits, wolves, hunters);
        }

        public void ClearStats()
        {
            _uiController.ClearStats();
        }

        public void GenerateWorld(int x, int z)
        {
            _matrixSize = new Vector2Int(x, z);
            _matrix = new Cell[_matrixSize.x, _matrixSize.y];
            CameraHandler.Instance.SetLimitsX(new Vector2Int(-2, z + 2));
            CameraHandler.Instance.SetLimitsZ(new Vector2Int(-3, z + 3));
            StartCoroutine("GenerateGrid");
        }

        private IEnumerator GenerateGrid()
        {                      
            for (int i = 0; i < _matrixSize.x; i++)
            {
                for (int j = 0; j < _matrixSize.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);

                    var cell = Instantiate(_cellPrefab, _parentForCell);

                    Vector3 cellPos = new Vector3(i * _cellOffset, 0, j * _cellOffset);
                    string cellName = string.Format("Cell [{0}][{1}]", i, j);
                    Environment environment;
                    if (Config.FieldCount != null && Config.MountainCount != null &&
                       Config.LakeCount != null)
                    {
                        if (Config.FieldCount == 0)
                        {
                            env.Remove(0);
                        }
                        if (Config.MountainCount == 0)
                        {
                            env.Remove(1);
                        }
                        if (Config.LakeCount == 0)
                        {
                            env.Remove(2);
                        }
                        environment = PseudorandomEnvironment();
                    }
                    else
                    {
                        environment = _worldData.GetRandomEnvironment();
                    }
                    cell.Init(cellPos, cellName, environment, index);

                    _matrix[index.x, index.y] = cell;
                }
            }
            yield return null;
        }

        private Environment PseudorandomEnvironment()
        {
            int random = 0;
            random = env.Count > 0 ? env.RandomItem() : Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    _countField++;
                    break;
                case 1:
                    _countMountain++;
                    break;
                case 2:
                    _countLake++;
                    break;
            }
            if (_countField == Config.FieldCount)
            {
                env.Remove(0);
            }
            if (_countMountain == Config.MountainCount)
            {
                env.Remove(1);
            }
            if (_countLake == Config.LakeCount)
            {
                env.Remove(2);
            }
            return environments[random];
        }

        public void ResetWorld()
        {
            for (int i = 0; i < _matrixSize.x; i++)
            {
                for (int j = 0; j < _matrixSize.y; j++)
                {
                    Destroy(_matrix[i, j].gameObject);
                }
            }
        }

        public void NextStep(int stepCount)
        {
            while (stepCount > 0)
            {
                EventManager.Trigger(Config.NextStep);
                stepCount--;
            }           
        }

        public Cell GetCellByPosition(int x, int y)
        {
            if (x < _matrixSize.x && x >= 0
                && y < _matrixSize.y && y >= 0)
                return _matrix[x, y];
            else
                return null;
        }
    }
}