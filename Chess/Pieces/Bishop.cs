namespace Chess;

public class Bishop(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
    {
        int xDiff = curCoords[0] - moveCoords[0];
        int yDiff = curCoords[1] - moveCoords[1];
        if ((curCoords[0] == moveCoords[0]) | (Math.Abs(xDiff) != Math.Abs(yDiff))) return false;
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
}