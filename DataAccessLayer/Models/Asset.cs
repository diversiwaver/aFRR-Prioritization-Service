using BaseDataAccess.Attributes;

namespace DataAccessLayer.Models;
public class Asset
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; init; }
    public int AssetGroupId { get; init; }
    public string Location { get; init; }
    public int CapacityMw { get; init; }

    public override string ToString()
    {
        return $"Asset:{{Id={Id}, AssetGroupId={AssetGroupId}, Location={Location}, CapacityMw={CapacityMw}}}";
    }
}
