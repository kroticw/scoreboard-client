using Avalonia.Controls;

namespace scoreboard_client;

public partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; }
    
    public MainWindow()
    {
        InitializeComponent();
    }
}