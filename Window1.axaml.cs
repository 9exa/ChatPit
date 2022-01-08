using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Diagnostics;
using ReactiveUI;



namespace ChatPit
{
    /// <summary>
    /// Main window view. Parent for either the login page or the chat page
    /// </summary>
    public class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

#if DEBUG
            this.AttachDevTools();
#endif

        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
    /// <summary>
    /// This project uses the MVVM approach. The MainWindowViewModel manages interaction
    /// with the login page and passes that info into the chat app
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {

        ChatViewModel chat;
        SignInPageViewModel signIn;
        ViewModelBase content;
        public ChatViewModel Chat { get { return chat; } private set { chat = value; } }
        public SignInPageViewModel SignIn { get { return signIn; } private set { signIn = value; } }        
        public ViewModelBase Content 
        { 
            get {return content; } 
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public MainWindowViewModel()
        {
            signIn = new SignInPageViewModel();
            chat = null;
            //Register a successful login event
            signIn.LoginSuccsess += OnLoginSuccess;
            //Display the signinpage
            Content = signIn;
        }
        /// <summary>
        /// Switch an instance of the main chat panel after a successful login
        /// </summary>
        /// <param name="uid"> The user id that the user is loging in as</param>
        /// <param name="connString">Connection string used to connect to database</param>
        public void OnLoginSuccess(int uid, string connString)
        {
            Console.WriteLine("Login Recieved");
            Chat = new ChatViewModel(uid, connString);
            Content = chat;
        }


    }
}
