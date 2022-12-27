namespace PrioritizationService.DTOs;
public class SignalDTO
{
    public int Id { get; set; }
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
    public decimal QuantityMw { get; set; }
    public int DirectionId { get; set; }
    public int BidId { get; set; }
    public IEnumerable<AssetDTO>? AssetsToReguate { get; set; }
}
