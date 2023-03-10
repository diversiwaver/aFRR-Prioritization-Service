using DataAccessLayer.Interfaces;
using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using PrioritizationModel;
using PrioritizationService.DTOs;

namespace TestPrioritizationModel.Tests;

internal class IntegrationTestPrioritizationService
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

    [TestCase("v1", 23.33), TestCase("v1", 12), TestCase("v1", 4), TestCase("v1", 2)]
    [Test]
    public void PrioritizationService_ShouldGetPrioritizedAssets_WhenGivenModelAndSignalDTO(string modelVersion, decimal quantityThreshold)
    {
        //Arrange
        decimal regulatedCapacity = 0;
        IAssetDataAccess dataAccess = DataAccessFactory.GetDataAccess<IAssetDataAccess>(_connectionString);
        IPrioritizationModel prioritizationModel = PrioritizationModelFactory.GetPrioritizationModel(modelVersion);
        PrioritizationModelController prioritizationModelController = new(dataAccess, prioritizationModel);
        SignalDTO signalDTO = new()
        {
            Id = 0,
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = quantityThreshold,
            Direction = Direction.Down,
            BidId = 0
        };

        //Act
        signalDTO = prioritizationModelController.GetAssetRegulationsAsync(signalDTO);
        regulatedCapacity = signalDTO.AssetsToRegulate.Sum(asset => (asset.RegulationPercentage / 100) * asset.CapacityMw);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(signalDTO, Is.Not.Null, "Returned SignalDTO was null!");
            Assert.That(signalDTO.AssetsToRegulate, Is.Not.Empty, "Returned SignalDTO had no assigned assets to regulate!");
            Assert.That(regulatedCapacity, Is.EqualTo(quantityThreshold).Within(0.0001), "Calculated regulation does not match the quantity threshold");
        });
    }
}
