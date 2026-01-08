using System;
using UnityEngine;

namespace Quatro.Core
{
    public class Piece : BoardBehavior
    {
        // public Vector3 boardPosition;
        public bool Placed;
        public Tile AssignedTile;
        
        public Piece Create()
        { 
            GameObject newInstance = Instantiate(gameObject);
            Piece piece = newInstance.GetComponent<Piece>();    
            return piece;
        }

        public void Place(Tile tile)
        {
            tile.SetPiece(this);
            Placed = true;
            AssignedTile = tile;
        }

        private void Update()
        {
            
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(90f,0 , 0f);

            if (Placed)
            {
                var pos = transform.position;
                pos.y = AssignedTile.transform.position.y + Mathf.Sin(Time.time)/2f + 1f;
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 2f);
            }
        }
    }
}
