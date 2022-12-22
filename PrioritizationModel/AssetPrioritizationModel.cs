using WebAPI.DTOs;

namespace PrioritizationModel;

internal class AssetPrioritizationModel : IPriorizitationModel
{
    public Task<IEnumerable<SignalDTO>> GetAssetRegulations(SignalDTO signalDTO)
    {
        throw new NotImplementedException();
    }
}
