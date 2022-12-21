using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer;

namespace TestDataAccess.Tests;

public class TestAssetGoupDataAccess
{
    private IAssetGroupDataAccess _dataAccess;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _dataAccess = DataAccessFactory.GetDataAccess<IAssetGroupDataAccess>(Configuration.CONNECTION_STRING_TEST);
    }

    [Test]
    public async Task AssetGroupDataAccess_ShouldGetAllAssetGroups()
    {
        //Arrange
        IEnumerable<AssetGroup> assetGroups;

        //Act
        assetGroups = await _dataAccess.GetAllAsync();

        //Assert
        Assert.That(assetGroups, Is.Not.Empty, $"Failed to retrieve all asset groups!");
    }

    [Test]
    public async Task AssetDataAccess_ShouldGetAssetWithId0()
    {
        //Arrange
        AssetGroup newAssetGroup;

        //Act
        newAssetGroup = await _dataAccess.GetAsync(0);

        //Assert
        Assert.That(newAssetGroup, Is.Not.Null, $"No Asset Group was found with ID: 0");
    }

    [Test]
    public async Task AssetDataAccess_ShouldFailToGetAssetWithIdMinus1()
    {
        //Arrange
        AssetGroup newAssetGroup;

        //Act
        newAssetGroup = await _dataAccess.GetAsync(-1);

        //Assert
        Assert.That(newAssetGroup, Is.Null, $"Surprisingly, an Asset Group with ID: -1 was found");

    }
}
