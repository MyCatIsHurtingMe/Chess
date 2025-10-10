using Microsoft.UI.Xaml.Media;

namespace Chess;

public sealed partial class MainPage : Page
{
    Piece selectedPiece = null;
    public MainPage()
    {
        this.InitializeComponent();
        for (int i = 0; i < 8; i++)
        {
            board.RowDefinitions.Add(new RowDefinition());
            board.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int i = 0; i < 8; i++)
        {
            char c = (char)(i + 65);
            for (int j = 0; j < 8; j++)
            {
                var button = new Button
                {
                    Name = $"{c}{j+1}",
                    Content = $"{c}{j+1}"
                };
                button.Click+=OnClick;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, j);
                board.Children.Add(button);
            }
        }
    }
    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button b)
        {
            int[] moveCoords = [Grid.GetRow(b), Grid.GetColumn(b)];
        }
        //else throw exception
    }
}
