public class Knight : Piece
{
    Knight(char col, int[] coords) : base(col, coords) { }
    public override bool IsValidMove(int[] moveCoords)
    {
        return ((Math.Abs(coordinates[0] - moveCoords[0])==2) & (Math.Abs(coordinates[1] - moveCoords[1]) == 1))|((Math.Abs(coordinates[1] - moveCoords[1])==2) & (Math.Abs(coordinates[1] - moveCoords[1]) == 1));
    }
}