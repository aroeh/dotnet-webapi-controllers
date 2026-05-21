using Demo.Restuarants.API.Models;
using Demo.Restuarants.API.SDK;
using Demo.Restuarants.Client.MVC.Helpers;
using Demo.Restuarants.Client.MVC.Models;
using Demo.Restuarants.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.Client.MVC.Controllers;

[Route("restuarants/{restuarantId}/business-hours")]
public class BusinessHoursController(IRestuarantsApi api) : Controller
{
    private readonly IRestuarantsApi _api = api;

    [HttpGet("")]
    public async Task<IActionResult> Index(string restuarantId)
    {
        try
        {
            ViewBag.RestuarantId = restuarantId;
            ViewBag.RestuarantName = (await _api.GetRestuarantAsync(restuarantId))?.Name;
            var hours = await _api.ListBusinessHoursAsync(restuarantId);
            return View(hours);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction("Details", "Restuarants", new { id = restuarantId });
        }
    }

    [HttpGet("create")]
    public IActionResult Create(string restuarantId) => View(new BusinessHourFormViewModel
    {
        RestuarantId = restuarantId,
        DayOfWeek = DayOfWeek.Monday,
        OpenTime = new TimeOnly(9, 0),
        CloseTime = new TimeOnly(17, 0)
    });

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string restuarantId, BusinessHourFormViewModel model)
    {
        if (!string.Equals(restuarantId, model.RestuarantId, StringComparison.Ordinal))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new CreateBusinessHourRequest
            {
                DayOfWeek = model.DayOfWeek,
                OpenTime = model.OpenTime,
                CloseTime = model.CloseTime
            };

            await _api.CreateBusinessHourAsync(restuarantId, request);
            TempData["Success"] = "Business hours entry was added.";
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ApiErrorHelper.GetMessage(ex));
            return View(model);
        }
    }

    [HttpGet("{businessHourId}/edit")]
    public async Task<IActionResult> Edit(string restuarantId, string businessHourId)
    {
        try
        {
            var hour = await _api.GetBusinessHourAsync(restuarantId, businessHourId);
            if (hour is null)
            {
                return NotFound();
            }

            return View(ToFormModel(restuarantId, hour));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
    }

    [HttpPost("{businessHourId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string restuarantId, string businessHourId, BusinessHourFormViewModel model)
    {
        if (!string.Equals(restuarantId, model.RestuarantId, StringComparison.Ordinal)
            || !string.Equals(businessHourId, model.BusinessHourId, StringComparison.Ordinal))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new UpdateBusinessHourRequest
            {
                DayOfWeek = model.DayOfWeek,
                OpenTime = model.OpenTime,
                CloseTime = model.CloseTime
            };

            var result = await _api.UpdateBusinessHourAsync(restuarantId, businessHourId, request);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["Success"] = "Business hours entry was updated.";
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ApiErrorHelper.GetMessage(ex));
            return View(model);
        }
    }

    [HttpGet("{businessHourId}/delete")]
    public async Task<IActionResult> Delete(string restuarantId, string businessHourId)
    {
        try
        {
            var hour = await _api.GetBusinessHourAsync(restuarantId, businessHourId);
            if (hour is null)
            {
                return NotFound();
            }

            ViewBag.RestuarantId = restuarantId;
            return View(hour);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
    }

    [HttpPost("{businessHourId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string restuarantId, string businessHourId)
    {
        try
        {
            var result = await _api.RemoveBusinessHourAsync(restuarantId, businessHourId);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Delete), new { restuarantId, businessHourId });
            }

            TempData["Success"] = "Business hours entry was removed.";
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index), new { restuarantId });
        }
    }

    private static BusinessHourFormViewModel ToFormModel(string restuarantId, RestuarantBusinessHourBO hour) => new()
    {
        RestuarantId = restuarantId,
        BusinessHourId = hour.Id,
        DayOfWeek = hour.DayOfWeek,
        OpenTime = hour.OpenTime,
        CloseTime = hour.CloseTime
    };
}
