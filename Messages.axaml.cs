using System.ComponentModel;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChatPit
{
    /// <summary>
    /// Control that contains the messages in the chatlog as a list
    /// </summary>
    public partial class Messages : UserControl
    {
        public Messages()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class MessagesViewModel : ViewModelBase
    {
        public ObservableCollection<MessageItem> Messages { get; } // need get; to expose it to binding
        /// <summary>
        /// Creates a Messages instance with a given list of message items as the list
        /// </summary>
        /// <param name="messages">Messages to be displayed</param>
        public MessagesViewModel(IEnumerable<MessageItem> messages)
        {
            Messages = new ObservableCollection<MessageItem>(messages);
        }
        //Default constructor for testing out editor
        private MessagesViewModel()
        {
            Messages = new ObservableCollection<MessageItem> (new[] {
                new MessageItem("Test Test", 0, DateTime.UtcNow, "Tis a test m8.")
            });
        }

    }
    /// <summary>
    /// Class that stores information held by a message
    /// </summary>
    public class MessageItem
    {
        public MessageItem(string name, int tag, DateTime sentTime,  string content)
        {
            DisplayName = name;
            Tag = tag;
            SentTime = sentTime;
            Content = content;
            
        }
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public int Tag { get; set; }  
        ///<remarks>Translates a tag type into it's coerresponding colour string</remarks>
        public string TagColor
        {
            get
            {
                switch (Tag)
                {
                    case 0:
                        return "Blue";
                        break;
                    case 1:
                        return "Yellow";
                        break;
                    default:
                        return "White";
                }
            }
        }
        public DateTime SentTime { get; set; }
        /// <remarks>Because a textbox cannot print a date time on it's own</remarks>
        public string SentTimeString
        {
            get { return SentTime.ToString(); }
        }
        public string Content { get; set; }
    }
}