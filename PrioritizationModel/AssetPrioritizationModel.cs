using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using PrioritizationService.DTOs;
using WebAPI.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace PrioritizationModel;

internal class AssetPrioritizationModel : IPrioritizationModel
{
    public IEnumerable<AssetDTO> GetPrioritizedAssets(IEnumerable<AssetDTO> assets, decimal threshold)
    {
        decimal currentQuantityMw = 0;
        IEnumerable<AssetDTO> optimalSelection = new List<AssetDTO>();

        foreach(AssetDTO asset in assets)
        {
            // If CapacityMw is less than what's needed to meet the threshold, add 100% of it
            if (currentQuantityMw + asset.CapacityMw <= threshold)
            {
                // Creates an iterator block in a method. When you use yield return, you can return a sequence of values one at a time, rather than all at once.
                asset.RegulationPercentage = 100;
                yield return asset;
                currentQuantityMw += asset.CapacityMw;
            }
            // Otherwise, calculate how many percent of the CapacityMw is needed to match the exact threshold and break out of the loop
            else
            {
                asset.RegulationPercentage = (threshold - currentQuantityMw) / asset.CapacityMw * 100;
                yield return asset;
                break;
            }
        }
    }
}
