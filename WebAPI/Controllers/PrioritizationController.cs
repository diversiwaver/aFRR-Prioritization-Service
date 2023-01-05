using aFRRService.DTOs.DTOConverters;
using Microsoft.AspNetCore.Mvc;
using PrioritizationModel;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrioritizationController : ControllerBase
{
    private readonly ILogger<PrioritizationController> _logger;
    private readonly PrioritizationModelController _prioritizationModelController;

    public PrioritizationController(ILogger<PrioritizationController> logger, PrioritizationModelController prioritizationController)
    {
        _logger = logger;
        _prioritizationModelController = prioritizationController;
    }

    [HttpGet]
    public async Task<ActionResult<aFRRService.DTOs.SignalDTO>> GetAssetRegulations(aFRRService.DTOs.SignalDTO signalDto)
    {
        _logger.LogInformation("GetAssetRegulations method called for signalDto: {signalDto}", signalDto);
        PrioritizationService.DTOs.SignalDTO signal = DTOConverter<aFRRService.DTOs.SignalDTO, PrioritizationService.DTOs.SignalDTO>.From(signalDto);
        _logger.LogInformation("Converted DTO to signal");
        signal = _prioritizationModelController.GetAssetRegulationsAsync(signal);
        _logger.LogInformation("Prioritized {Count} assets to regulate for Signal Id: {Id}", signal.AssetsToRegulate.Count(), signal.Id);
        signalDto.AssetsToRegulate = DTOConverter<PrioritizationService.DTOs.AssetDTO, aFRRService.DTOs.AssetDTO>.FromList(signal.AssetsToRegulate);
        _logger.LogInformation("Converted signal to DTO");
        return Ok(signalDto);
    }
}
