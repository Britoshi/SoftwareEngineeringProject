using UnityEngine;

namespace Quarto
{
    public class HighlightTile : MonoBehaviour
    {
        RaycastHit hits;
        
        private Renderer currentRenderer;
        private Material originalMaterial;
        
        private Board board;
        
        public Material highlightMaterial;

        void ClearHighlightedTile()
        {
            if (currentRenderer != null)
            {
                currentRenderer.sharedMaterial = originalMaterial;
                currentRenderer = null;
            }
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            board = FindAnyObjectByType<Board>();
        }

        // Update is called once per frame
        void Update()
        {
            if (board == null) return;

            if (board.IsDrawingOrPlacing())
            {
                ClearHighlightedTile();
                return;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hits))
            {
                Renderer hitRenderer = hits.collider.GetComponent<Renderer>();
                if (hitRenderer == null)
                {
                    return;
                }

                if (hitRenderer != currentRenderer)
                {
                    ClearHighlightedTile();
                    currentRenderer = hitRenderer;
                    originalMaterial = currentRenderer.sharedMaterial;
                    currentRenderer.sharedMaterial = highlightMaterial;
                }
                
                Debug.Log(hits.transform.name + "Was hit");
                Debug.Log(originalMaterial.name);
            }
            else
            {
                ClearHighlightedTile();
            }
        }
    }
}

