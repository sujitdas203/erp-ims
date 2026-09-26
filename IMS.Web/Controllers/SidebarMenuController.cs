using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Helpers.Constants;
using IMS.Models.Common.Navigation;
using IMS.Services.Interfaces;
using IMS.Web.Authorization;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class SidebarMenuController : Controller
    {
        private readonly ISidebarMenuService _sidebarMenuService;
        private readonly ILogger<SidebarMenuController> _logger;

        public SidebarMenuController(ISidebarMenuService sidebarMenuService, ILogger<SidebarMenuController> logger)
        {
            _sidebarMenuService = sidebarMenuService;
            _logger = logger;
        }

        [Permission(Permissions.ViewSidebarMenu)]
        public async Task<IActionResult> Index()
        {
            var items = await _sidebarMenuService.GetAllForAdminAsync();
            return View(items);
        }

        [HttpGet]
        [Permission(Permissions.ViewSidebarMenu)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _sidebarMenuService.GetByIdAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "Menu item not found." });
            }
            return Json(new { success = true, data = item });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.ManageSidebarMenu)]
        public async Task<IActionResult> Create([FromBody] SidebarMenuItemModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return Json(new { success = false, message = "Title is required." });
            }

            try
            {
                var newId = await _sidebarMenuService.CreateAsync(model);
                return Json(new { success = true, message = "Sidebar menu item created successfully!", id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sidebar menu item");
                return Json(new { success = false, message = "Failed to create menu item: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.ManageSidebarMenu)]
        public async Task<IActionResult> Edit([FromBody] SidebarMenuItemModel model)
        {
            if (model.Id <= 0 || string.IsNullOrWhiteSpace(model.Title))
            {
                return Json(new { success = false, message = "Invalid data or missing title." });
            }

            try
            {
                var success = await _sidebarMenuService.UpdateAsync(model);
                if (success)
                {
                    return Json(new { success = true, message = "Sidebar menu item updated successfully!" });
                }
                return Json(new { success = false, message = "Menu item not found or could not be updated." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sidebar menu item");
                return Json(new { success = false, message = "Failed to update menu item: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.ManageSidebarMenu)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _sidebarMenuService.DeleteAsync(id);
                if (success)
                {
                    return Json(new { success = true, message = "Sidebar menu item deleted successfully!" });
                }
                return Json(new { success = false, message = "Item not found or could not be deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sidebar menu item");
                return Json(new { success = false, message = "Failed to delete item: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.ManageSidebarMenu)]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var success = await _sidebarMenuService.ToggleStatusAsync(id);
                if (success)
                {
                    return Json(new { success = true, message = "Status updated successfully!" });
                }
                return Json(new { success = false, message = "Failed to toggle status." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling sidebar menu status");
                return Json(new { success = false, message = "Failed to toggle status: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Permissions.ManageSidebarMenu)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateModel model)
        {
            try
            {
                var success = await _sidebarMenuService.UpdateOrderAsync(model.Id, model.SectionOrder, model.MenuOrder);
                if (success)
                {
                    return Json(new { success = true, message = "Order updated successfully!" });
                }
                return Json(new { success = false, message = "Failed to update order." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order");
                return Json(new { success = false, message = "Failed to update order: " + ex.Message });
            }
        }
    }

    public class OrderUpdateModel
    {
        public int Id { get; set; }
        public int SectionOrder { get; set; }
        public int MenuOrder { get; set; }
    }
}
