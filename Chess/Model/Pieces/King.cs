namespace Chess;

public class King(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        if (!hasMoved & (Math.Abs(curCoords[0]-moveCoords[0])==2) & (curCoords[1]-moveCoords[1]==0))
        {
            if (curCoords[0] - moveCoords[0] < 0)
            {
                for (int i=curCoords[0]+1;i<moveCoords[0];i++)
                {
                    if (board[i, curCoords[1]] != null) return false;
                }
                Piece piece = board[7, curCoords[1]];
                if ((piece?.GetType().Name == "Rook") & (piece?.HasMoved==false))
                {
                    board.UpdateSquare([5, curCoords[1]], piece);
                    board.UpdateSquare([7, curCoords[1]], null);
                    board.SetKingCoords(moveCoords, colour);
                    return true;
                }
            }
            else
            {
                for (int i=moveCoords[0]+1;i<curCoords[0];i++)
                {
                    if (board[i, curCoords[1]] != null) return false;
                }
                Piece piece = board[0, curCoords[1]];
                if ((piece?.GetType().Name == "Rook") & (piece?.HasMoved==false))
                {
                    board.UpdateSquare([3, curCoords[1]], piece);
                    board.UpdateSquare([0, curCoords[1]], null);
                    board.SetKingCoords(moveCoords, colour);
                    return true;
                }
            }
        }
        if (((Math.Abs(curCoords[0] - moveCoords[0]) == 1) | (Math.Abs(curCoords[1] - moveCoords[1]) == 1)) & (board[moveCoords]?.Colour != colour))
        {
            board.SetKingCoords(moveCoords, colour);
            return true;
        }
        return false;
    }
}