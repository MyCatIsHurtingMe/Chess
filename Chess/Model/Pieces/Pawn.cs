using System.Transactions;

namespace Chess;

public class Pawn(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        bool valid = false;
        int sign = (colour == 'w') ? 1 : -1;

        if ((Math.Abs(moveCoords[0] - curCoords[0]) == 1) & (sign * (moveCoords[1] - curCoords[1]) == 1))
        {
            if ((board[moveCoords]?.Colour != colour) & (board[moveCoords] != null))
            {
                valid = true;
            }
            if(board[moveCoords[0], curCoords[1]] == board.JustMovedTwo){
                board.UpdateSquare([moveCoords[0], curCoords[1]], null);
                valid = true;
            }

        }
        if ((curCoords[0] == moveCoords[0]) & (board[moveCoords] == null))
        {
            if (!hasMoved & (Math.Abs(moveCoords[1] - curCoords[1]) == 2))
            {
                valid = (sign * (moveCoords[1] - curCoords[1]) > 0) & (board[moveCoords[0], moveCoords[1] - sign] == null);
                if (valid == true) board.JustMovedTwo = this;
            }
            else
            {
                valid = sign * (moveCoords[1] - curCoords[1]) == 1;
            }
        }
        int finalRank = (sign == 1) ? 7 : 0;
        if (valid)
        {
            if (moveCoords[1] == finalRank) board.PromoteMenu(moveCoords, colour);
            else hasMoved = true;
            return true;
        }
        return false;
    }
}