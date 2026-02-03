using BritoWorks;
using System.Collections.Generic;
using UnityEngine;

namespace Quarto
{
    public enum TileData : byte
    {
        Black = 0b0000_0001,
        White = 0b0000_0010,
        Tall = 0b0000_0100,
        Short = 0b0000_1000,
        Full = 0b0001_0000,
        Hollow = 0b0010_0000,
    }

    public class GameManager : BritoBehavior
    {
        private static readonly int[] HORIZONTAL =
        {
            0b0001_0001_0001_0001,
            0b0010_0010_0010_0010,
            0b0100_0100_0100_0100,
            0b1000_1000_1000_1000
        };

        private static readonly int[] VERTICAL =
        {
            0b0000_0000_0000_1111,
            0b0000_0000_1111_0000,
            0b0000_1111_0000_0000,
            0b1111_0000_0000_0000
        };

        private const int DIAGONAL_LEFT = 0b1000_0100_0010_0001;
        private const int DIAGONAL_RIGHT = 0b0001_0010_0100_1000;

        private int boardOccupancy = 0;
        private readonly TileData[] boardData = new TileData[16];

        public void OccupyTile(int x, int y, TileData data)
        {
            //the tile is 4 by 4, so it'll move left depending on the x and y bit wise.
            //E.g. x = 1, y = 1 will be 0b_0001_0000
            boardOccupancy |= 1 << y * 4 + x;
            boardData[y * 4 + x] = data;

            if (CheckWinCondition(out int winner))
            {
                Debug.Log("Player " + winner + " wins!");
            }
        }

        public TileData GetTileData(int x, int y) => boardData[y * 4 + x];

        private bool CheckWinCondition(out int winner)
        {
            foreach ((int dir, int index) in CheckForFill())
            {
                byte check = 0b0011_1111;

                for (int i = 0; i < 4; i++)
                {
                    int targetIndex = dir switch
                    {
                        //Diagonal left
                        2 => i * 4 + i,
                        //Diagonal right
                        3 => (3 - i) * 4 + i,
                        _ => dir == 0 ? index * 4 + i : i * 4 + index
                    };
                    check &= (byte)boardData[targetIndex];
                }

                //This means one or the other won AND there is a condition matching (INVERTED)
                if ((check & 0b0011) <= 0 || check >> 2 <= 0) continue;

                //Return the winning tile
                winner = check & 0b0011;
                return true;
            }

            winner = -1;
            return false;
        }

        private List<(int dir, int index)> CheckForFill()
        {
            var fills = new List<(int dir, int index)>();

            for (int i = 0; i < 4; i++)
            {
                if ((boardOccupancy & HORIZONTAL[i]) == HORIZONTAL[i])
                    fills.Add((0, i));
                if ((boardOccupancy & VERTICAL[i]) == VERTICAL[i])
                    fills.Add((1, i));
            }

            if ((boardOccupancy & DIAGONAL_LEFT) == DIAGONAL_LEFT)
                fills.Add((2, 0));
            if ((boardOccupancy & DIAGONAL_RIGHT) == DIAGONAL_RIGHT)
                fills.Add((3, 0));

            return fills;
        }
    }
}