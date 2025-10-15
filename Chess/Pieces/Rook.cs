namespace Chess;

public class Rook(char col) : Piece(col)
{
    public override Boolean IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
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
        return false;
    }
}