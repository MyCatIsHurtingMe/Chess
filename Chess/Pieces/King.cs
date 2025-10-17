using Uno.WinUI.Runtime.Skia.X11;

namespace Chess;

public class King(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
    {
        return (curCoords[0] != moveCoords[0]) & (curCoords[1] != moveCoords[1]) & (Math.Abs(curCoords[0] - moveCoords[0]) == 1) & (Math.Abs(curCoords[1] - moveCoords[1]) == 1);
    }
    public bool IsAttacked(int[] coords, Board board)
    {
        Piece? piece;
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) == Math.Abs(j) & i != 0)
                {
                    //diagonal checks
                    piece = CheckLine(i, j, coords, board);
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
                    //knight checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if (x + i >= 0 & x + i < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x + i, y];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour)) return true;
                    }
                    if (x >= 0 & x < 8 & y + j >= 0 & y + j < 8)
                    {
                        piece = board[x, y + j];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour)) return true;
                    }
                    //diagonal king checks
                    if (x >= 0 & x < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x, y];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) return true;
                    }
                }
                if (Math.Abs(i) != Math.Abs(j))
                {
                    //axis (non-diagonal line) checks
                    piece = CheckLine(i, j, coords, board);
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
                    //axis king checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if (x >= 0 & x < 8 & y >= 0 & y < 8)
                    {
                        piece = board[i, j];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) return true;
                    }
                }
            }
        }
        //Pawn checks
        int sign = (colour == 'w') ? 1 : -1;
        x = coords[0] + sign;
        if (x >= 0 & x > 8)
        {
            if (coords[1] + 1 < 8)
            {
                piece = board[x, coords[1] + 1];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
            if (coords[1] - 1 >= 0)
            {
                piece = board[x, coords[1] - 1];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
        }
        return false;
    }
}