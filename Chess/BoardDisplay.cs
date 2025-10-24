using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chess;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Media.Devices;

public class BoardDisplay:INotifyPropertyChanged
{
    Button[,] gridIndex = new Button[8, 8];
    Board boardStructure;
    Piece? selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    char currentPlayer = 'w';
    List<Board> moves = [];
    Board temp;
    Popup gameOverPopup;
    Popup promotePopup;
    public char CurrentPlayer { get => currentPlayer; }
    public BoardDisplay(Grid g, Popup gameOver, Popup promote, Button b)
    {
        boardStructure = new(promote);
        promotePopup = promote;
        b.Click += ResetGame;
        int width = 300;
        int height = 200;
        gameOver.Width = width;
        gameOver.Height = height;
        var bounds = Window.Current.Bounds;//maybe breakable
        gameOver.HorizontalOffset = (bounds.Width + width/2) / 2;
        gameOver.VerticalOffset = (bounds.Height + height/2) / 2;
        gameOverPopup = gameOver;
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
                Piece piece = boardStructure[i, j];
                var button = new Button
                {
                    Name = $"{c}{j + 1}",
                    Content = new Image
                    {
                        Source = (piece == null) ? "" : $"Assets/Icons/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
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
                if (boardStructure[Grid.GetColumn(b), Grid.GetRow(b)]?.Colour == currentPlayer)
                {
                    selectedPiece = boardStructure[Grid.GetColumn(b), Grid.GetRow(b)];
                    selectedPieceCoords = [Grid.GetColumn(b), Grid.GetRow(b)];
                }
                Console.WriteLine("piece colour: " + boardStructure[Grid.GetColumn(b), Grid.GetRow(b)]?.Colour);
            }
            else
            {
                int[] moveCoords = [Grid.GetColumn(b), Grid.GetRow(b)];
                temp = (Board)boardStructure.Clone();
                if (selectedPiece.IsValidMove(selectedPieceCoords, moveCoords, temp))
                {
                    temp.UpdateSquare(moveCoords, selectedPiece);
                    temp.UpdateSquare(selectedPieceCoords, null);
                    if (temp.IsAttacked(temp.GetKingCoords(currentPlayer), currentPlayer))
                    {
                        return;
                    }
                    boardStructure = temp;
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
                    Console.WriteLine(boardStructure.JustMovedTwo);
                    currentPlayer = (currentPlayer == 'w') ? 'b' : 'w';
                    selectedPiece = null;
                    boardStructure.MoveSuccess();
                }
                else
                {
                    selectedPiece = null;
                    OnClick(b, null);
                }
            }
        }
        Console.WriteLine("play colour: " + currentPlayer);
        if(boardStructure.isCheckmate(boardStructure.GetKingCoords(currentPlayer), currentPlayer)){
            Console.WriteLine(currentPlayer + "checkmated");
            DisableButtons();
            gameOverPopup.IsOpen = true;
        }
    }
    public void UpdateButtons()
    {
        List<int[]> list = boardStructure.ButtonUpdates;
        foreach (int[] coords in list)
        {
            Piece? piece = boardStructure[coords];
            if (piece == null)
            {
                gridIndex[coords[0], coords[1]].Content = null;
            }
            else
            {
                Image image = new Image
                {
                    Source = $"Assets/Icons/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                };
                gridIndex[coords[0], coords[1]].Content = image;
            }
        }
    }
    public void EnableButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridIndex[i, j].IsEnabled = true;
            }
        }
    }
    public void DisableButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridIndex[i, j].IsEnabled = false;
            }
        }
    }
    public void ReloadButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece? piece = boardStructure[i,j];
                if (piece == null)
                {
                    gridIndex[i,j].Content = null;
                }
                else
                {
                    Image image = new Image
                    {
                        Source = $"Assets/Icons/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                    };
                    gridIndex[i,j].Content = image;
                }
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void ResetGame(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Working");
        gameOverPopup.IsOpen = false;
        boardStructure = new(promotePopup);
        currentPlayer = 'w';
        moves = [];
        ReloadButtons();
        EnableButtons();
    }
}