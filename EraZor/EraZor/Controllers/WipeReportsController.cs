﻿using EraZor.Interfaces;
using EraZor.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class WipeReportsController : ControllerBase
{
    private readonly IWipeReportService _wipeReportService;
    private readonly UserManager<IdentityUser> _userManager;

    public WipeReportsController(IWipeReportService wipeReportService, UserManager<IdentityUser> userManager)
    {
        _wipeReportService = wipeReportService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWipeReports()
    {
        var reports = await _wipeReportService.GetWipeReportsAsync();
        return Ok(reports);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWipeReport([FromBody] WipeReportCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid data.");

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var success = await _wipeReportService.CreateWipeReportAsync(dto, user.Id);
        if (!success) return BadRequest("Failed to create wipe report.");

        return Ok(new { message = "Wipe report created successfully." });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWipeReport(int id)
    {
        var success = await _wipeReportService.DeleteWipeReportAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}




