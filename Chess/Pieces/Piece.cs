namespace Chess;

public abstract class Piece(char col)
{
    protected char colour = col;
    public char Colour => colour;

    public abstract bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display);
}