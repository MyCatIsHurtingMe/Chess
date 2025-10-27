using System.Runtime.Serialization;

namespace Chess;

public class Queen(PieceColour col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        int xDiff = moveCoords[0] - curCoords[0];
        int yDiff = moveCoords[1] - curCoords[1];
        if ((xDiff == 0) ^ (yDiff == 0) ^ (Math.Abs(xDiff) == Math.Abs(yDiff)))
        {
            int xSign = xDiff switch
            {
                < 0 => -1,
                0 => 0,
                > 0 => 1
            };
            int ySign = yDiff switch
            {
                < 0 => -1,
                0 => 0,
                > 0 => 1
            };
            int x = curCoords[0] + xSign;
            int y = curCoords[1] + ySign;
            while (x != moveCoords[0])
            {
                if (board[x, y] != null) return false;
                x += xSign;
                y += ySign;
            }
            if (board[moveCoords]?.Colour != colour) return true;
        }
        return false;
    }
}