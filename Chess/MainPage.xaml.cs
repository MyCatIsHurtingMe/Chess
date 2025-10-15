using System.Globalization;
using Microsoft.UI.Xaml.Media;

namespace Chess;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        BoardDisplay boardDisplay = new(boardGrid);
    }
}