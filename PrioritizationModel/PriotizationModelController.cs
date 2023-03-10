using aFRRService.DTOs.DTOConverters;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using PrioritizationService.DTOs;

namespace PrioritizationModel;

public class PrioritizationModelController
{
    private readonly IAssetDataAccess _assetDataAccess;
    private readonly IPrioritizationModel _prioritizationModel;
    private IEnumerable<AssetDTO> _assets;

    public PrioritizationModelController(IAssetDataAccess assetDataAccess, IPrioritizationModel prioritizationModel)
    {
        _assetDataAccess = assetDataAccess;
        _assets = Enumerable.Empty<AssetDTO>();
        Task.Run(InitializeAssetsAsync).Wait();
        _prioritizationModel = prioritizationModel;
    }

    public SignalDTO GetAssetRegulationsAsync(SignalDTO signalDTO)
    {
        signalDTO.AssetsToRegulate = _prioritizationModel.GetPrioritizedAssets(_assets, signalDTO.QuantityMw);
        return signalDTO;
    }

    private async Task InitializeAssetsAsync()
    {
        _assets = DTOConverter<Asset, AssetDTO>.FromList(await _assetDataAccess.GetAllAsync());
    }
}
