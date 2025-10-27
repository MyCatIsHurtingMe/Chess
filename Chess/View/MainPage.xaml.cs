namespace Chess;
public sealed partial class MainPage : Page
{
    Button[,] gridIndex = new Button[8, 8];
    public MainPage()
    {
        InitializeComponent();
        for (int i = 0; i < 8; i++)
        {
            BoardGrid.RowDefinitions.Add(new RowDefinition());
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int i = 0; i < 8; i++)
        {
            char c = (char)(i + 65);
            for (int j = 0; j < 8; j++)
            {
                // Piece piece = boardStructure[i, j];
                var button = new Button
                {
                    Name = $"{c}{j + 1}",
                    // Content = new Image
                    // {
                    //     Source = (piece == null) ? "" : $"Assets/Images/{piece.Colour}_{piece.GetType().Name.ToLower()}.png"
                    // },
                    Width = 100,
                    Height = 100,
                    Background = ((i + j) % 2 == 0) ? "Green" : "DarkGreen"
                };
                // button.Click += OnClick;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                BoardGrid.Children.Add(button);
                gridIndex[i, j] = button;
            }
        }
        DataContext = new BoardDisplay(BoardGrid, GameOver, Promote, PlayAgain);
    }
}