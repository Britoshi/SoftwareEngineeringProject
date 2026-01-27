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

            Camera cam = Camera.main;
            if (!cam) throw new System.Exception("No camera found");

            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x + Input.GetAxis("Vertical"),
                cam.transform.eulerAngles.y - Input.GetAxis("Horizontal"), 0);
            HandleDrawOnBoard();
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
        
        private void HandleDrawOnBoard()
        {
            if (!showBoard) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                GameObject line = Instantiate(LinePrefab, currentPiece.transform);
                lineRenderers.Add(line.GetComponent<LineRenderer>());
                drawPoints.Add(new List<Vector3>());
            }

            if (Input.GetMouseButton(0))
            {
                var currLine = lineRenderers[^1];
                var currPoint = drawPoints[^1];
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    currPoint.Add(hit.point);
                    currLine.positionCount = currPoint.Count;
                    currLine.SetPositions(currPoint.ToArray());
                }
            }
        }
    }
}