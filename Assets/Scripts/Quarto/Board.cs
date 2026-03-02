using System.Collections.Generic;
using UnityEngine;

namespace Quarto
{
    /// <summary>
    /// This is a singleton instance that has the board 
    /// </summary>
    public class Board : MonoBehaviour
    {
        private GameObject drawBoard;
        
        private bool showBoard = false;
        private Transform gridInstanceHolder;
        public static Tile[][] Grid;

        private bool pieceInHand = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Grid = new Tile[4][];

            drawBoard = GameObject.FindGameObjectWithTag("Draw Board");
            drawBoard.SetActive(false);
            
            gridInstanceHolder = new GameObject("Grid").transform;

            for (int y = 0; y < 4; y++)
            {
                Grid[y] = new Tile[4];

                for (int x = 0; x < 4; x++)
                {
                    Tile tile = new GameObject(x + ", " + y + " Tile").AddComponent<Tile>();
                    tile.transform.SetParent(gridInstanceHolder);
                    tile.gameObject.transform.position = new Vector3(x, 0, y);
                    GameObject visualModel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    visualModel.transform.SetParent(tile.transform);
                    visualModel.transform.localPosition = Vector3.zero;
                    visualModel.transform.localScale = new Vector3(1, .1f, 1);
                    tile.X = x;
                    tile.Y = y;
                    Grid[y][x] = tile;
                }
            }

            Camera cam = Camera.main;
            if (!cam) throw new System.Exception("No camera found");

            cam.orthographic = true;
            cam.orthographicSize = 4;
            cam.nearClipPlane = .01f;
            cam.transform.position = new Vector3(3f / 2f, 2, 3f / 2);
            cam.transform.rotation = Quaternion.Euler(90, 45, 0);

          

            ResetDrawBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Destroy(gridInstanceHolder.gameObject);
                Start();
            } 
            
            if(Input.GetKeyDown(KeyCode.Space))
            {
                showBoard = !showBoard;
                drawBoard.SetActive(showBoard);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ConfirmDrawing();
            }

          



            Camera cam = Camera.main;
            if (!cam) throw new System.Exception("No camera found");

            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x + Input.GetAxis("Vertical"),
                cam.transform.eulerAngles.y - Input.GetAxis("Horizontal"), 0);
            HandleDrawOnBoard();
            HandlePieceInHand();

        }

        private List<List<Vector3>> drawPoints;
        public GameObject LinePrefab;
        
        private List<LineRenderer> lineRenderers;
        private Piece currentPiece;
        
        private void ResetDrawBoard()
        {
            drawPoints = new List<List<Vector3>>();
            lineRenderers = new List<LineRenderer>();
            
            currentPiece = new GameObject("Piece").AddComponent<Piece>();
            // currentPiece.transform.SetParent(gridInstanceHolder);
        }

        // This method is confirms the drawing by detaching the 
        // piece from the draw board, scaling it, and putting in player's hand
        private void ConfirmDrawing()
        {
            currentPiece.transform.SetParent(null);
            ScalePieceToFitTile();
            pieceInHand = true;
            currentPiece.transform.position = new Vector3(5, 1, 0);

            showBoard = false;
            drawBoard.SetActive(false);
        }

        // Scaling piece to fit tile by finding the bounding
        // box of the drawn points and scaling it down to fit within a 1x1 unit 
        private void ScalePieceToFitTile()
        {
            if (drawPoints.Count == 0) return; 

            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            foreach (List<Vector3> stroke in drawPoints)
            {
                foreach (Vector3 point in stroke)
                {
                    min = Vector3.Min(min, point);
                    max = Vector3.Max(max, point);
                }
            }

            float width = max.x - min.x;
            float height = max.z - min.z;
            float largestDimension = Mathf.Max(width, height);

            if (largestDimension == 0) return;

            float scale = 1f / largestDimension;
            currentPiece.transform.localScale = new Vector3(scale, scale, scale);
        }    

        // Handles the piece in hand by raycasting mouse position to move the piece and place 
        // it on a tile
        private void HandlePieceInHand()
        {
            if (!pieceInHand || currentPiece == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name + " at " + hit.point);
                currentPiece.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                if (Input.GetMouseButtonDown(0) && hit.collider.GetComponent<Tile>() != null)
                {
                    // Not reading the mouse click 
                    Debug.Log("Left mouse button is being held down");
                    Debug.Log("Clicked on tile: " + hit.collider.gameObject.name);
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (!tile.IsOccupied)
                    {
                        PlacePiece(tile);
                    }
                }
            }
        }

        public void PlacePiece(Tile tile)
        {
            if (currentPiece == null) return;

            currentPiece.transform.position = new Vector3(tile.X, 0.5f, tile.Y);
            tile.Piece = currentPiece;

            ResetDrawBoard();
        }
        
        private void HandleDrawOnBoard()
        {
            if (!showBoard) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                GameObject line = Instantiate(LinePrefab, currentPiece.transform);
                LineRenderer lr = line.GetComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lineRenderers.Add(lr);
                drawPoints.Add(new List<Vector3>());
            }

            if (Input.GetMouseButton(0))
            {
                var currLine = lineRenderers[^1];
                var currPoint = drawPoints[^1];
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    currPoint.Add(currentPiece.transform.InverseTransformPoint(hit.point));
                    currLine.positionCount = currPoint.Count;
                    currLine.SetPositions(currPoint.ToArray());
                }
            }
        }
    }
}