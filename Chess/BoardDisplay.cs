using Chess;

public class BoardDisplay
{
    Button[,] gridIndex = new Button[8, 8];
    Board boardStructure = new(); //make board have grid instead
    Piece selectedPiece = null;
    int[] selectedPieceCoords = new int[2];
    char currentPlayer = 'W';
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
                    Content = (boardStructure[i, j] == null) ? "" : $"{boardStructure[i,j].Colour} {boardStructure[i, j].GetType().Name}"
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
                    boardStructure[moveCoords] = selectedPiece;
                    boardStructure[selectedPieceCoords] = null;
                    selectedPiece = null;
                    updateButton(moveCoords);
                    updateButton(selectedPieceCoords);
                    currentPlayer = (currentPlayer == 'W') ? 'B' : 'W';
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
    public void updateButton(int[] coords)
    {
        gridIndex[coords[0], coords[1]].Content = (boardStructure[coords] == null) ? "" :$"{boardStructure[coords]?.Colour} {boardStructure[coords]?.GetType().Name}";
    }
}