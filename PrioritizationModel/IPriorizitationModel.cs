using WebAPI.DTOs;

namespace PrioritizationModel;

public interface IPriorizitationModel
{
    Task<IEnumerable<SignalDTO>> GetAssetRegulations(SignalDTO signalDTO);
}
