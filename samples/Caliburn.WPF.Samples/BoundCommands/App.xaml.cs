﻿namespace BoundCommands
{
    using System.Windows;
    using Caliburn.Core;
    using Caliburn.PresentationFramework;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CaliburnFramework
                .ConfigureCore()
                .WithPresentationFramework()
                .Start();
        }
    }
}