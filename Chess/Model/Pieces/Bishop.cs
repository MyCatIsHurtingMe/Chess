namespace Chess;

public class Bishop(PieceColour col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        int xDiff = moveCoords[0] - curCoords[0];
        int yDiff = moveCoords[1] - curCoords[1];
        if ((curCoords[0] == moveCoords[0]) | (Math.Abs(xDiff) != Math.Abs(yDiff))) return false;
        int xSign = xDiff switch
        {
            < 0 => -1,
            > 0 => 1
        };
        int ySign = yDiff switch
        {
            < 0 => -1,
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
        if (board[moveCoords[0], moveCoords[1]]?.Colour == colour) return false;
        return true;
    }
}