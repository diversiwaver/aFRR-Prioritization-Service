using BaseDataAccess.Attributes;

namespace DataAccessLayer.Models;
public class AssetGroup
{
    [IsPrimaryKey]
    [IsAutoIncrementingID]
    public int Id { get; init; }
    public string BalanceArea { get; init; }

    public override string ToString()
    {
        return $"AssetGroup:{{Id={Id}, BalanceArea={BalanceArea}}}";
    }
}

