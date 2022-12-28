﻿using PrioritizationService.DTOs;

namespace PrioritizationModel;

internal class AssetPrioritizationModel : IPrioritizationModel
{
    public IEnumerable<AssetDTO> GetPrioritizedAssets(IEnumerable<AssetDTO> assets, decimal threshold)
    {
        decimal totalCapacityMw = 0;

        if (assets == null)
        {
            throw new ArgumentNullException(nameof(assets), "The assets collection cannot be null.");
        }

        foreach (AssetDTO asset in assets)
        {
            // If CapacityMw is less than what's needed to meet the threshold, add 100% of it
            if (totalCapacityMw + asset.CapacityMw <= threshold)
            {
                // Creates an iterator block in a method. When you use yield return, you can return a sequence of values one at a time, rather than all at once.
                asset.RegulationPercentage = 100;
                yield return asset;
                totalCapacityMw += asset.CapacityMw;
            }
            // Otherwise, calculate how many percent of the CapacityMw is needed to match the exact threshold and break out of the loop
            else
            {
                asset.RegulationPercentage = (threshold - totalCapacityMw) / asset.CapacityMw * 100;
                yield return asset;
                break;
            }
        }
    }
}