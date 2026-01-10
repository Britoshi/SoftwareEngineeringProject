using UnityEngine;
using UnityEngine.Serialization;

namespace Quatro.Core
{
    public class Tile : BoardBehavior
    {
        private bool hoveringCache;
        private bool IsMouseHovering => Board.CurrentHovering == this;

        /// <summary>
        /// For Hovering Visual FX
        /// </summary>
        public Renderer HoverRenderer;
        /// <summary> X index of the board grid </summary>
        public int X;
        /// <summary> Y index of the board grid </summary>
        public int Y;
        /// <summary> Assigned Piece. Null means nothing assigned yet. </summary>
        private Piece piece;

        /// <summary>
        /// The center position of the grid represented in world coordinate
        /// </summary> 
        public void SetPiece(Piece setPiece) => piece = setPiece;

        /// <summary> Does it have a piece? </summary>
        public bool HasPiece => piece != null;

        /// <summary>
        /// Tile World Position in the 3D world
        /// </summary>
        private Vector3 originWorldPosition;
        /// <summary> World Position dedicated for Hovering Visual FX </summary>
        private Vector3 hoveringWorldPosition;
        
        //Visual FX
        private MaterialPropertyBlock block;
        private static readonly int SELECTED_ID = Shader.PropertyToID("_Selected");
        private static readonly int OPACITY_ID = Shader.PropertyToID("_Opacity");
        private float opacity;
        
        public Tile Create(int x, int y, Transform parent)
        {
            GameObject newInstance = Instantiate(gameObject, parent);
            Tile tile = newInstance.GetComponent<Tile>();
            tile.X = x;
            tile.Y = y;

            const float TILE_SIZE = Board.BOARD_SIZE / Board.DIMENSION;
            Vector2 normPosition = new Vector2(x / (float)(Board.DIMENSION - 1), y / (float)(Board.DIMENSION - 1))
                                   - Vector2.one / 2f; 
            Vector2 worldPosition = normPosition * TILE_SIZE;

            tile.originWorldPosition= new Vector3(worldPosition.x, 0f, worldPosition.y);
            tile.hoveringWorldPosition = tile.originWorldPosition + Vector3.up * .5f;
            newInstance.transform.position = Vector3.up * 10f;
            
            return tile;
        }

        void Start()
        { 
            block = new MaterialPropertyBlock();
            opacity = 0;
            SetOpacity();
        }
        
        void Update()
        {
            float transitionSpeed = 10f;
            if (IsMouseHovering)
            {
                if (!hoveringCache)
                {
                    //Started hovering this frame;
                    SetSelected(true);
                }
                if(!Approximately(transform.position, hoveringWorldPosition))
                    transform.position = Vector3.Lerp(transform.position, hoveringWorldPosition, Time.deltaTime *transitionSpeed);
                
                    
                    
                if (!Mathf.Approximately(opacity, 1f))
                {
                    SetOpacity();
                }
                
                opacity = Mathf.Lerp(opacity, 1f, Time.deltaTime * transitionSpeed);
            }
            else
            {
                if (hoveringCache)
                {
                    //Ended hovering this frame
                    SetSelected(false);

                }
                if(!Approximately(transform.position, originWorldPosition))
                    transform.position = Vector3.Lerp(transform.position, originWorldPosition, Time.deltaTime * transitionSpeed);
                
                if (!Mathf.Approximately(opacity, 0f))
                {
                    SetOpacity();
                }
                
                opacity = Mathf.Lerp(opacity, 0f, Time.deltaTime * transitionSpeed);
            }
            hoveringCache = IsMouseHovering;
        }

        private void SetSelected(bool selected)
        {
            HoverRenderer.GetPropertyBlock(block);
            block.SetFloat(SELECTED_ID, selected ? 1.0f : 0.0f);
            HoverRenderer.SetPropertyBlock(block);
        }

        private void SetOpacity()
        {
            HoverRenderer.GetPropertyBlock(block);
            block.SetFloat(OPACITY_ID, opacity);
            HoverRenderer.SetPropertyBlock(block);
        }
        
        private bool Approximately(Vector3 a, Vector3 b) => Vector3.SqrMagnitude(a - b) < 0.001f; 
    }
}