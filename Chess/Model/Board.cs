using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Globalization.DateTimeFormatting;
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
    Popup promotePopup;
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
    public Board(Popup p)
    {
        promotePopup = p;
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
    public int[] CheckLine(int i, int j, int[] coords)
    {
        int x = coords[0] + i;
        int y = coords[1] + j;
        while ((x >= 0) & (x < 8) & (y >= 0) & (y < 8))
        {
            if (board[x, y] != null) return [x, y];
            x += i;
            y += j;
        }
        return [];
    }
    public bool IsAttacked(int[] coords, char colour)
    {
        Piece? piece;
        int[] attackCoords;
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) == Math.Abs(j) & (i != 0))
                {
                    //diagonal checks
                    attackCoords = CheckLine(i, j, coords);
                    if (attackCoords.Length == 2)
                    {
                        piece = board[attackCoords[0], attackCoords[1]];
                        if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
                    }
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
                    attackCoords = CheckLine(i, j, coords);
                    if (attackCoords.Length != 0)
                    {
                        piece = board[attackCoords[0], attackCoords[1]];
                        if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
                    }
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
        y = coords[1] + sign;
        if (y >= 0 & y < 8)
        {
            if (coords[0] + 1 < 8)
            {
                piece = board[coords[0] + 1, y];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
            if (coords[0] - 1 >= 0)
            {
                piece = board[coords[0] - 1, y];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) return true;
            }
        }
        return false;
    }
    public List<int[]> AttackedBy(int[] coords, char colour)
    {
        List<int[]> attacks = new List<int[]>();
        int[] attackCoords = new int[2];
        Piece? piece;
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (attacks.Count > 1) return attacks;
                if (Math.Abs(i) == Math.Abs(j) & (i != 0))
                {
                    //diagonal checks
                    attackCoords = CheckLine(i, j, coords);
                    piece=(attackCoords.Length == 0)? null:board[attackCoords[0],attackCoords[1]];
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen")))attacks.Add(attackCoords);
                    //knight checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if (x + i >= 0 & x + i < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x + i, y];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour))attacks.Add([x + i, y]);
                    }
                    if (x >= 0 & x < 8 & y + j >= 0 & y + j < 8)
                    {
                        piece = board[x, y + j];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour != colour)) attacks.Add([x, y + j]);
                    }
                    //diagonal king checks
                    if (x >= 0 & x < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x, y];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) attacks.Add([x, y]);
                    }
                }
                if (Math.Abs(i) != Math.Abs(j))
                {
                    //axis (non-diagonal line) checks
                    attackCoords = CheckLine(i, j, coords);
                    piece=(attackCoords.Length == 0)? null:board[attackCoords[0],attackCoords[1]];
                    if (piece != null) if ((piece.Colour != colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) attacks.Add(attackCoords);
                    //axis king checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if ((x >= 0) & (x < 8) & (y >= 0) & (y < 8))
                    {
                        piece = board[x, y];
                        if (piece != null) if ((piece.GetType().Name == "King") & (piece.Colour != colour)) attacks.Add([x,y]);
                    }
                }
            }
        }
        //Pawn checks
        int sign = (colour == 'w') ? 1 : -1;
        y = coords[1] + sign;
        if (y >= 0 & y < 8)
        {
            if (coords[0] + 1 < 8)
            {
                piece = board[coords[0] + 1, y];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) attacks.Add([coords[0]+1,y]);
            }
            if (coords[0] - 1 >= 0)
            {
                piece = board[coords[0] - 1, y];
                if (piece != null) if ((piece.Colour != colour) & (piece.GetType().Name == "Pawn")) attacks.Add([coords[0]-1,y]);
            }
        }
        return attacks;
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
        return new Board(promotePopup)
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
    public bool isCheckmate(int[] coords, char colour)
    {
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 & j == 0) continue;
                x = coords[0] + i;
                y = coords[1] + j;
                if (x >= 0 & x < 8 & y >= 0 & y < 8) if (board[x, y]?.Colour != colour & !IsAttacked([x, y], colour)) return false;
            }
        }
        Console.WriteLine(0);
        //find all attackers for king square
        List<int[]> list = AttackedBy(coords, colour);
        char attackerColour = (colour == 'w') ? 'b' : 'w';
        if (list.Count == 0) return false;
        if (list.Count == 1)
        {
            Console.WriteLine(1);
            int[] attackerCoords = list[0];
            (int xDiff, int yDiff) = (attackerCoords[0] - coords[0], attackerCoords[1] - coords[1]);
            if (Math.Abs(xDiff) <= 1 & Math.Abs(yDiff) <= 1)
            {
                Console.WriteLine(2);
                Piece defender;
                list = AttackedBy(attackerCoords, attackerColour);
                foreach (int[] defenderCoords in list)
                {
                    defender = board[defenderCoords[0], defenderCoords[1]];
                    if (defender.GetType().Name != "King") return false;
                }
            }
            else
            {
                Console.WriteLine(3);
                if ((xDiff == 0) | (yDiff == 0) | Math.Abs(xDiff) == Math.Abs(yDiff))
                {
                    int xSign = xDiff switch
                    {
                        < 0 => -1,
                        0 => 0,
                        > 0 => 1
                    };
                    int ySign = yDiff switch
                    {
                        < 0 => -1,
                        0 => 0,
                        > 0 => 1
                    };
                    (x, y) = (coords[0] + xSign, coords[1] + ySign);
                    while (x != attackerCoords[0])
                    {
                        if (IsReachable([x, y], colour)) return false;
                        x += xSign;
                        y += ySign;
                    }
                }
                if (IsAttacked(attackerCoords, attackerColour)) return false;
            }
        }
        Console.WriteLine(4);
        return true;
    }
    public bool IsReachable(int[] coords, char colour)
    {
        Piece? piece;
        int[] pieceCoords;
        int x;
        int y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) == Math.Abs(j) & (i != 0))
                {
                    //diagonal checks
                    pieceCoords = CheckLine(i, j, coords);
                    piece = board[pieceCoords[0], pieceCoords[1]];
                    if (piece != null) if ((piece.Colour == colour) & ((piece.GetType().Name == "Bishop") | (piece.GetType().Name == "Queen"))) return true;
                    //knight checks
                    x = coords[0] + i;
                    y = coords[1] + j;
                    if (x + i >= 0 & x + i < 8 & y >= 0 & y < 8)
                    {
                        piece = board[x + i, y];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour == colour)) return true;
                    }
                    if (x >= 0 & x < 8 & y + j >= 0 & y + j < 8)
                    {
                        piece = board[x, y + j];
                        if (piece != null) if ((piece.GetType().Name == "Knight") & (piece.Colour == colour)) return true;
                    }
                }
                if (Math.Abs(i) != Math.Abs(j))
                {
                    //axis (non-diagonal line) checks
                    pieceCoords = CheckLine(i, j, coords);
                    piece = board[pieceCoords[0], pieceCoords[1]];
                    if (piece != null) if ((piece.Colour == colour) & ((piece.GetType().Name == "Rook") | (piece.GetType().Name == "Queen"))) return true;
                }
            }
        }
        //Pawn checks
        int sign = (colour == 'w') ? 1 : -1;
        y = coords[1] - sign;
        if (y >= 0 & y < 8)
        {
            piece = board[coords[0], y];
            if (piece != null) if ((piece.Colour == colour) & (piece.GetType().Name == "Pawn")) return true;
        }
        y -= sign;
        if (y >= 0 & y < 8)
        {
            piece = board[coords[0], y];
            if (piece != null) if ((piece.Colour == colour) & (piece.GetType().Name == "Pawn") & (piece.HasMoved == false)) return true;
        }
        return false;
    }
    public void PromoteMenu(int[] coords, char colour)
    {
        StackPanel s = (StackPanel)promotePopup.Child;
        s.Children.Clear();
        s.Children.Add(new TextBlock
        {
            Foreground = "Black",
            HorizontalAlignment = HorizontalAlignment.Center,
            Text = "Choose Promotion:"
        });
        s.Children.Add(BuildButton(coords, "Queen", colour));
        s.Children.Add(BuildButton(coords, "Rook", colour));
        s.Children.Add(BuildButton(coords, "Knight", colour));
        s.Children.Add(BuildButton(coords, "Bishop", colour));
        promotePopup.IsOpen = true;
    }
    private Button BuildButton(int[] coords, string promotion, char colour)
    {
        Button b = new Button
        {
            Foreground = "Black",
            Background = "Gray",
            Content = new Image
            {
                Source = $"../Assets/Icons/{colour}_{promotion.ToLower()}.jpg"
            }
        };
        Console.WriteLine($"../Assets/Icons/{colour}_{promotion.ToLower()}.jpg");
        b.Click += (sender, e) =>
        {
            OnClick(sender, e, coords, promotion, colour);
        };
        return b;
    }
    private void OnClick(object sender, EventArgs e, int[] coords, string promotion, char colour)
    {
        Piece piece = promotion switch
        {
            "Queen" => new Queen(colour),
            "Rook" => new Rook(colour),
            "Knight" => new Knight(colour),
            "Bishop" => new Bishop(colour)
        };
        board[coords[0], coords[1]] = piece;
        promotePopup.IsOpen = false;
    } 
}