// See https://aka.ms/new-console-template for more information
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;


namespace ChatPit
{    
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            Console.WriteLine(1);

        }
        private static void AppMain(Application app, string[] args)
        {
            //app.Run(new Window1());
            

        }

        /// <summary>
        /// Looks Upwards throw file system to find the bin folder.
        /// It makes sense to have this function here as it is basically a global, neccisary for
        /// rest of the program to run
        /// </summary>
        
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}