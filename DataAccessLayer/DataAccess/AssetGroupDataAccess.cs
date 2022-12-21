using BaseDataAccess;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;

internal class AssetGroupDataAccess : BaseDataAccess<AssetGroup>, IAssetGroupDataAccess
{
    public AssetGroupDataAccess(string connectionString) : base(connectionString)
    {
    }
}
