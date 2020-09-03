using Okta.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using TimeTracker.UWP.Core.Models;

namespace TimeTracker.WebApi.Services
{
   
    public class WorkItemService : IWorkItemService
    {
        private readonly IOktaClient _oktaClient;
        public WorkItemService(IOktaClient oktaClient)
        {
            _oktaClient = oktaClient;
        }


        private Task<IUser> _getUserFromId(string uid) =>  _oktaClient.Users.GetUserAsync(uid);

        /// <summary>
        /// Retrieves the Work Items from the user's profile
        /// </summary>
        /// <param name="oktaUser">The user</param>
        /// <returns>Returns a list of WorkItem objects</returns>
        private IEnumerable<WorkItem> _getWorkItemsForUser(IUser oktaUser)
        {
            var workItemsJSON = oktaUser?.Profile.GetProperty<string>("workItems");

            // Create sample list of items if nothing exists in the profile
            if (string.IsNullOrEmpty(workItemsJSON))
            {
                var testItems = new List<WorkItem>()
                {
                    new WorkItem { Id = 1, Title = "Start Development of WebPage"},
                    new WorkItem { Id = 2, Title = "Create Banner"}
                };

                return testItems;
            }

           return JsonSerializer.Deserialize<IEnumerable<WorkItem>>(workItemsJSON) 
                ?? Enumerable.Empty<WorkItem>();
        }

        /// <summary>
        /// Saves Work Items to the user's profile
        /// </summary>
        /// <param name="oktaUser">The user</param>
        /// <param name="updatedWorkItems">The Updated Work Items</param>
        /// <returns></returns>
        private Task _saveWorkItemsForUser(IUser oktaUser, IEnumerable<WorkItem> updatedWorkItems)
        {
            
            oktaUser.Profile.SetProperty("workItems", JsonSerializer.Serialize(updatedWorkItems));
            return oktaUser.UpdateAsync();
        }

        /// <summary>
        /// Creates a Work Item for the user
        /// </summary>
        /// <param name="uid">The User Id</param>
        /// <param name="itemTitle">The Work Item Title</param>
        /// <returns>Return The new work item</returns>
        public async Task<WorkItem> CreateWorkItemAsync(string uid, string itemTitle)
        {
            if( await _getUserFromId(uid) is IUser user)
            {
                var workItems = _getWorkItemsForUser(user);

                workItems.Append(new WorkItem
                {
                    Id = workItems.Max(item=>item.Id),
                    Title = itemTitle
                });

                await _saveWorkItemsForUser(user, workItems);
            }
            return await Task.FromResult<WorkItem>(null);

        }

        /// <summary>
        /// Deletes a Work Item for the user
        /// </summary>
        /// <param name="uid">The User Id</param>
        /// <param name="itemId">The Work Item Id</param>
        /// <returns></returns>
        public async Task<WorkItem> DeleteWorkItemAsync(string uid, int itemId)
        {
            if(await _getUserFromId(uid) is IUser user)
            {
                var workItems = _getWorkItemsForUser(user).ToList();
                var workItem = workItems.FirstOrDefault(wi => wi.Id == itemId);

                if (workItem is null)
                    return await Task.FromResult<WorkItem>(null);

                workItems.Remove(workItem);

                await _saveWorkItemsForUser(user, workItems);

                return workItem;
            }

            return await Task.FromResult<WorkItem>(null);
        }

        /// <summary>
        /// Gets Work Items for the user
        /// </summary>
        /// <param name="uid">The User Id</param>
        /// <returns>Returns the list of Work Items</returns>
        public async Task<IEnumerable<WorkItem>> GetWorkItemsAsync(string uid)
        {
            if(await _getUserFromId(uid) is IUser user)
            {
                return _getWorkItemsForUser(user);
            }
            return Enumerable.Empty<WorkItem>();
        }

        /// <summary>
        /// Updates a Work Item for the user
        /// </summary>
        /// <param name="uid">The User Id</param>
        /// <param name="itemId">The Work Item Id</param>
        /// <param name="workItem">The modified Work Item</param>
        /// <returns></returns>
        public async Task<WorkItem> UpdateWorkItemAsync(string uid, int itemId, WorkItem workItem)
        {
           if(await _getUserFromId(uid) is IUser user)
            {
                var workItems  = _getWorkItemsForUser(user);
                
                var currentItem = workItems.FirstOrDefault(item => item.Id == itemId);

                if (currentItem == null)
                    return null;

                currentItem.Title = workItem.Title;
                currentItem.Completed = workItem.Completed;

                await _saveWorkItemsForUser(user, workItems);

                return currentItem;

            }
            return null;
        }
    }
}
