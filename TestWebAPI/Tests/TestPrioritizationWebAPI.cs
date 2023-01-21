using aFRRService.DTOs;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PrioritizationModel;
using WebAPI.Controllers;
using AssetDTO = PrioritizationService.DTOs.AssetDTO;

namespace TestWebAPI.Tests;

internal class TestPrioritizationWebAPI
{
    PrioritizationsController _webApiController;

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
        var mockLogger = new Mock<ILogger<PrioritizationsController>>();
        _webApiController = new PrioritizationsController(mockLogger.Object, mockPrioritizationModelController);
    }

    [Test]
    public async Task PrioritizationController_ShouldReturnSignalDTOWithAssetsToRegulateAndStatusCode200_WhenGettingPrioritization()
    {
        //Arrange
        aFRRService.DTOs.SignalDTO signal = new()
        {
            Id = 0,
            ReceivedUtc = DateTime.UtcNow,
            SentUtc = DateTime.UtcNow.AddHours(1),
            QuantityMw = 25,
            Direction = Direction.Down,
            BidId = 0
        };
        List<aFRRService.DTOs.AssetDTO> expectedAssets = new() {
            new  aFRRService.DTOs.AssetDTO() { Id = 1, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
            new  aFRRService.DTOs.AssetDTO() { Id = 2, AssetGroupId = 1, CapacityMw = 8, RegulationPercentage = 100},
            new  aFRRService.DTOs.AssetDTO() { Id = 3, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 100},
            new  aFRRService.DTOs.AssetDTO() { Id = 4, AssetGroupId = 1, CapacityMw = 6, RegulationPercentage = 50}
        };

        //Act
        var actionResult = _webApiController.GetAssetRegulations(signal).Result;

        //Assert
        try
        {
            if (actionResult is ObjectResult objRes)
            {
                aFRRService.DTOs.SignalDTO? signalDto = objRes.Value as aFRRService.DTOs.SignalDTO;
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

internal class AssetDTOComparer : IEqualityComparer<aFRRService.DTOs.AssetDTO>
{
    public bool Equals(aFRRService.DTOs.AssetDTO x, aFRRService.DTOs.AssetDTO y)
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

    public int GetHashCode(aFRRService.DTOs.AssetDTO obj)
    {
        // Return a consistent value for objects with the same values
        return obj.Id.GetHashCode()
            ^ obj.AssetGroupId.GetHashCode()
            ^ obj.Location.GetHashCode()
            ^ obj.CapacityMw.GetHashCode()
            ^ obj.RegulationPercentage.GetHashCode();
    }
}
