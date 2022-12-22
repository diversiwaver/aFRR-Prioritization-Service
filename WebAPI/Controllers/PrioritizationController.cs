using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrioritizationController : ControllerBase
{
    private readonly ILogger<PrioritizationController> _logger;
    private readonly IAssetDataAccess _assetDataAccess;

    public PrioritizationController(ILogger<PrioritizationController> logger, IAssetDataAccess assetDataAccess)
    {
        _logger = logger;
        _assetDataAccess = assetDataAccess;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SignalDto>>> GetAssetRegulations(IEnumerable<SignalDto> signalDtos)
    {
        _logger.LogInformation("GetAssetRegulations method called with IEnumerable of signalDtos: {signalDtos}", signalDtos);
        return null;
    }
}
