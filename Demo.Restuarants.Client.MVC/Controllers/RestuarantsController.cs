using Demo.Restuarants.API.SDK;
using Demo.Restuarants.Client.MVC.Helpers;
using Demo.Restuarants.Client.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.Client.MVC.Controllers;

public class RestuarantsController(IRestuarantsApi api) : Controller
{
    private readonly IRestuarantsApi _api = api;

    public async Task<IActionResult> Index(RestuarantListViewModel model)
    {
        try
        {
            model.Results = await _api.QueryRestuarantsAsync(model.ToQueryParameters());
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return View(model);
        }
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        try
        {
            var restuarant = await _api.GetRestuarantAsync(id);
            if (restuarant is null)
            {
                return NotFound();
            }

            return View(restuarant);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create() => View(new RestuarantFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RestuarantFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var created = await _api.CreateRestuarantAsync(model.ToCreateRequest());
            TempData["Success"] = $"Restaurant \"{created.Name}\" was created.";
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ApiErrorHelper.GetMessage(ex));
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        try
        {
            var restuarant = await _api.GetRestuarantAsync(id);
            if (restuarant is null)
            {
                return NotFound();
            }

            return View(RestuarantFormViewModel.FromRestuarant(restuarant));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, RestuarantFormViewModel model)
    {
        if (!string.Equals(id, model.Id, StringComparison.Ordinal))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var result = await _api.UpdateRestuarantAsync(id, model.ToUpdateRequest());
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["Success"] = "Restaurant was updated.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ApiErrorHelper.GetMessage(ex));
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        try
        {
            var restuarant = await _api.GetRestuarantAsync(id);
            if (restuarant is null)
            {
                return NotFound();
            }

            return View(restuarant);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            await _api.RemoveRestuarantAsync(id);
            TempData["Success"] = "Restaurant was removed.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ApiErrorHelper.GetMessage(ex);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
