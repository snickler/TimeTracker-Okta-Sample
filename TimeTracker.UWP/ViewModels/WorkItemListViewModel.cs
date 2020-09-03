using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Okta.Xamarin;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using TimeTracker.UWP.Core.Helpers;
using TimeTracker.UWP.Core.Models;
using TimeTracker.UWP.Core.Services;
using TimeTracker.UWP.Models;

using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TimeTracker.UWP.ViewModels
{
    public class WorkItemListViewModel : ViewModelBase
    {
        private readonly HttpDataService _httpDataService;
        private readonly IOidcClient _oidcClient;
        private readonly AuthInfo _authInfo;
        private readonly INavigationService _navigationService;
        public WorkItemListViewModel(HttpDataService httpDataService, IOidcClient oidcClient, AuthInfo authInfo, INavigationService navigationService)
        {
            _oidcClient = oidcClient;
            _httpDataService = httpDataService;
            _authInfo = authInfo;
            _navigationService = navigationService;
        }

       
        public ObservableCollection<WorkItem> WorkItems { get; private set; } = new ObservableCollection<WorkItem>();

        public void OnItemClick(object sender, TappedRoutedEventArgs e)
        {
            _navigationService.Navigate(PageTokens.MainPage, (e.OriginalSource as FrameworkElement).DataContext);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if(!_authInfo.State.IsAuthenticated)
                _authInfo.State = await _oidcClient.SignInWithBrowserAsync();

            var items = await _httpDataService.GetAsync<IEnumerable<WorkItem>>("api/workitem", _authInfo.State.AccessToken);
            WorkItems.Clear();

            foreach (var item in items)
                WorkItems.Add(item);

            base.OnNavigatedTo(e, viewModelState);

           
        }


    }
}
