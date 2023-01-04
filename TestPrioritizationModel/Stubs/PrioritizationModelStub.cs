using PrioritizationModel;
using PrioritizationService.DTOs;

namespace TestPrioritizationModel.Stubs;

internal class PrioritizationModelStub : IPrioritizationModel
{
    public IEnumerable<AssetDTO> GetPrioritizedAssets(IEnumerable<AssetDTO> assets, decimal quantityThreshold)
    {
        return new List<AssetDTO> { 
            new AssetDTO() {Id = 1, CapacityMw = 10, RegulationPercentage = 100},
            new AssetDTO() {Id = 2, CapacityMw = 15, RegulationPercentage = 100},
            new AssetDTO() {Id = 3, CapacityMw = 20, RegulationPercentage = 50}
        };
    }
}
