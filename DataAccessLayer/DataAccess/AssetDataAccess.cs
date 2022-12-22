using BaseDataAccess;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.DataAccess;

internal class AssetDataAccess : BaseDataAccess<Asset>, IAssetDataAccess
{
    public AssetDataAccess(string connectionString) : base(connectionString)
    {
    }
}
