using PrioritizationModel;
using PrioritizationService.DTOs;

namespace TestPrioritizationModel.Tests;

internal class TestPrioritizationModel
{
    IEnumerable<AssetDTO> _assets;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _assets = new List<AssetDTO>()
        {
            new AssetDTO() { Id = 1, CapacityMw = 1},
            new AssetDTO() { Id = 1, CapacityMw = 2},
            new AssetDTO() { Id = 1, CapacityMw = 3},
            new AssetDTO() { Id = 1, CapacityMw = 4},
            new AssetDTO() { Id = 1, CapacityMw = 5},
            new AssetDTO() { Id = 1, CapacityMw = 6},
            new AssetDTO() { Id = 1, CapacityMw = 7},
            new AssetDTO() { Id = 1, CapacityMw = 8},
            new AssetDTO() { Id = 1, CapacityMw = 8},
            new AssetDTO() { Id = 1, CapacityMw = 10},
            new AssetDTO() { Id = 1, CapacityMw = 10},
            new AssetDTO() { Id = 1, CapacityMw = 15}
        };
    }

    [TestCase("v1", 999), TestCase("v1", 30), TestCase("v1", 23.33), TestCase("v1", 12), TestCase("v1", 4), TestCase("v1", 2), TestCase("v1", 0)]
    [Test]
    public void PrioritizationModel_ShouldGetPrioritizedAssets_WhenGivenSignalAndThreshold(string modelVersion, decimal threshold)
    {
        // Assert
        IPrioritizationModel prioritizationModel = PrioritizationModelFactory.GetPrioritizationModel(modelVersion);
        IEnumerable<AssetDTO> retrievedAssets = Enumerable.Empty<AssetDTO>();
        decimal totalRegulatedCapacity = 0;

        // Act
        if (threshold > _assets.Sum(asset => asset.CapacityMw))
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => prioritizationModel.GetPrioritizedAssets(_assets, threshold), "Prioritization Model did not throw exception when given unreachable threshold.");
        }
        else
        {
            retrievedAssets = prioritizationModel.GetPrioritizedAssets(_assets, threshold);

            totalRegulatedCapacity = retrievedAssets.Sum(asset => (asset.RegulationPercentage / 100) * asset.CapacityMw);

            // Arrange
            Assert.That(totalRegulatedCapacity, Is.EqualTo(threshold).Within(0.0001));
        }
    }
}
