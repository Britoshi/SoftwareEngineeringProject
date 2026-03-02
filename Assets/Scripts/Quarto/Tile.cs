using UnityEngine;

namespace Quarto
{
    public class Tile : MonoBehaviour
    {
        public int X, Y;
        public Piece Piece;
        public bool IsOccupied => Piece != null;
    }
}
