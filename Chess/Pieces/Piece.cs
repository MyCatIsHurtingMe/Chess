public abstract class Piece
{
    protected char colour;
    public char getColour(){
            return colour;
    }
    protected int[] coordinates;
    public int[] getCoordinates()
    {
        return coordinates;
    }
    protected Piece(char col, int[] coords)
    {
        colour = col;
        coordinates = coords;
    }
    public abstract Boolean IsValidMove(int[] moveCoords);
}