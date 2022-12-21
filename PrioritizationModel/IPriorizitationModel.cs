namespace PrioritizationModel;

public interface IPriorizitationModel
{
    Task<IEnumerable<SignalDto>> GetAssetRegulations(SignalDto);
}
