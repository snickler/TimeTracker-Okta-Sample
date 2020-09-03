using System;

using TimeTracker.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TimeTracker.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
