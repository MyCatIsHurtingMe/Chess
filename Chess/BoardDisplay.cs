using Chess;
using Microsoft.UI.Xaml.Media.Imaging;

public class BoardDisplay
{
    Button[,] gridIndex = new Button[8, 8];
    Board boardStructure = new();
    Piece? selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    char currentPlayer = 'w';
    public BoardDisplay(Grid g)
    {
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
                var button = new Button
                {
                    Name = $"{c}{j + 1}",
                    Content = new Image
                    {
                        Source=(boardStructure[i, j] == null) ? "" : $"Assets/Icons/{boardStructure[i, j].Colour}_{boardStructure[i, j].GetType().Name.ToLower()}.png" 
                    },
                    Width = 100,
                    Height = 100,
                    Background = ((i+j)%2==0)?"Green":"DarkGreen"
                };
                button.Click += OnClick;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                g.Children.Add(button);
                gridIndex[i, j] = button;
            }
        }
    }
    private void OnClick(object sender, RoutedEventArgs e)
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
                if (selectedPiece.IsValidMove(selectedPieceCoords, moveCoords, boardStructure, this))
                {
                    UpdateSquare(moveCoords, selectedPiece);
                    UpdateSquare(selectedPieceCoords, null);
                    selectedPiece = null;
                    currentPlayer = (currentPlayer == 'w') ? 'b' : 'w';
                    boardStructure.Wipe();
                }
                else
                {
                    selectedPiece = null;
                    OnClick(b, null);
                }
            }
            Console.WriteLine(selectedPiece?.GetType().Name);
        }
        Console.WriteLine("play colour: " + currentPlayer);
    }
    public void UpdateSquare(int[] coords, Piece? piece)
    {
        boardStructure[coords] = piece;
        UpdateButton(coords);
    }
    public void UpdateButton(int[] coords)
    {
        Piece? piece = boardStructure[coords];
        if (piece == null)
        {
            gridIndex[coords[0], coords[1]].Content = null;
            return;
        }
        Image image = new Image
        {
            Source = $"Assets/Icons/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
        };
        gridIndex[coords[0], coords[1]].Content = image;
    }
}