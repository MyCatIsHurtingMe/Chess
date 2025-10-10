namespace Chess;
public class Board
{
    private Piece[,] board;

    public Piece this[int i, int j]=>board[i,j];
    Board()
    {
        board = new Piece[8, 8];
        //initialising pieces
        board[0, 0] = new Rook('W');
        board[1, 0] = new Knight('W');
        board[2, 0] = new Bishop('W');
        board[3, 0] = new Queen('W');
        board[4, 0] = new King('W');
        board[5, 0] = new Bishop('W');
        board[6, 0] = new Knight('W');
        board[7, 0] = new Rook('W');
        for (int i = 0; i < 8; i++)
        {
            board[i, 1] = new Pawn('W');
            board[i, 6] = new Pawn('B');
        }
        board[0, 7] = new Rook('B');
        board[1, 7] = new Knight('B');
        board[2, 7] = new Bishop('B');
        board[3, 7] = new Queen('B');
        board[4, 7] = new King('B');
        board[5, 7] = new Bishop('B');
        board[6, 7] = new Knight('B');
        board[7, 7] = new Rook('B');
    }
    
}