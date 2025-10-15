namespace Chess;

public class Knight(char col) : Piece(col)
{
    public override bool IsValidMove(int[] curCoords, int[] moveCoords, Board board, BoardDisplay display)
    {
        return (((Math.Abs(curCoords[0] - moveCoords[0]) == 2) & (Math.Abs(curCoords[1] - moveCoords[1]) == 1)) | (Math.Abs(curCoords[1] - moveCoords[1]) == 2) & (Math.Abs(curCoords[0] - moveCoords[0]) == 1))&(board[moveCoords]?.Colour!=colour);
    }
}