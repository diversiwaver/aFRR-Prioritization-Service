using Microsoft.Extensions.Configuration;
using WebAPI.DTOs;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using PrioritizationService.DTOs;
using WebAPI.DTOs.DTOConverters;

namespace PrioritizationModel;

public class PriotizationModelController
{
    private IAssetDataAccess _assetDataAccess;
    private IPrioritizationModel _prioritizationModel;
    private IEnumerable<AssetDTO> _assets;

    public PriotizationModelController(IAssetDataAccess assetDataAccess, IPrioritizationModel prioritizationModel)
    {
        _assetDataAccess = assetDataAccess;
        Task.Run(InitializeOrderedAssetsAsync).Wait();
        _prioritizationModel = prioritizationModel;
    }

    public async Task<IEnumerable<SignalDTO>> GetAssetRegulationsAsync(SignalDTO signalDTO, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("aFRR-Service-DataBase");
        if (connectionString is null)
        {
            throw new ArgumentException($"Connection string is null. Failed to retrieve connection string from {configuration}");
        }

        _prioritizationModel.GetPrioritizedAssets(_assets, signalDTO.QuantityMw);
        //TODO: assign returned assets to signal

        return await Task.FromResult(new List<SignalDTO>());
    }

    private async Task InitializeOrderedAssetsAsync()
    {
        _assets = DTOConverter<Asset, AssetDTO>.FromList(await _assetDataAccess.GetAllAsync()).OrderBy(asset => asset.CapacityMw);
    }
}
