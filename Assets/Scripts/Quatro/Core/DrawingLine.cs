using UnityEngine;

namespace Quatro.Core
{
    public class DrawingLine : MonoBehaviour
    {
        
        public Renderer HoverRenderer;
        private MaterialPropertyBlock block;
        
        private static readonly int COLOR_ID = Shader.PropertyToID("_Color");
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        { 
            block = new MaterialPropertyBlock();

            if (Board.Phase == Phase.Player2Draw)
            {
                SetColor(Color.white);
            }
            else
            {
                SetColor(Color.black);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void SetColor(Color color)
        {
            HoverRenderer.GetPropertyBlock(block);
            block.SetColor(COLOR_ID, color);
            HoverRenderer.SetPropertyBlock(block);
        }

    }
}
