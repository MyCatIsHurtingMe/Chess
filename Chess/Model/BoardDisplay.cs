using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chess;
using Microsoft.UI.Xaml.Controls.Primitives;

public class BoardDisplay:INotifyPropertyChanged
{
    Button[,] gridIndex = new Button[8, 8];
    Board board;
    Piece? selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    PieceColour currentPlayer = PieceColour.White;
    List<Board> moves = [];
    Board temp;
    Popup gameOverPopup;
    Popup promotePopup;
     public event PropertyChangedEventHandler PropertyChanged;
    [Bindable(true)]
    public PieceColour CurrentPlayer { get => currentPlayer; }
    // public Board Board {get => board; }
    // public BoardDisplay()
    // {
    //     board = new();
    // }
    public BoardDisplay(Grid g, Popup gameOver, Popup promote, Button b)
    {
        board = new(promote);
        promotePopup = promote;
        b.Click += ResetGame;
        int width = 300;
        int height = 200;
        gameOver.Width = width;
        gameOver.Height = height;
        var bounds = Window.Current.Bounds;//maybe breakable
        gameOver.HorizontalOffset = (bounds.Width + width) / 2;
        gameOver.VerticalOffset = (bounds.Height + height) / 2;
        gameOverPopup = gameOver;
        promote.Width = width;
        promote.Height = height;
        promote.HorizontalOffset = (bounds.Width + width) / 2;
        promote.VerticalOffset = (bounds.Height + height) / 2;
        for (int i = 0; i < 8; i++)
        {
            g.RowDefinitions.Add(new RowDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int i = 0; i < 8; i++)
        {
            char c = (char)(i + 65);
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board[i, j];
                var button = new Button
                {
                    Name = $"{c}{j + 1}",
                    Content = new Image
                    {
                        Source = (piece == null) ? "" : $"Assets/Images/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                    },
                    Width = 100,
                    Height = 100,
                    Background = ((i + j) % 2 == 0) ? "Green" : "DarkGreen"
                };
                button.Click += OnClick;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                g.Children.Add(button);
                gridIndex[i, j] = button;
            }
        }
    }
    private async void OnClick(object sender, RoutedEventArgs? e)
    {
        if (sender is Button b)
        {
            if (selectedPiece == null)
            {
                if (board[Grid.GetColumn(b), Grid.GetRow(b)]?.Colour == currentPlayer)
                {
                    selectedPiece = board[Grid.GetColumn(b), Grid.GetRow(b)];
                    selectedPieceCoords = [Grid.GetColumn(b), Grid.GetRow(b)];
                }
                Console.WriteLine("piece colour: " + board[Grid.GetColumn(b), Grid.GetRow(b)]?.Colour);
            }
            else
            {
                int[] moveCoords = [Grid.GetColumn(b), Grid.GetRow(b)];
                temp = (Board)board.Clone();
                if (selectedPiece.IsValidMove(selectedPieceCoords, moveCoords, temp))
                {
                    temp.UpdateSquare(moveCoords, selectedPiece);
                    temp.UpdateSquare(selectedPieceCoords, null);
                    if (temp.IsAttacked(temp.GetKingCoords(currentPlayer), currentPlayer))
                    {
                        return;
                    }
                    board = temp;
                    UpdateButtons();
                    if (promotePopup.IsOpen)
                    {
                        DisableButtons();
                        while (promotePopup.IsOpen)
                        {
                            await Task.Delay(100);
                        }
                        EnableButtons();
                        UpdateButtons();
                    }
                    Console.WriteLine(board.JustMovedTwo);
                    currentPlayer = (currentPlayer == PieceColour.White) ? PieceColour.Black : PieceColour.White;
                    selectedPiece = null;
                    board.MoveSuccess();
                }
                else
                {
                    selectedPiece = null;
                    OnClick(b, null);
                }
            }
        }
        Console.WriteLine("play colour: " + currentPlayer);
        if (board.isCheckmate(board.GetKingCoords(currentPlayer), currentPlayer))
        {
            Console.WriteLine(currentPlayer + "checkmated");
            DisableButtons();
            gameOverPopup.IsOpen = true;
        }
    }
    //updates the buttons in the ButtonUpdates list, then clears the list
    public void UpdateButtons()
    {
        List<int[]> list = board.ButtonUpdates;
        foreach (int[] coords in list)
        {
            Piece? piece = board[coords];
            if (piece == null)
            {
                gridIndex[coords[0], coords[1]].Content = null;
            }
            else
            {
                Image image = new Image
                {
                    Source = $"Assets/Images/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                };
                gridIndex[coords[0], coords[1]].Content = image;
            }
        }
    }
    //adds the onclick to all buttons
    public void EnableButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridIndex[i, j].Click += OnClick;
            }
        }
    }
    //removes the onclick from all buttons
    public void DisableButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridIndex[i, j].Click -= OnClick;
            }
        }
    }
    //updates every button on the frontend board to match the backend
    public void ReloadButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = board[i,j];
                if (piece == null)
                {
                    gridIndex[i,j].Content = null;
                }
                else
                {
                    Image image = new Image
                    {
                        Source = $"Assets/Images/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                    };
                    gridIndex[i,j].Content = image;
                }
            }
        }
    }
    //resets the game board for a new game
    private void ResetGame(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Working");
        gameOverPopup.IsOpen = false;
        board = new(promotePopup);
        currentPlayer = PieceColour.White;
        moves = [];
        ReloadButtons();
        EnableButtons();
    }
}