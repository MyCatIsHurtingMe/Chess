using Chess;

public class BoardDisplay
{
    Button[,] gridIndex = new Button[8, 8];
    Board boardStructure = new();
    Piece? selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    char currentPlayer = 'w';
    List<Board> moves = [];
    Board temp;
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
                Piece piece=boardStructure[i, j];
                var button = new Button
                {
                    Name = $"{c}{j + 1}",
                    Content = new Image
                    {
                        Source=(piece == null) ? "" : $"Assets/Icons/{piece.Colour}_{piece.GetType().Name.ToLower()}.png" 
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
    private void OnClick(object sender, RoutedEventArgs? e)
    {
        if (sender is Button b)
        {
            if (selectedPiece == null)
            {
                // Console.WriteLine(boardStructure.GetKingCoords(currentPlayer)[1]);
                // if(boardStructure.IsAttacked(boardStructure.GetKingCoords(currentPlayer), currentPlayer))
                // {
                //     //check for checkmate
                // }
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
    }
    public void UpdateButtons()
    {
        List<int[]> list = boardStructure.ButtonUpdates;
        foreach (int[] coords in list)
        {
            Console.WriteLine(coords[0]+","+coords[1]);
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
}