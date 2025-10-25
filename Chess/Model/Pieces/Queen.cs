namespace Chess;

public class Queen(PieceColour col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        if ((moveCoords[0] == curCoords[0]) ^ (moveCoords[1] == curCoords[1]))
        {
            if (moveCoords[0] == curCoords[0])
            {
                if (moveCoords[1] > curCoords[1])
                {
                    for (int i = 1; i < moveCoords[1] - curCoords[1]; i++)
                    {
                        if (board[curCoords[0], curCoords[1] + i] != null) return false;
                    }
                }
                else
                {
                    for (int i = 1; i < curCoords[1] - moveCoords[1]; i++)
                    {
                        if (board[moveCoords[0], moveCoords[1] + i] != null) return false;
                    }
                }
            }
            else
            {
                if (moveCoords[0] > curCoords[0])
                {
                    for (int i = 1; i < moveCoords[0] - curCoords[0]; i++)
                    {
                        if (board[curCoords[0] + i, curCoords[1]] != null) return false;
                    }
                }
                else
                {
                    for (int i = 1; i < curCoords[0] - moveCoords[0]; i++)
                    {
                        if (board[moveCoords[0] + i, moveCoords[1]] != null) return false;
                    }
                }
            }
            return board[moveCoords]?.Colour != colour;
        }
        int xDiff = curCoords[0] - moveCoords[0];
        int yDiff = curCoords[1] - moveCoords[1];
        if ((curCoords[0] != moveCoords[0]) & (Math.Abs(xDiff) == Math.Abs(yDiff)))
        {
            if (board[moveCoords[0], moveCoords[1]]?.Colour == colour) return false;
            if (xDiff < 0)
            {
                if (yDiff < 0)
                {
                    for (int i = 1; i < Math.Abs(xDiff); i++)
                    {
                        if (board[curCoords[0] + i, curCoords[1] + i] != null) return false;
                    }
                }
                else
                {
                    for (int i = 1; i < Math.Abs(xDiff); i++)
                    {
                        if (board[curCoords[0] + i, curCoords[1] - i] != null) return false;
                    }
                }
            }
            else
            {
                if (yDiff < 0)
                {
                    for (int i = 1; i < Math.Abs(xDiff); i++)
                    {
                        if (board[curCoords[0] - i, curCoords[1] + i] != null) return false;
                    }

                }
                else
                {
                    for (int i = 1; i < Math.Abs(xDiff); i++)
                    {
                        if (board[curCoords[0] - i, curCoords[1] - i] != null) return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
}