public class Pawn : Piece
{
    Pawn(char col, int[] coords) : base(col, coords) { }
    public override bool IsValidMove(int[] moveCoords)
    {
        if (colour == 'W') return (moveCoords[0] == coordinates[0]) & (moveCoords[1] - coordinates[1] < 3);
        if (colour == 'B') return (moveCoords[0] == coordinates[0]) & (coordinates[1] - moveCoords[1] < 3);
        return false;//throw wrong colour exception
    }
}