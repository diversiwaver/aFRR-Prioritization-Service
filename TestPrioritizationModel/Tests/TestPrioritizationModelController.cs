using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using PrioritizationModel;
using PrioritizationService.DTOs;
using TestPrioritizationModel.Stubs;

namespace TestPrioritizationModel.Tests;

internal class TestPrioritizationModelController
{
    string _connectionString;
    IConfiguration _configuration;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddXmlFile("app.config");
        _configuration = builder.Build();

        _connectionString = _configuration.GetConnectionString("aFRR-Service-DataBase");
    }

    [TestCase("v1")]
    [Test]
    public async Task AssetDataAccess_ShouldGetAllAssets(string version)
    {
        //Arrange
        IAssetDataAccess dataAccess = DataAccessFactory.GetDataAccess<IAssetDataAccess>(_connectionString); // TODO: Use a Stub instead
        IPrioritizationModel prioritizationModel = new PrioritizationModelStub();
        PriotizationModelController priotizationModelController = new(dataAccess, prioritizationModel);
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
        //Act
        signalDTO = priotizationModelController.GetAssetRegulationsAsync(signalDTO, _configuration);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Fail("Task failed successfully (TODO: Figure out the asserts and finish the PrioritizationModelController)");
        });
    }
}
