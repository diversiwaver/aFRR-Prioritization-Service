using PrioritizationService.DTOs;

namespace PrioritizationModel;

public interface IPrioritizationModel
{
    IEnumerable<AssetDTO> GetPrioritizedAssets(IEnumerable<AssetDTO> assets, decimal threshold);
}
