using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quatro.Core
{
    public class DrawingBoard : BoardBehavior
    {
        [SerializeField] private Piece PiecePrefab;
        [SerializeField] private LineRenderer LineRendererPrefab;

        private new Camera camera;
        private bool mouseDragging;
        private bool isPressedThisFrame;

        [SerializeField] private List<LineRenderer> LineRenderers;

        private List<List<Vector3>> inputLines;
        private List<Vector3> currentLine;

        private LineRenderer currentLineRenderer;

        void Start()
        {
            inputLines = new List<List<Vector3>>();
            LineRenderers = new List<LineRenderer>();
            camera = Camera.main;
            if (camera == null) Debug.LogError("No camera found in scene");
        }

        void ResetBoard()
        {
            foreach (var lineRenderer in LineRenderers)
                Destroy(lineRenderer.gameObject);
            LineRenderers.Clear();
            inputLines.Clear();
            isPressedThisFrame = false;
            mouseDragging = false;
        }

        void RotateBoard(float angle)
        {
            Quaternion rotation = Quaternion.Euler(angle, 0f, 0f);

            for (int i = 0; i < inputLines.Count; i++)
            {
                List<Vector3> set = inputLines[i];
                RotateSet(set);

                LineRenderers[i].positionCount = set.Count;
                LineRenderers[i].SetPositions(set.ToArray());
                LineRenderers[i].Simplify(.001f);
            }

            void RotateSet(List<Vector3> set)
            {
                for (int i = 0; i < set.Count; i++)
                {
                    set[i] = rotation * set[i];
                }
            }
        }

        void ConfirmDrawing()
        {
            RotateBoard(90);
            Piece piece = PiecePrefab.Create();
            foreach(LineRenderer lineRenderer in LineRenderers)
            {
                lineRenderer.transform.SetParent(piece.transform, false);
                lineRenderer.transform.localPosition = Vector3.zero;
                lineRenderer.transform.localRotation = Quaternion.identity;
            }

            LineRenderers.Clear();
            ResetBoard();

            Board.CurrentPlacingPiece = piece;
            Board.ToNextPlacingPhase();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ResetBoard();
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfirmDrawing();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                mouseDragging = true;
                isPressedThisFrame = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                mouseDragging = false;
                isPressedThisFrame = false;
            }

            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, 100, 1 << LayerMask.NameToLayer("Drawing Board")))
            {
                return;
            }
            
            if (isPressedThisFrame)
            { 
                currentLine = new List<Vector3>();
                inputLines.Add(currentLine);


                GameObject lineRendererObject = Instantiate(LineRendererPrefab.gameObject, transform);
                currentLineRenderer = lineRendererObject.GetComponent<LineRenderer>();
                LineRenderers.Add(currentLineRenderer);
            }

            if (mouseDragging)
            { 
                currentLine.Add(-transform.position + hit.point);

                currentLineRenderer.positionCount = currentLine.Count;
                currentLineRenderer.SetPositions(currentLine.ToArray());
                currentLineRenderer.Simplify(.001f);

                if (isPressedThisFrame)
                    isPressedThisFrame = false;
            }
        }
    }
}