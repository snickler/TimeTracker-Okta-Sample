using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TimeTracker.UWP.Core.Models;
using TimeTracker.WebApi.Services;

namespace TimeTracker.UWP.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        /// <summary>
        /// Retrieves Work Items for the current autenticated user
        /// </summary>
        /// <returns>Returns List of WorkItems</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkItem>>> List()
        {
            var userId = User.FindFirstValue("uid");
            var workItems = await _workItemService.GetWorkItemsAsync(userId);

            return Ok(workItems);
        }

        /// <summary>
        /// Retrieves an individual Work Item by the Id entered
        /// </summary>
        /// <param name="id">The Work Item Id</param>
        /// <returns>Returns the Work Item</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkItem>> GetWorkItem(int id)
        {
            var userId = User.FindFirstValue("uid");
            var workItems = await _workItemService.GetWorkItemsAsync(userId);
            var workItem = workItems.FirstOrDefault(item => item.Id == id);

            if (workItem is null)
            {
                return NotFound();
            }

            return workItem;
        }

        /// <summary>
        /// Creates a new Work Item
        /// </summary>
        /// <param name="item">The Work Item</param>
        /// <returns>Returns the newly created Work Item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkItem>> Create([FromBody] WorkItem item)
        {
            var userId = User.FindFirstValue("uid");
            var workItem = await _workItemService.CreateWorkItemAsync(userId, item.Title);

            return CreatedAtAction(nameof(GetWorkItem), new { workItem.Id }, workItem);
        }

        /// <summary>
        /// Modifies an existing Work Item
        /// </summary>
        /// <param name="item">The Work Item</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit([FromBody] WorkItem item)
        {
            var userId = User.FindFirstValue("uid");
            var workItems = await _workItemService.GetWorkItemsAsync(userId);

            if (await _workItemService.UpdateWorkItemAsync(userId, item.Id, item) is null)
                return BadRequest("Error Updating Item");

            return NoContent();

        }
        /// <summary>
        /// Deletes an individual Work Item by the Id entered
        /// </summary>
        /// <param name="id">The Work Item Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue("uid");
            var item = await _workItemService.DeleteWorkItemAsync(userId, id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
