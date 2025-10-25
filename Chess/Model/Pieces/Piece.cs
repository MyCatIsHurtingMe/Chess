namespace Chess;

public abstract class Piece(PieceColour col) : ICloneable
{
    protected bool hasMoved = false;
    public bool HasMoved => hasMoved;
    protected PieceColour colour = col;
    public PieceColour Colour => colour;
    public abstract bool IsValidMove(int[] curCoords, int[] moveCoords, Board board);
    public object Clone()
    {
        return MemberwiseClone();
    }
}