using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace ChatPit
{
    ///<summary>Main Control of the application. Displays previous messages and input chatbox </summary>
    public partial class Chat : UserControl
    {
        public Chat()
        {
            InitializeComponent();
            DataContext = new ChatViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    /// <summary>
    /// <summary>   ViewModel for chat contains viewmodels for messages and chatbox</summary>
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        //helper class that stores userdata
        struct UserData
        {
            public int id;
            public string dispName;
        }

        /// <summary>
        /// Text user is typing in to send when they're done
        /// </summary>
        string boxText = "";

        private UserData user;

        ///<remarks>User data for default construction or failstate instances of Chat</remarks>
        const int DefaultUserID = 27;
        const string DefaultUserName = "Joe Swanson";
        /// <summary>
        /// Creates a ChatViewModel. Uses arguments to prepare Server Connection and userdata.
        /// </summary>
        /// <param name="uid">Userid of this session's user</param>
        /// <param name="connString">String that allows program to connect to server from query</param>
        public ChatViewModel(int uid, string connString)
        {
            connectionString = connString;
            //prepare userdate
            user = GetUserData(uid, connString);
            //display the existing messages
            SyncMessagesToDataBase();
        }
        // default constructor for debug
        public ChatViewModel()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                 AttachDbFilename=C:\Users\MainUser\source\repos\ChatPit\ChatPit\Database1.mdf;
                                 Integrated Security=True";
            user = new UserData() { id = DefaultUserID, dispName = DefaultUserName };
            SyncMessagesToDataBase();
        }
        ///<summary>String that connects to database for query. That database is hence known as the 'Connected Database'</summary>
        string connectionString;

        /// <summary>
        /// Gets the username and any other relevant info from the user with the given username from
        /// the database accessed from the given connection string
        /// </summary>
        private UserData GetUserData(int uid, string connString)
        {
            string queryString = "SELECT dispName\n" +
                                 $"FROM Person WHERE id={uid}";
            string dispName;
            try
            {
                using (SqlConnection cnn = new SqlConnection(connString))
                using (SqlCommand comm = new SqlCommand(queryString, cnn))
                {
                    cnn.Open();
                    using(SqlDataReader reader = comm.ExecuteReader())
                    {
                        //uid is a primary key so there i exactly one instance
                        reader.Read();
                        dispName = reader.GetString(0);
                    }
                }
                return new UserData() { id = uid, dispName = dispName };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new UserData() { id = DefaultUserID, dispName = DefaultUserName }; 

            }
        }

        /// <summary>
        /// Display the existing messages in the database accessed by *connectionString* property
        /// </summary>
        public void SyncMessagesToDataBase()
        {


            string queryString = "SELECT dispName, caste as \"type\", content, sentTime\n" +
                                  "FROM Person JOIN ChatMessage ON Person.id = ChatMessage.sender" +
                                  ";";
            //queryString = "SELECT * FROM information_schema.tables; ";
            //queryString = "SELECT * FROM ChatMessage;";
            ObservableCollection<MessageItem> messages = new ObservableCollection<MessageItem>();

            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand comm = new SqlCommand(queryString, cnn);
                    cnn.Open();
                    Console.WriteLine("Connection Open!");

                    SqlDataReader reader = comm.ExecuteReader();
                    //Console.WriteLine(reader.Read());
                    while (reader.Read())
                    {
                        messages.Add(new MessageItem(reader["dispName"].ToString(), (int)reader["type"],
                            (DateTime)reader["sentTime"], reader["content"].ToString()));
                    }
                    Messages = new MessagesViewModel(messages);
                    reader.Close();
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        /// <summary>
        /// Pushes a new message written by user to the attached database
        /// </summary>
        /// <param name="messageItem"></param>
        public void addMessageToDataBase(MessageItem messageItem)
        {
            //duplicate quotations so sql can read them
            string makeSQLSafe(string s) => s.Replace("\"", "\"\"").Replace("\'", "\'\'");
            string queryString = String.Format("Insert Into ChatMessage\n" +
                                               "Values ({0}, '{1}', '{2}');", user.id,
                                               messageItem.SentTime.ToString("s"), makeSQLSafe(messageItem.Content));
            Console.WriteLine(queryString);
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand comm = new SqlCommand(queryString, cnn))
            {
                try
                {

                    cnn.Open();
                    comm.ExecuteNonQuery();
                    Console.WriteLine("Message added to database!");
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        /// <summary>
        /// The messages list being displayed
        /// </summary>
        public MessagesViewModel Messages { get; set; }
        public string BoxText
        {
            get => boxText;
            set
            {
                //boxText = value;  
                this.RaiseAndSetIfChanged(ref boxText, value);
            }
        }

        //public void ButtonClicked() => ButtonText = "Hello, Avalonia!";
        public void ButtonClicked()
        {


            if (boxText != "")
            {
                MessageItem newMessage = new MessageItem(user.dispName, 0, DateTime.Now, boxText);
                Messages.Messages.Add(newMessage);
                BoxText = "";
                addMessageToDataBase(newMessage);
            }
            Console.WriteLine(boxText);
        }
    }
}

