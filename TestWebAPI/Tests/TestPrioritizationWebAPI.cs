using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PrioritizationModel;
using PrioritizationService.DTOs;
using TestPrioritizationModel.Tests;
using WebAPI.Controllers;

namespace TestWebAPI.Tests;

internal class TestPrioritizationWebAPI
{
    PrioritizationController _webApiController;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var mockDataAccess = new Mock<IAssetDataAccess>();
        mockDataAccess.Setup(m => m.GetAllAsync())
            .Returns(Task.FromResult(new List<Asset>()
            {
                new Asset() { Id = 1, AssetGroupId = 1, CapacityMw = 8},
                new Asset() { Id = 2, AssetGroupId = 1, CapacityMw = 8},
                new Asset() { Id = 3, AssetGroupId = 1, CapacityMw = 6},
                new Asset() { Id = 4, AssetGroupId = 1, CapacityMw = 6}
            } as IEnumerable<Asset>));
        var mockModel = new Mock<IPrioritizationModel>();
        mockModel.Setup(m => m.GetPrioritizedAssets(It.IsAny<IEnumerable<AssetDTO>>(), It.IsAny<decimal>()))
            .Returns(new List<AssetDTO>()
            {
                new AssetDTO() { Id = 1, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
                new AssetDTO() { Id = 2, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
                new AssetDTO() { Id = 3, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 100},
                new AssetDTO() { Id = 4, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 50}
            });
        var mockPrioritizationModelController = new PrioritizationModelController(mockDataAccess.Object, mockModel.Object);
        var mockLogger = new Mock<ILogger<PrioritizationController>>();
        _webApiController = new PrioritizationController(mockLogger.Object, mockPrioritizationModelController);
    }

    [Test]
    public async Task PrioritizationController_ShouldReturnSignalDTOWithAssetsToRegulateAndStatusCode200_WhenGettingPrioritization()
    {
        //Arrange
        WebAPI.DTOs.SignalDTO signal = new()
        {
            Id = 0,
            FromUtc = DateTime.UtcNow,
            ToUtc = DateTime.UtcNow.AddHours(1),
            Price = 15,
            CurrencyId = 0,
            QuantityMw = 25,
            DirectionId = 1,
            BidId = 0
        };
        List<AssetDTO> expectedAssets = new() {
            new AssetDTO() { Id = 1, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
            new AssetDTO() { Id = 2, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
            new AssetDTO() { Id = 3, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 100},
            new AssetDTO() { Id = 4, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 50}
        };

        //Act
        var actionResult = (await _webApiController.GetAssetRegulations(signal)).Result;

        //Assert
        try
        {
            if (actionResult is ObjectResult objRes)
            {
                WebAPI.DTOs.SignalDTO? signalDto = objRes.Value as WebAPI.DTOs.SignalDTO;
                Assert.Multiple(() =>
                {
                    Assert.That(objRes.StatusCode, Is.EqualTo(200), "Status code returned was not 200");
                    Assert.That(signalDto.AssetsToRegulate, Is.Not.Empty, "Returned SignalDTO had to assigned assets to regulate!");
                    Assert.That(signalDto.AssetsToRegulate, Is.EquivalentTo(expectedAssets).Using(new AssetDTOComparer()), "Returned assets did not match the assets from the stub.");
                });
            }
            else if (actionResult is StatusCodeResult scr)
            {
                Assert.That(scr.StatusCode, Is.EqualTo(200));
            }
        }
        catch
        {
            Assert.Fail("Action result was null");
        }
    }
}
