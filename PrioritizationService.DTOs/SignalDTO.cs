namespace PrioritizationService.DTOs;
public class SignalDTO
{
    public int Id { get; set; }
    public DateTime ReceivedUtc { get; set; }
    public DateTime SentUtc { get; set; }
    public decimal QuantityMw { get; set; }
    public Direction Direction { get; set; }
    public int BidId { get; set; }
    public IEnumerable<AssetDTO>? AssetsToRegulate { get; set; }
}
