public class Bishop : Piece
{
    Bishop(char col, int[] coords) : base(col, coords) { }
    public override bool IsValidMove(int[] moveCoords)
    {
        return (coordinates[0] != moveCoords[0]) & (Math.Abs(moveCoords[0] - moveCoords[1]) == Math.Abs(coordinates[0] - coordinates[1]));
    }
}