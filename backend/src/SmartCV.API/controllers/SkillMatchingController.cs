using API.dtos.Matching;
using API.services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillMatchingController : ControllerBase
{
    private readonly SkillMatchingService _matchingService;

    public SkillMatchingController(
        SkillMatchingService matchingService)
    {
        _matchingService = matchingService;
    }

    [HttpPost]
    public async Task<IActionResult> CompareSkills(
        [FromBody] JobDescriptionDto request)
    {
        var result = await _matchingService.CompareAsync(
            request.ProfilId,
            request.JobDescription);

        return Ok(result);
    }
}