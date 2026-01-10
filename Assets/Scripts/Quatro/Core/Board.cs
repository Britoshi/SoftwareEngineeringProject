using Core;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quatro.Core
{
    public enum Phase : byte
    {
        Player2Draw,
        Player1Place,
        PlaceTransition1,
        Player1Draw,
        Player2Place,
        PlaceTransition2,
    }

    public class Board : Singleton<Board>
    {
        internal const float BOARD_SIZE = 16f;
        internal const int DIMENSION = 4;

        private static Tile[][] grid;
        public static Phase Phase;

        private const float PIECE_HOVER_HEIGHT = 3f;

        [SerializeField] private Tile TilePrefab;
        [SerializeField] private Piece PiecePrefab;

        [SerializeField] private Animator CameraAnimator;
        [SerializeField] private Transform TileHolder;
        private new Camera camera;

        internal static Tile CurrentHovering;
        internal static Piece CurrentPlacingPiece;

        [SerializeField] private Button ToggleBoardButton;
        [SerializeField] private Button ConfirmButton;

        private Tile confirmTile;
        private Tile lastHovered;

        public static bool IsDrawingPhase => Phase is Phase.Player2Draw or Phase.Player1Draw;

        public static void ToNextPlacingPhase()
        {
            if (Phase == Phase.Player1Place || Phase == Phase.Player2Place)
            {
                throw new Exception("Going into placing phase when already in placing phase?");
            }

            Instance.SetPhase((byte)Phase + 1);
        }

        private void SetPhase(int value)
        {
            if (value > (int)Phase.PlaceTransition2) value = 0;
            Phase = (Phase)value;

            switch (Phase)
            {
                case Phase.Player1Place:
                case Phase.Player2Place:
                    if (CurrentPlacingPiece == null) throw new Exception("CurrentPlacingPiece == null");

                    //Edge Case. Leave this be
                    isDrawingBoardOpen = false;
                    Instance.CameraAnimator.Play("Board");

                    ToggleBoardButton.gameObject.SetActive(false);
                    ConfirmButton.gameObject.SetActive(false);
                    break;
                case Phase.Player1Draw:
                case Phase.Player2Draw:
                    ToggleBoardButton.gameObject.SetActive(true);
                    ConfirmButton.gameObject.SetActive(true);
                    break;

                case Phase.PlaceTransition1:
                case Phase.PlaceTransition2:
                    Vector3 pos = confirmTile.transform.position;
                    pos.y = PIECE_HOVER_HEIGHT;
                    CurrentPlacingPiece.transform.position = pos;

                    ToggleBoardButton.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void ToNextPhase()
        {
            SetPhase((byte)Phase + 1);
        }

        private void Start()
        {
            camera = Camera.main;
            StartGame();
        }

        protected override void Awake()
        {
            base.Awake();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            grid = new Tile[DIMENSION][];

            for (int y = 0; y < DIMENSION; y++)
            {
                grid[y] = new Tile[DIMENSION];

                for (int x = 0; x < DIMENSION; x++)
                {
                    grid[y][x] = TilePrefab.Create(x, y, TileHolder);
                }
            }
        }

        private bool hasGameStarted = false;

        private void StartGame()
        {
            if (hasGameStarted) return;

            hasGameStarted = true;
            SetPhase(0);
        }

        bool isDrawingBoardOpen = false;

        public void ToggleDrawingBoard()
        {
            if (!IsDrawingPhase) return;

            isDrawingBoardOpen = !isDrawingBoardOpen;
            string animName = isDrawingBoardOpen ? "Drawing" : "Board";
            CameraAnimator.Play(animName);
        }

        private void Update()
        {
            if (!hasGameStarted)
            {
                return;
            }

            UIController.SetDebugText(Phase.ToString());


            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleDrawingBoard();
            }

            switch (Phase)
            {
                case Phase.Player1Place:
                case Phase.Player2Place:
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);


                    if (CurrentPlacingPiece != null && lastHovered != null)
                    {
                        Vector3 hoverPosition = lastHovered.transform.position;
                        hoverPosition.y = 3;
                        CurrentPlacingPiece.transform.position = Vector3.Lerp(CurrentPlacingPiece.transform.position,
                            hoverPosition,
                            Time.deltaTime * 10f
                        );
                    }

                    if (Physics.Raycast(ray, out RaycastHit hit, 1 << LayerMask.NameToLayer("Tile")))
                    {
                        Tile tile = hit.transform.GetComponentInParent<Tile>();


                        if (!tile) break;


                        if (tile.HasPiece) break;

                        lastHovered = tile;
                        CurrentHovering = tile;

                        if (Input.GetMouseButtonDown(0))
                        {
                            confirmTile = lastHovered;
                            ToNextPhase();
                        }
                    }
                    else
                    {
                        CurrentHovering = null;
                    }

                    break;

                case Phase.Player2Draw:
                case Phase.Player1Draw:
                    break;

                case Phase.PlaceTransition1:
                case Phase.PlaceTransition2:

                    CurrentPlacingPiece.transform.position = Vector3.Lerp(CurrentPlacingPiece.transform.position,
                        confirmTile.transform.position,
                        Time.deltaTime * 10f);

                    if (Vector3.Distance(CurrentPlacingPiece.transform.position, confirmTile.transform.position) <
                        0.01f)
                    {
                        CurrentPlacingPiece.transform.position = confirmTile.transform.position;
                        CurrentPlacingPiece.Place(confirmTile);
                        CurrentPlacingPiece = null;
                        confirmTile = null;
                        CurrentHovering = null;
                        ToNextPhase();
                    }

                    break;
            }
        }

        public Tile this[int x, int y] => grid[y][x];
    }
}