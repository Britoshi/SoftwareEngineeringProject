using Quarto;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X, Y;
    public Piece Piece;
    public bool IsOccupied => Piece != null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile GetClosestTileToMouse (){
    //Make new variables for closest Tile and Distance
    Tile closestTile = null;
    float closestDistance = float.MaxValue;

    //Loop through 2D array
    for (int row = 0; row < 4; row++){
        for (int col = 0; col < 4; col++){
            Tile currentTile = Board.grid[row][col];
            //Calculate distance of mouse Position to current Tile
            Vector3 diff = mouseWorldPosition - currentTile.transform.position;
            float distance = diff.sqrMagnitude;

            //Update variables
            if (distance < closestDistance){
                closestTile = currentTile;
                closestDistance = distance;
            }
        }
    }
    return closestTile;

}
}
