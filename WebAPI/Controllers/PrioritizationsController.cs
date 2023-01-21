using aFRRService.DTOs.DTOConverters;
using Microsoft.AspNetCore.Mvc;
using PrioritizationModel;
using SignalDTOPrioritization = PrioritizationService.DTOs.SignalDTO;
using SignalDTOaFRR = aFRRService.DTOs.SignalDTO;
using AssetDTOPrioritization = PrioritizationService.DTOs.AssetDTO;
using AssetDTOaFRR = aFRRService.DTOs.AssetDTO;


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

        SignalDTOPrioritization signal = DTOConverter<SignalDTOaFRR, SignalDTOPrioritization>.From(signalDTO);
        _logger.LogInformation("Converted DTO to signal");

        signal = _prioritizationModelController.GetAssetRegulationsAsync(signal);
        _logger.LogInformation("Prioritized {Count} assets to regulate for Signal Id: {Id}", signal.AssetsToRegulate.Count(), signal.Id);

        signalDTO.AssetsToRegulate = DTOConverter<AssetDTOPrioritization, AssetDTOaFRR>.FromList(signal.AssetsToRegulate);
        _logger.LogInformation("Converted signal to DTO");

        return Ok(signalDTO);
    }
}
