namespace Chess;
public sealed partial class MainPage : Page
{
    Button[,] gridIndex = new Button[8, 8];
    public MainPage()
    {
        InitializeComponent();
        DataContext = new BoardDisplay(BoardGrid, GameOver, Promote, PlayAgain);
    }
}