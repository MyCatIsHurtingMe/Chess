namespace Chess;

public class King(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
    {
        return (curCoords[0] != moveCoords[0]) & (curCoords[1] != moveCoords[1]) & (Math.Abs(curCoords[0] - moveCoords[0]) == 1) & (Math.Abs(curCoords[1] - moveCoords[1]) == 1);
    }
    // public bool IsAttacked(int[] coords, Board board)
    // {
    //     //this can't be right. This is an error-prone monstrosity. Rewrite this
    //     Piece? piece = CheckRow(1, 0, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(-1, 0, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(0, 1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(0, -1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(1, 1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(-1, 1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(-1, -1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
    //     piece = CheckRow(1, -1, coords, board);
    //     if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;

    //     return false;
    // }
}