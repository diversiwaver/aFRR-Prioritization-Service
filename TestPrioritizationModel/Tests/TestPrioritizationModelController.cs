using DataAccessLayer.Interfaces;
using PrioritizationModel;
using PrioritizationService.DTOs;
using TestPrioritizationModel.Stubs;

namespace TestPrioritizationModel.Tests;

internal class TestPrioritizationModelController
{
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
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = 35,
            Direction = Direction.Down,
            BidId = 0
        };
        IEnumerable<AssetDTO> expectedResults = new List<AssetDTO> {
            new AssetDTO() {Id = 1, CapacityMw = 10, RegulationPercentage = 100},
            new AssetDTO() {Id = 2, CapacityMw = 15, RegulationPercentage = 100},
            new AssetDTO() {Id = 3, CapacityMw = 20, RegulationPercentage = 50}
        };

        //Act
        signalDTO = priotizationModelController.GetAssetRegulationsAsync(signalDTO);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(signalDTO, Is.Not.Null, "Returned SignalDTO was null!");
            Assert.That(signalDTO.AssetsToRegulate, Is.Not.Empty, "Returned SignalDTO had to assigned assets to regulate!");
            Assert.That(signalDTO.AssetsToRegulate, Is.EquivalentTo(expectedResults).Using(new AssetDTOComparer()), "Returned assets did not match the assets from the stub.");
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