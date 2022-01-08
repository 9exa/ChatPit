using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace ChatPit
{
    public class App : Application
    {
        /// <summary>
        /// Basic avalonia app environment. Copied it from the docs
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Window1();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
