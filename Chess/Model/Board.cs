using Windows.Media.SpeechSynthesis;

namespace Chess;
public class Board:ICloneable
{
    private Piece[,] board = new Piece[8, 8];
    Pawn justMovedTwo = null;
    bool wipeJustMovedPawn = false;
    King whiteKing;
    King blackKing;
    int[] whiteKingCoords;
    int[] blackKingCoords;
    int currentMove = 0;
    List<int[]> buttonUpdates = new List<int[]>();
    public King GetKing(char colour)
    {
        return (colour == 'w') ? whiteKing : blackKing;
    }
    public int[] GetKingCoords(char colour)
    {
        return (colour == 'w') ? whiteKingCoords : blackKingCoords;
    }
    public void SetKingCoords(int[] coords, char colour)
    {
        if (colour == 'w') whiteKingCoords = coords;
        if (colour == 'b') blackKingCoords = coords;
    }
    public Piece this[int column, int row] {
        get => board[column, row];
        set => board[column, row] = value;
    }
    public Piece this[int[] i]
    {
        get => board[i[0], i[1]];
        set => board[i[0], i[1]] = value;
    }
    public List<int[]> ButtonUpdates => buttonUpdates;
    public Board()
    {
        //initialising pieces
        board[0, 0] = new Rook('w');
        board[1, 0] = new Knight('w');
        board[2, 0] = new Bishop('w');
        board[3, 0] = new Queen('w');
        whiteKing = new King('w');
        whiteKingCoords = [4, 0];
        board[4, 0] = whiteKing;
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
        blackKing = new King('b');
        blackKingCoords = [4, 7];
        board[4, 7] = blackKing;
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
    public void MoveSuccess()
    {
        currentMove++;
        if (wipeJustMovedPawn) justMovedTwo = null;
        wipeJustMovedPawn = true;
        buttonUpdates.Clear();
    }
    public Piece? CheckLine(int i, int j, int[] coords)
    {
        int x = coords[0] + i;
        int y = coords[1] + j;
        while ((x >= 0) & (x < 8) & (y >= 0) & (y < 8))
        {
            if (board[x, y] != null) return board[x, y];
            x += i;
            y += j;
        }
        return null;
    }
    public bool IsAttacked(int[] coords, char colour)
    {
        Piece? piece;
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) == Math.Abs(j) & (i != 0))
                {
                    //diagonal checks
                    piece = CheckLine(i, j, coords);
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
                    //knight checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if (x + i >= 0 & x + i < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x + i, y];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour)) return true;
                    }
                    if (x >= 0 & x < 8 & y + j >= 0 & y + j < 8)
                    {
                        piece = board[x, y + j];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour)) return true;
                    }
                    //diagonal king checks
                    if (x >= 0 & x < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x, y];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) return true;
                    }
                }
                if (Math.Abs(i) != Math.Abs(j))
                {
                    //axis (non-diagonal line) checks
                    piece = CheckLine(i, j, coords);
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
                    //axis king checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if ((x >= 0) & (x < 8) & (y >= 0) & (y < 8))
                    {
                        piece = board[x, y];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) return true;
                    }
                }
            }
        }
        //Pawn checks
        int sign = (colour == 'w') ? 1 : -1;
        x = coords[0] + sign;
        if (x >= 0 & x > 8)
        {
            if (coords[1] + 1 < 8)
            {
                piece = board[x, coords[1] + 1];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
            if (coords[1] - 1 >= 0)
            {
                piece = board[x, coords[1] - 1];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
        }
        return false;
    }
    public object Clone()
    {
        Piece[,] copy = new Piece[8, 8];
        int[] whiteCoords = new int[2];
        int[] blackCoords = new int[2];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? temp = board[i, j];
                copy[i, j] = (temp == null) ? null : (Piece)temp.Clone();
            }
        }
        Array.Copy(whiteKingCoords, whiteCoords, 2);
        Array.Copy(blackKingCoords, blackCoords, 2);
        return new Board
        {
            board = copy,
            justMovedTwo = (Pawn)justMovedTwo?.Clone(),
            wipeJustMovedPawn = wipeJustMovedPawn,
            whiteKing = (King)whiteKing.Clone(),
            blackKing = (King)blackKing.Clone(),
            currentMove = currentMove,
            whiteKingCoords = whiteCoords,
            blackKingCoords = blackCoords,
            buttonUpdates = new List<int[]>()
        };
    }
    public void PrintBoard(Piece[,] b)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = b[i, j];
                Console.Write((piece == null) ? "null " : $"{piece.Colour}_{piece.GetType().Name} ");
            }
            Console.WriteLine();
        }
    }
    public void UpdateSquare(int[] coords, Piece? piece)
    {
        this[coords] = piece;
        buttonUpdates.Add(coords);
    }
}