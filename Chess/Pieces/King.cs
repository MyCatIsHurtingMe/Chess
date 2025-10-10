using Windows.Media.MediaProperties;

public class King : Piece
{
    King(char col, int[] coords) : base(col, coords) { }
    public override bool IsValidMove(int[] moveCoords)
    {
        return (coordinates[0] != moveCoords[0]) & (coordinates[1] != moveCoords[1]) & (Math.Abs(coordinates[0] - moveCoords[0]) == 1) & (Math.Abs(coordinates[1] - moveCoords[1]) == 1);
    }
}