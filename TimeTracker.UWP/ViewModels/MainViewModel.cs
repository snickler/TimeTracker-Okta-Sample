using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Okta.Xamarin;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using TimeTracker.UWP.Core.Models;
using TimeTracker.UWP.Core.Services;

using Windows.UI.Xaml;

namespace TimeTracker.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly IOidcClient _oidcClient;
        private readonly HttpDataService _httpDataService;
        public MainViewModel(IOidcClient oidcClient, HttpDataService httpDataService)
        {
            _oidcClient = oidcClient;
            _httpDataService = httpDataService;
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 1)
            };
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if(e.Parameter is WorkItem wi)
            {
                WorkItem = wi;
            }
            base.OnNavigatedTo(e, viewModelState);

        }
        private WorkItem _workItem;
        public WorkItem WorkItem { get => _workItem; set => SetProperty(ref _workItem, value); }
        private DateTime _begin;
        public DateTime Begin { get => _begin; set => SetProperty(ref _begin, value); }

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private int _workItemNum;
        public int WorkItemNum { get => _workItemNum; set => SetProperty(ref _workItemNum, value); }

        private TimeSpan _ellapsed;
        public TimeSpan Ellapsed { get => _ellapsed; set => SetProperty(ref _ellapsed, value); }

        private bool _isTimerActive;
        public bool IsTimerActive { get => _isTimerActive; set => SetProperty(ref _isTimerActive, value); }

       
        private async Task StartTimerAsync()
        {

            Title = WorkItem.Title;
            _dispatcherTimer.Tick += Dt_Tick;
            _dispatcherTimer.Start();

            Begin = DateTime.Now;
            IsTimerActive = true;

        }

        private async Task StopTimerAsync()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= Dt_Tick;

            double finTime = (DateTime.Now - Begin).TotalHours;
            IsTimerActive = false;

            WorkItem.Completed = finTime;

            var workItemUpdated = await _httpDataService.PutAsJsonAsync("api/workItem", WorkItem);
            

        }
        public DelegateCommand ToggleTimerCommand => new DelegateCommand(async () =>
        {
            if (!IsTimerActive)
                await StartTimerAsync();
            else
                await StopTimerAsync();
        });



        private void Dt_Tick(object sender, object e)
        {
            Ellapsed = DateTime.Now - Begin;
        }
    }
}
