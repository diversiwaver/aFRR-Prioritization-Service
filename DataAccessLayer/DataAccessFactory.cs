using DataAccessLayer.DataAccess;

namespace DataAccessLayer;

public class DataAccessFactory
{
    public static T GetDataAccess<T>(string connectionString) where T : class
    {
        switch (typeof(T).Name)
        {
            case "IAssetDataAccess": return new AssetDataAccess(connectionString) as T;
            case "IAssetGroupDataAccess": return new AssetGroupDataAccess(connectionString) as T;
            default:
                throw new ArgumentException($"Unknown type {typeof(T).FullName}");
        }
    }
}
