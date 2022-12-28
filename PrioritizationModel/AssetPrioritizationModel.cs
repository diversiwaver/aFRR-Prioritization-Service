using PrioritizationService.DTOs;

namespace PrioritizationModel;

internal class AssetPrioritizationModel : IPrioritizationModel
{
    public IEnumerable<AssetDTO> GetPrioritizedAssets(IEnumerable<AssetDTO> assets, decimal threshold)
    {
        decimal totalCapacityMw = 0;
        HashSet<AssetDTO> prioritizedAssets = new();

        if (assets == null)
        {
            throw new ArgumentNullException(nameof(assets), "The assets collection cannot be null.");
        }

        foreach (AssetDTO asset in assets)
        {
            // If CapacityMw is less than what's needed to meet the threshold, add 100% of it
            if (totalCapacityMw + asset.CapacityMw <= threshold)
            {
                asset.RegulationPercentage = 100;
                prioritizedAssets.Add(asset);
                totalCapacityMw += asset.CapacityMw;
            }
            // Otherwise, calculate how many percent of the CapacityMw is needed to match the exact threshold and break out of the loop
            else
            {
                asset.RegulationPercentage = (threshold - totalCapacityMw) / asset.CapacityMw * 100;
                prioritizedAssets.Add(asset);
                totalCapacityMw += (asset.RegulationPercentage / 100) * asset.CapacityMw;
                break;
            }
        }

        if (decimal.Round(totalCapacityMw, 4) < threshold)
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), threshold, $"totalCapacityMw({totalCapacityMw}) couldn't reach threshold.");
        }

        return prioritizedAssets;
    }
}
