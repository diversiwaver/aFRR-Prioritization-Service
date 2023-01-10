using aFRRService.DTOs.DTOConverters;
using Microsoft.AspNetCore.Mvc;
using PrioritizationModel;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrioritizationsController : ControllerBase
{
    private readonly ILogger<PrioritizationsController> _logger;
    private readonly PrioritizationModelController _prioritizationModelController;

    public PrioritizationsController(ILogger<PrioritizationsController> logger, PrioritizationModelController prioritizationController)
    {
        _logger = logger;
        _prioritizationModelController = prioritizationController;
    }

    [HttpGet]
    [Route("AssetRegulations")]
    public ActionResult<aFRRService.DTOs.SignalDTO> GetAssetRegulations(aFRRService.DTOs.SignalDTO signalDTO)
    {
        _logger.LogInformation("GetAssetRegulations method called for signalDto: {signalDTO}", signalDTO);
        PrioritizationService.DTOs.SignalDTO signal = DTOConverter<aFRRService.DTOs.SignalDTO, PrioritizationService.DTOs.SignalDTO>.From(signalDTO);
        _logger.LogInformation("Converted DTO to signal");
        signal = _prioritizationModelController.GetAssetRegulationsAsync(signal);
        _logger.LogInformation("Prioritized {Count} assets to regulate for Signal Id: {Id}", signal.AssetsToRegulate.Count(), signal.Id);
        signalDTO.AssetsToRegulate = DTOConverter<PrioritizationService.DTOs.AssetDTO, aFRRService.DTOs.AssetDTO>.FromList(signal.AssetsToRegulate);
        _logger.LogInformation("Converted signal to DTO");
        return Ok(signalDTO);
    }
}
