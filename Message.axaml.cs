using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
/// <summary>
/// Unused
/// </summary>
namespace ChatPit
{
    public partial class Message : UserControl
    {
        public Message()
        {
            InitializeComponent();
            DataContext = new MessageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public static string SE() => "e";
    }
    public class MessageViewModel : INotifyPropertyChanged
    {
        string displayName = "User Name";
        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }

        string content = "No Text";
        public string Content
        {
            get => content;
            set
            {
                content = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void ButtonClicked() => DisplayName = "Hello, Avalonia!";
    }

}
