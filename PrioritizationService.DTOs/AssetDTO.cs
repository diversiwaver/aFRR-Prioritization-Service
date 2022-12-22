using System.Security.Principal;

namespace PrioritizationService.DTOs;

public class AssetDTO
{
    public int Id { get; init; }
    public int AssetGroupId { get; init; }
    public string Location { get; init; }
    public int CapacityMw { get; init; }
    public decimal RegulationPercentage { get; set; }
}