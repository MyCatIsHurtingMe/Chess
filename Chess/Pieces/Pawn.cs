namespace Chess;

public class Pawn(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board)
    {
        if (colour == 'W') return (moveCoords[0] == curCoords[0]) & (moveCoords[1] - curCoords[1] < 3);
        if (colour == 'B') return (moveCoords[0] == curCoords[0]) & (curCoords[1] - moveCoords[1] < 3);
        return false;//throw wrong colour exception
    }
}