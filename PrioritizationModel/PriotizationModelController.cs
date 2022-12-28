using Microsoft.Extensions.Configuration;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using PrioritizationService.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace PrioritizationModel;

public class PrioritizationModelController
{
    private IAssetDataAccess _assetDataAccess;
    private IPrioritizationModel _prioritizationModel;
    private IEnumerable<AssetDTO> _assets;

    public PrioritizationModelController(IAssetDataAccess assetDataAccess, IPrioritizationModel prioritizationModel)
    {
        _assetDataAccess = assetDataAccess;
        Task.Run(InitializeOrderedAssetsAsync).Wait();
        _prioritizationModel = prioritizationModel;
    }

    public SignalDTO GetAssetRegulationsAsync(SignalDTO signalDTO, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("aFRR-Service-DataBase");
        if (connectionString is null)
        {
            throw new ArgumentNullException($"Connection string is null. Failed to retrieve connection string from {configuration}");
        }
        signalDTO.AssetsToReguate = _prioritizationModel.GetPrioritizedAssets(_assets, signalDTO.QuantityMw);
        return signalDTO;
    }

    private async Task InitializeOrderedAssetsAsync()
    {
        _assets = DTOConverter<Asset, AssetDTO>.FromList(await _assetDataAccess.GetAllAsync()).OrderBy(asset => asset.CapacityMw);
    }
}
