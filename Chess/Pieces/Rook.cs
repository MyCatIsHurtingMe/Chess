namespace Chess;

public class Rook(char col) : Piece(col)
{
    public override Boolean IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        return (moveCoords[0]==curCoords[0]) ^ (moveCoords[1]==curCoords[1]);
    }
}