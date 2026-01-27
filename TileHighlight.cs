using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    public Camera cam;

    public LayerMask tileLayer;
    
    private Title currentTitle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Tile hovered = GetTileUnderMouse();
            
            
            //Code to stop the tile from flickering when not hovering over it
            
            //When selecting a tile(not hovering over it anymore) - unhighlight it
            if (hovered != currentTitle)
            {
                Transform h = currentTitle.transform.Find("Highlight");
                if (h != null)
                {
                    h.gameObject.SetActive(false);
                }
            }
            
            
            //Set the current to the new closest tile(the one were hovering)
            currentTitle = hovered;
            
            //Now highlight the new current closest tile
            if (currentTitle != null)
            {
                Transform h = currentTitle.transform.Find("Highlight");
                if (h != null)
                {
                    h.gameObject.SetActive(true);
                }
            }

        }


        Tile GetTileUnderMouse()
        {
            //Getting the position of the mouse
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            
            //Use Raycast to highlight the tile
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, tileLayer))
                return hit.collider.GetComponent<Tile>();

            return null;
        }
    }
}



