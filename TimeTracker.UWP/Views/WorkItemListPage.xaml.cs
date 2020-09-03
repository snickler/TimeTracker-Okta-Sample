using System;

using TimeTracker.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TimeTracker.UWP.Views
{
    public sealed partial class WorkItemListPage : Page
    {
        private WorkItemListViewModel ViewModel => DataContext as WorkItemListViewModel;

        public WorkItemListPage()
        {
            InitializeComponent();
        }


    }
}
