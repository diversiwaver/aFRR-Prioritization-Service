using aFRRService.DTOs.DTOConverters;
using BenchmarkDotNet.Attributes;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using PrioritizationModel;
using PrioritizationService.DTOs;
using DataAccessLayer.Models;
using System.Runtime.InteropServices;

namespace BenchmarkTests;

[MemoryDiagnoser]
public class BenchmarkPrioritizationModel
{
    
    private IAssetDataAccess _assetDataAccess;

    private IPrioritizationModel _basicModel;

    private SealedPrioritizationModel _sealedBasicModel;

    IEnumerable<AssetDTO> _assets;

    [Params(1, 10)]
    public int Iterations;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _assetDataAccess = DataAccessFactory.GetDataAccess<IAssetDataAccess>(Configuration.CONNECTION_STRING_TEST);
        _assets = DTOConverter<Asset, AssetDTO>.FromList(await _assetDataAccess.GetAllAsync());
        _basicModel = PrioritizationModelFactory.GetPrioritizationModel("v1");
        _sealedBasicModel = new SealedPrioritizationModel();
    }

    [Benchmark]
    public void GetPrioritizedAssets_BasicPrioritizationModel_BasicModel()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = _basicModel.GetPrioritizedAssets(_assets, 25m);
        }
    }

    [Benchmark]
    public void GetPrioritizedAssets_BasicPrioritizationModel_SpanV1()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = GetPrioritizedAssets_SpanV1(_assets, 25m);
        }
    }

    [Benchmark]
    public void GetPrioritizedAssets_BasicPrioritizationModel_SealedSpan()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = _sealedBasicModel.GetPrioritizedAssets_SealedSpan(_assets, 25m);
        }
    }

    public IEnumerable<AssetDTO> GetPrioritizedAssets_SpanV1(IEnumerable<AssetDTO> assets, decimal quantityThreshold)
    {
        decimal totalCapacityMw = 0;
        HashSet<AssetDTO> prioritizedAssets = new();
        assets = assets.OrderByDescending(asset => asset.CapacityMw);

        if (assets == null)
        {
            throw new ArgumentNullException(nameof(assets), "The assets collection cannot be null.");
        }

        Span<AssetDTO> assetSpan = CollectionsMarshal.AsSpan(assets.ToList());

        for (int i = 0; i < assetSpan.Length; i++)
        {
            // If CapacityMw is less than what's needed to meet the threshold, add 100% of it
            if (totalCapacityMw + assetSpan[i].CapacityMw <= quantityThreshold)
            {
                assetSpan[i].RegulationPercentage = 100;
                prioritizedAssets.Add(assetSpan[i]);
                totalCapacityMw += assetSpan[i].CapacityMw;
            }
            // Otherwise, calculate how many percent of the CapacityMw is needed to match the exact threshold and break out of the loop
            else
            {
                assetSpan[i].RegulationPercentage = (quantityThreshold - totalCapacityMw) / assetSpan[i].CapacityMw * 100;
                prioritizedAssets.Add(assetSpan[i]);
                totalCapacityMw += (assetSpan[i].RegulationPercentage / 100) * assetSpan[i].CapacityMw;
                break;
            }
        }

        return prioritizedAssets;
    }
}

internal sealed class SealedPrioritizationModel
{
    public IEnumerable<AssetDTO> GetPrioritizedAssets_SealedSpan(IEnumerable<AssetDTO> assets, decimal quantityThreshold)
    {
        decimal totalCapacityMw = 0;
        HashSet<AssetDTO> prioritizedAssets = new();

        if (assets == null)
        {
            throw new ArgumentNullException(nameof(assets), "The assets collection cannot be null.");
        }

        Span<AssetDTO> assetSpan = assets.OrderByDescending(asset => asset.CapacityMw).ToArray().AsSpan();

        for (int i = 0; i < assetSpan.Length; i++)
        {
            // If CapacityMw is less than what's needed to meet the threshold, add 100% of it
            if (totalCapacityMw + assetSpan[i].CapacityMw <= quantityThreshold)
            {
                assetSpan[i].RegulationPercentage = 100;
                prioritizedAssets.Add(assetSpan[i]);
                totalCapacityMw += assetSpan[i].CapacityMw;
            }
            // Otherwise, calculate how many percent of the CapacityMw is needed to match the exact threshold and break out of the loop
            else
            {
                assetSpan[i].RegulationPercentage = (quantityThreshold - totalCapacityMw) / assetSpan[i].CapacityMw * 100;
                prioritizedAssets.Add(assetSpan[i]);
                totalCapacityMw += (assetSpan[i].RegulationPercentage / 100) * assetSpan[i].CapacityMw;
                break;
            }
        }

        return prioritizedAssets;
    }
}