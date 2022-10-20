using System;
using System.Collections.Generic;
using UnityEngine;

    public class BoardHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer borderRenderer;
        
        private GameManager _gameManager;
        private List<List<int>> _board;
        private List<List<CellData>> _groups;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _gameManager.boardHandler = this;
            Initialize();
        }

        private void Start()
        {
            FillBoard();
            GroupTiles();
        }

        private void Initialize()
        {
            Debug.Log("Initializing Board");
            _gameManager.GameStart();
        }

        private void FillBoard()
        {
            Debug.Log("Filling Board");
        }

        private void GroupTiles()
        {
            Debug.Log("Grouping Tiles");
        }
    }