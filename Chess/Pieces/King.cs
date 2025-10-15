namespace Chess;

public class King(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
    {
        return (curCoords[0] != moveCoords[0]) & (curCoords[1] != moveCoords[1]) & (Math.Abs(curCoords[0] - moveCoords[0]) == 1) & (Math.Abs(curCoords[1] - moveCoords[1]) == 1);
    }
}