using System;
using Avalonia.Controls;
using Avalonia.Threading;
using connect;

namespace scoreboard_client;

public partial class MainWindow : Window
{
    public static MainWindow? Instance { get; private set; }

    public void SetGameTime(string gameTime)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            GameTime.Text = gameTime;
        });
    }

    public void SetCommandOneName(string name)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            CommandNameOne.Text = name;
        });
    }

    public void SetCommandOneScore(string score)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            ScoreCommandOne.Text = score;
        });
    }

    public void SetCommandOneDeleting(string deleting, string time)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            DeletingCommandOne.Text = deleting;
            TimeDeletingCommandOne.Text = time;
        });
    }

    public void SetCommandTwoName(string name)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            CommandNameTwo.Text = name;
        });
    }

    public void SetCommandTwoScore(string score)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            ScoreCommandTwo.Text = score;
        });
    }

    public void SetAllLabels(ClientServerMessage mess) {
        Dispatcher.UIThread.InvokeAsync(() => {
            if (mess.period.Pause)
            {
                Pause.IsVisible = true;
            }
            else
            {
                Pause.IsVisible = false;
            }
            //Console.WriteLine(mess);
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(mess.Time).DateTime;
            GameTime.Text = $"{dateTime:t}";
            CommandNameOne.Text = mess.CommandOne.Name;
            CommandNameTwo.Text = mess.CommandTwo.Name;
            ScoreCommandOne.Text = mess.CommandOne.Score.ToString();
            ScoreCommandTwo.Text = mess.CommandTwo.Score.ToString();
            Period.Text = mess.period.Count?.ToString();
            DateTime periodTime = DateTimeOffset.FromUnixTimeSeconds(mess.period.TimeInPeriod).DateTime;
            PeriodTime.Text = $"{periodTime:t}";
        });
    }

    public void SetCommandTwoDeleting(string deleting, string time)
    {
        Dispatcher.UIThread.InvokeAsync(() => {
            DeletingCommandTwo.Text = deleting;
            TimeDeletingCommandTwo.Text = time;
        });
    }

    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        Pause.IsVisible = false;
    }
}