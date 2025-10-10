using System.Drawing;

public class Rook : Piece
{
    Rook(char col, int[] coords) : base(col, coords) { }
    public override Boolean IsValidMove(int[] moveCoords)
    {
        return (moveCoords[0]==coordinates[0]) ^ (moveCoords[1]==coordinates[1]);
    }
}