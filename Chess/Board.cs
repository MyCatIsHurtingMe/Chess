namespace Chess;
public class Board
{
    private Piece[,] board;
    Pawn justMovedTwo = null;
    bool wipeJustMovedPawn = false;

    public Piece this[int i, int j] {
        get => board[i, j];
        set => board[i, j] = value;
    }
    public Piece this[int[] i]
    {
        get => board[i[0], i[1]];
        set => board[i[0], i[1]] = value;
    }
    public Board()
    {
        board = new Piece[8, 8];
        //initialising pieces
        board[0, 0] = new Rook('w');
        board[1, 0] = new Knight('w');
        board[2, 0] = new Bishop('w');
        board[3, 0] = new Queen('w');
        board[4, 0] = new King('w');
        board[5, 0] = new Bishop('w');
        board[6, 0] = new Knight('w');
        board[7, 0] = new Rook('w');
        for (int i = 0; i < 8; i++)
        {
            board[i, 1] = new Pawn('w');
            board[i, 6] = new Pawn('b');
        }
        board[0, 7] = new Rook('b');
        board[1, 7] = new Knight('b');
        board[2, 7] = new Bishop('b');
        board[3, 7] = new Queen('b');
        board[4, 7] = new King('b');
        board[5, 7] = new Bishop('b');
        board[6, 7] = new Knight('b');
        board[7, 7] = new Rook('b');
    }
    public Pawn JustMovedTwo
    {
        get=>justMovedTwo; 
        set
        {
            justMovedTwo = value;
            wipeJustMovedPawn = false;
        }
    }

    public void Wipe()
    {
        if (wipeJustMovedPawn) justMovedTwo = null;
        wipeJustMovedPawn = true;
    }
}