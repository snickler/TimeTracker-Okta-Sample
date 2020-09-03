using Okta.Sdk;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TimeTracker.UWP.Core.Models;

namespace TimeTracker.WebApi.Services
{
    public interface IWorkItemService
    {
        public Task<WorkItem> CreateWorkItemAsync(string uid, string itemTitle);
        public Task<IEnumerable<WorkItem>> GetWorkItemsAsync(string uid);
        public Task<WorkItem> UpdateWorkItemAsync(string uid, int itemId, WorkItem workItem);
        public Task<WorkItem> DeleteWorkItemAsync(string uid, int itemId);
    }
}
