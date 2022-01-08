using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Data.SqlClient;
using ReactiveUI;

namespace ChatPit
{
    /// <summary>
    /// Control that contains username and password fields for signing in
    /// </summary>
    public partial class SignInPage : UserControl
    {
        public SignInPage()
        {
            InitializeComponent();
            DataContext = new SignInPageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    /// <summary>
    /// Delegate for defining events for passing an userdata and server connection in after successful login
    /// </summary>
    /// <param name="value"></param>
    public delegate void PassLoginRequst(int uid, string connectionString);

    /// <summary>
    /// ViewModel for the Sign-in page. Asks for username, password., querys server to check it's validity and passes a successful userlogin
    /// to window parent
    /// </summary>
    public class SignInPageViewModel : ViewModelBase
    {
        public SignInPageViewModel()
        {
            dbLoc = GetPathAboveBin() + "\\Database1.mdf";
        }

        string username = "SadBoi"; //= string.Empty;
        string password = "12wan7firends"; //= string.Empty;
        string dbLoc;
        /// <summary> The name that the user logs in with. Not necissarily the display name </summary>
        public string Username { get { return username; } set { username = value; } }
        /// <summary> Key user needs to login with. displayed as series * for privacy sake </summary>
        public string Password { get { return password; } set { password = value; } }
        /// <summary>Local file location of the database file</summary>
        public string DBLoc { get { return dbLoc; } set { dbLoc = value; } }

        //Error Handleing
        string errorString = "Error: ";
        bool errorVisible = false;
        ///<summary>Content of error box</summary>
        public string ErrorString 
        { 
            get { return errorString; } 
            private set
            {
                this.RaiseAndSetIfChanged(ref errorString, value);
            } 
        }
        ///<summary>Whether the error box is visible</summary>
        public bool ErrorVisible 
        { 
            get { return errorVisible; } 
            private set 
            { 
                this.RaiseAndSetIfChanged(ref errorVisible, value); 
            } 
        }
        /// <summary>
        /// Causes the error box to appear on screen
        /// </summary>
        /// <param name="The text to be displayed in the error box"></param>
        private void pingErrorBox(string error)
        {
            ErrorVisible = true;
            ErrorString = "Error: " + error;
            
        }
        /// <summary>
        /// Event emmited when a successful login request has been made
        /// </summary>
        public event PassLoginRequst LoginSuccsess;
        ///Emits Login Event
        protected virtual void OnLoginSuccess(int uid, string connString)
        {
            Console.WriteLine("Login Success!!!!");
            LoginSuccsess?.Invoke(uid, connString);
        }

        /// <summary>
        ///Processes a login request from current field entries by checking if connection is valid and username-password match
        /// </summary>
        public void OnSignInPressed()
        {

            //We know the request is invalid if either the username or password fields are empty
            if (username == string.Empty)
            {
                pingErrorBox("We need a username here");
                return;
            }
            if (password == string.Empty)
            {
                pingErrorBox("There's no passwords what's been entered thar'");
                return;
            }

            //Check if sql connection can be made
            //We'll close it at the end of the method. Afterall it's too dangerous to just have an sql connection oper
            //in several peices of code
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;\n" +
                                      $"AttachDbFilename={dbLoc};\n" +
                                      "Connect Timeout=15;\n" +
                                      "Integrated Security=True;";
            SqlConnection cnn;
            try
            {
                cnn = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                pingErrorBox("Could not connect to server");
                return;
            }


            //Now try to see that reading from database does not result in an error
            string query = "SELECT userid, uname, pass\n" +
                            "FROM LoginData\n" +
                            $"WHERE uname=\'{username}\'";
            using (var comm = new SqlCommand(query, cnn))
            {

                try
                {
                    cnn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    //Bring up error and exit if the username does not exist in the database
                    if (!reader.HasRows)
                    {
                        pingErrorBox("Username not recognised");
                        reader.Close();
                        cnn.Close();
                        return;
                    }
                    //Check if password matches password in database
                    //since Username is defined to be UNIQUE, we know there is only one entry
                    reader.Read();
                    if (reader.GetString(2) != password)
                    {
                        pingErrorBox("Password does not match");
                        reader.Close();
                        cnn.Close();
                        return;
                    }

                    //Login Success. Invoke that event so it can be recieved by the Window or whoever
                    int uid = reader.GetInt32(0);
                    reader.Close();
                    cnn.Close();
                    OnLoginSuccess(uid, connectionString);
                }
                catch (Exception ex)
                {
                    if (ex is SqlException)
                    {
                        SqlException exs = (SqlException)ex;
                        for (int i = 0; i < exs.Errors.Count; i++)
                        {
                            string errorMessage = ("Index #" + i + "\n" +
                                "Message: " + exs.Errors[i].Message + "\n" +
                                "LineNumber: " + exs.Errors[i].LineNumber + "\n" +
                                "Source: " + exs.Errors[i].Source + "\n" +
                                "Procedure: " + exs.Errors[i].Procedure + "\n");
                            Console.Write(errorMessage);
                        }
                    }
                    else 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    pingErrorBox("Problem with reading");
                    cnn.Close();
                }
            }
        }

        
        //Path to dierctory containing the bin directory. We assume this is where the dbfile is
        private string GetPathAboveBin()
        {
            string wd = Directory.GetCurrentDirectory();
            string[] dirs = wd.Split(new char[] { '\\', '/' });
            for (int i = dirs.Length - 1; i >= 0; i--)
            {
                if (dirs[i] == "bin")
                {
                    return string.Join("\\", new ArraySegment<string>(dirs, 0, i));
                }
            }
            return wd;
        }
    }
}
