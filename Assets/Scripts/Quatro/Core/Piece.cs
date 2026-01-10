using System;
using UnityEngine;

namespace Quatro.Core
{
    public class Piece : BoardBehavior
    {
        // public Vector3 boardPosition;
        public bool Placed;
        public Tile AssignedTile;
        /// <summary>
        /// 0 = not placed.
        /// 1 = Player 1
        /// 2 = Player 2
        /// </summary>
        public int PlayerMaker = 0;
        
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
            if(Board.Phase == Phase.Player1Place) PlayerMaker = 1;
            else if(Board.Phase == Phase.Player2Place) PlayerMaker = 2;
            else throw new Exception("Critical Error took place where piece is being placed in non-placing phase.");
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
