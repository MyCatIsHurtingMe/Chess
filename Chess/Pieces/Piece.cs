namespace Chess;

public abstract class Piece(char col)
{
    protected char colour = col;
    public char Colour => colour;

    public abstract bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display);
    public Piece? CheckRow(int i, int j, int[] coords, Board board)
    {
        int x = coords[0] + i;
        int y = coords[1] + j;
        while ((x >= 0) & (x < 8) & (y >= 0) & (y < 8))
        {
            if (board[x, y] != null) return board[x, y];
        }
        return null;
    }
}