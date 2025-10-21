namespace Chess;

public abstract class Piece(char col) : ICloneable
{
    protected bool hasMoved = false;
    public bool HasMoved => hasMoved;
    protected char colour = col;
    public char Colour => colour;
    public abstract bool IsValidMove(int[] curCoords, int[] moveCoords, Board board);
    public object Clone()
    {
        return MemberwiseClone();
    }
}