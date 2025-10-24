namespace Chess;
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = new BoardDisplay(BoardGrid, GameOver, Promote, PlayAgain);
    }
}