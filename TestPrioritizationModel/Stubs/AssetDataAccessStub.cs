using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using PrioritizationService.DTOs;

namespace TestPrioritizationModel.Stubs;

internal class AssetDataAccessStub : IAssetDataAccess
{
    public Task<int> CreateAsync(Asset entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Asset>> GetAllAsync()
    {
        IEnumerable<Asset> assets = new List<Asset>()
        {
            new Asset() { Id = 1, CapacityMw = 1},
            new Asset() { Id = 1, CapacityMw = 2},
            new Asset() { Id = 1, CapacityMw = 3},
            new Asset() { Id = 1, CapacityMw = 4},
            new Asset() { Id = 1, CapacityMw = 5},
            new Asset() { Id = 1, CapacityMw = 6},
            new Asset() { Id = 1, CapacityMw = 7},
            new Asset() { Id = 1, CapacityMw = 8},
            new Asset() { Id = 1, CapacityMw = 8},
            new Asset() { Id = 1, CapacityMw = 10},
            new Asset() { Id = 1, CapacityMw = 10},
            new Asset() { Id = 1, CapacityMw = 15}
        };
        return Task.FromResult(assets);
    }

    public Task<Asset> GetAsync(params dynamic[] id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Asset entity)
    {
        throw new NotImplementedException();
    }
}
