using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using PrioritizationModel;
using PrioritizationService.DTOs;
using TestPrioritizationModel.Stubs;

namespace TestPrioritizationModel.Tests;

internal class TestPrioritizationModelController
{
    string? _connectionString;
    IConfiguration _configuration;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../");
        _configuration = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("appsettings.json", optional: false).Build();

        _connectionString = _configuration.GetConnectionString("aFRR-Service-DataBase");
    }

    [Test]
    public void PrioritizationController_ShouldGetPrioritizedAssets_WhenProvidedAModel()
    {
        //Arrange
        IAssetDataAccess dataAccess = new AssetDataAccessStub();
        IPrioritizationModel prioritizationModel = new PrioritizationModelStub();
        PrioritizationModelController priotizationModelController = new(dataAccess, prioritizationModel);
        SignalDTO signalDTO = new()
        {
            Id = 0,
            FromUtc = DateTime.UtcNow,
            ToUtc = DateTime.UtcNow.AddHours(1),
            Price = 25,
            CurrencyId = 0,
            QuantityMw = 35,
            DirectionId = 1,
            BidId = 0
        };
        IEnumerable<AssetDTO> expectedResults = new List<AssetDTO> {
            new AssetDTO() {Id = 1, CapacityMw = 10, RegulationPercentage = 100},
            new AssetDTO() {Id = 2, CapacityMw = 15, RegulationPercentage = 100},
            new AssetDTO() {Id = 3, CapacityMw = 20, RegulationPercentage = 50}
        };

        //Act
        signalDTO = priotizationModelController.GetAssetRegulationsAsync(signalDTO, _configuration);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(signalDTO, Is.Not.Null, "Returned SignalDTO was null!");
            Assert.That(signalDTO.AssetsToReguate, Is.Not.Empty, "Returned SignalDTO had to assigned assets to regulate!");
            Assert.That(signalDTO.AssetsToReguate, Is.EquivalentTo(expectedResults).Using(new AssetDTOComparer()), "Returned assets did not match the assets from the stub.");
        });
    }
}

internal class AssetDTOComparer : IEqualityComparer<AssetDTO>
{
    public bool Equals(AssetDTO x, AssetDTO y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
            && x.AssetGroupId == y.AssetGroupId
            && x.Location == y.Location
            && x.CapacityMw == y.CapacityMw
            && x.RegulationPercentage == y.RegulationPercentage;
    }

    public int GetHashCode(AssetDTO obj)
    {
        // Return a consistent value for objects with the same values
        return obj.Id.GetHashCode()
            ^ obj.AssetGroupId.GetHashCode()
            ^ obj.Location.GetHashCode()
            ^ obj.CapacityMw.GetHashCode()
            ^ obj.RegulationPercentage.GetHashCode();
    }
}