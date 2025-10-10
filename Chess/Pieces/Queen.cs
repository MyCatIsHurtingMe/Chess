namespace Chess;

public class Queen(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        return ((moveCoords[0] == curCoords[0]) ^ (moveCoords[1] == curCoords[1])) | ((curCoords[0] != moveCoords[0]) & (Math.Abs(moveCoords[0] - moveCoords[1]) == Math.Abs(curCoords[0] - curCoords[1])));
    }
}