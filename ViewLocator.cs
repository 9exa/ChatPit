using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;


namespace ChatPit
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;
        public IControl Build(object data)
        {
            var ogName = data.GetType().Name;
            switch (ogName)
            {
                case "MessagesViewModel":
                    Console.WriteLine(ogName);
                    Console.WriteLine(((MessagesViewModel)data).Messages[0].DisplayName);

                    return new Messages() { DataContext = data };
                case "ChatViewModel":
                    return new Chat() { DataContext = data };
                case "SignInPageViewModel":
                    return new SignInPage() { DataContext = data };
            }
            return new TextBlock { Text = "No view found for: " + ogName };
        }
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
