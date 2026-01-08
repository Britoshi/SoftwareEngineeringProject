using Core;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quatro.Core
{
    public enum Phase
    {
        Player2Draw, Player1Place,
        Player1Draw, Player2Place
    }
    public class Board : Singleton<Board>
    {
        [SerializeField] private Tile TilePrefab;
        [SerializeField] private Piece PiecePrefab;
        
        [SerializeField] private Animator CameraAnimator;
        [SerializeField] private Transform TileHolder;
        private new Camera camera;

        internal const float BOARD_SIZE = 16f;
        internal const int DIMENSION = 4; 
        internal static Tile CurrentHovering;

        private static Tile[][] grid;
        private void Start()
        {
            camera = Camera.main;
        }

        protected override void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        { 
            grid = new Tile[DIMENSION][];
            for(int y = 0; y < DIMENSION; y++)
            {
                grid[y] = new Tile[DIMENSION];

                for (int x = 0; x < DIMENSION; x++)
                {
                    grid[y][x] = TilePrefab.Create(x, y, TileHolder);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CameraAnimator.Play("Drawing");
            }
            
            if (camera != null)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
                if (Physics.Raycast(ray, out RaycastHit hit, 1 << LayerMask.NameToLayer("Tile")))
                {
                    Tile tile = hit.transform.GetComponentInParent<Tile>();
                    CurrentHovering = tile;
                }
                else
                {
                    CurrentHovering = null;
                }
            }
        }

        public Tile this[int x, int y] => grid[y][x];
        
        
    }
}