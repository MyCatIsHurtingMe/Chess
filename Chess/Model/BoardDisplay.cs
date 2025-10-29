using System.ComponentModel;
using Chess;
using Microsoft.UI.Xaml.Controls.Primitives;

public class BoardDisplay:INotifyPropertyChanged
{
    Button[,] gridIndex = new Button[8, 8];
    Board board;
    Piece? selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    [Bindable(true)]
    PieceColour currentPlayer = PieceColour.White;
    public PieceColour CurrentPlayer { get => currentPlayer; }
    List<Board> moves = [];
    Board temp;
    Popup gameOverPopup;
    Popup promotePopup;
     public event PropertyChangedEventHandler PropertyChanged;
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
                Button button = new Button
                {
                    Background = ((i + j) % 2 == 0) ? "Green" : "DarkGreen",
                    Width = 100,
                    Height = 100,
                    Content = new Image
                    {
                        Source = (piece == null) ? "" : $"ms-appx:///Assets/Images/{piece.Colour.ToString().ToLower()}_{piece.GetType().Name.ToLower()}.png"
                    }
                };
                Console.WriteLine(1);
                button.Click += OnClick;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                g.Children.Add(button);
                Console.WriteLine(g.Children.Count);
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
                    if (board.PromoteCoords!=null)
                    {
                        board.PromoteMenu(currentPlayer);
                        DisableButtons();
                        while (board.PromoteCoords!=null)
                        {
                            await Task.Delay(100);
                        }
                        EnableButtons();
                        UpdateButtons();
                    }
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
        if (board.IsCheckmate(board.GetKingCoords(currentPlayer), currentPlayer))
        {
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
                    Source = $"ms-appx:///Assets/Images/{piece.Colour.ToString().ToLower()}_{piece.GetType().Name.ToLower()}.png"
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
                        Source = $"ms-appx:///Assets/Images/{piece.Colour.ToString().ToLower()}_{piece.GetType().Name.ToLower()}.png"
                    };
                    gridIndex[i,j].Content = image;
                }
            }
        }
    }
    //resets the game board for a new game
    private void ResetGame(object sender, RoutedEventArgs e)
    {
        gameOverPopup.IsOpen = false;
        board = new(promotePopup);
        currentPlayer = PieceColour.White;
        moves = [];
        ReloadButtons();
        EnableButtons();
    }
}