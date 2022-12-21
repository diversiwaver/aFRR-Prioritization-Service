using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace TestDataAccess.Tests;

public class TestAssetDataAccess
{
    private IAssetDataAccess _dataAccess;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _dataAccess = DataAccessFactory.GetDataAccess<IAssetDataAccess>(Configuration.CONNECTION_STRING_TEST);
    }

    [Test]
    public async Task AssetDataAccess_ShouldGetAllAssets()
    {
        //Arrange
        IEnumerable<Asset> assets;

        //Act
        assets = await _dataAccess.GetAllAsync();

        //Assert
        Assert.That(assets, Is.Not.Empty, $"Failed to retrieve all assets!");
    }

    [Test]
    public async Task AssetDataAccess_ShouldGetAssetWithId1()
    {
        //Arrange
        Asset newAsset;

        //Act
        newAsset = await _dataAccess.GetAsync(1);

        //Assert
        Assert.That(newAsset, Is.Not.Null, $"No Asset was found with ID: 1");
    }

    [Test]
    public async Task AssetDataAccess_ShouldFailToGetAssetWithId0()
    {
        //Arrange
        Asset newAsset;

        //Act
        newAsset = await _dataAccess.GetAsync(0);

        //Assert
        Assert.That(newAsset, Is.Null, $"Surprisingly, an Asset with ID: 0 was found");

    }
}
