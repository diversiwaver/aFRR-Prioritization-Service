using BenchmarkDotNet.Attributes;
using Dapper;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Data.SqlClient;

namespace BenchmarkTests;

[MemoryDiagnoser]
public class BenchmarkDataAccess
{

    private IAssetDataAccess _assetDataAccess;

    [Params(1, 10)]
    public int Iterations;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _assetDataAccess = DataAccessFactory.GetDataAccess<IAssetDataAccess>(Configuration.CONNECTION_STRING_TEST);
    }

    [Benchmark]
    public async Task GetAllAssets_AssetDataAccess_BaseDataAccess()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = await _assetDataAccess.GetAllAsync();
        }
    }

    [Benchmark]
    public async Task GetAllAssets_AssetDataAccess_Dapper()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = await GetAllAsync_Dapper();
        }
    }

    [Benchmark]
    public async Task GetAllAssets_AssetDataAccess_SqlCommand()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = await GetAllAsync_SqlCommand();
        }
    }

    public async Task<IEnumerable<Asset>> GetAllAsync_Dapper()
    {
        try
        {
            using IDbConnection connection = new SqlConnection(Configuration.CONNECTION_STRING_TEST);
            return await connection.QueryAsync<Asset>("SELECT * FROM Asset;");
        }
        catch (Exception ex)
        {
            throw new Exception("Error in Benchmark", ex);
        }
    }

    public async Task<IEnumerable<Asset>> GetAllAsync_SqlCommand()
    {
        try
        {
            using SqlConnection connection = new(Configuration.CONNECTION_STRING_TEST);
            await connection.OpenAsync();

            using SqlCommand command = new("SELECT * FROM Asset;", connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Asset> assets = new();

            while (await reader.ReadAsync())
            {
                Asset asset = new()
                {
                    Id = reader.GetInt32(0),
                    AssetGroupId = reader.GetInt32(1),
                    CapacityMw = reader.GetInt32(2)
                };
                assets.Add(asset);
            }

            return assets;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in Benchmark", ex);
        }
    }
}
