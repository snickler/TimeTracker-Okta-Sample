using Okta.Xamarin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UWP.Models
{
    public class AuthInfo
    {
        public StateManager State { get; set; } = new StateManager();
    }
}
