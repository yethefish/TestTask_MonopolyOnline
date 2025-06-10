using Entities;
using Xunit;

namespace WarehouseApp.Tests;

public class BoxTests
{
    [Fact]
    public void Constructor_WithValidDates_ShouldCreateBox()
    {
        var dateOfProduction = "01.01.2023";
        var expirationDate = "10.01.2023";

        var box = new Box(10, 10, 10, 5, dateOfProduction, expirationDate);

        Assert.Equal(DateOnly.Parse("2023-01-01"), box.DateOfProduction);
        Assert.Equal(DateOnly.Parse("2023-01-10"), box.ExpirationDate);
        Assert.NotEqual(Guid.Empty, box.Id);
    }

    [Fact]
    public void Constructor_WithExpirationDateBeforeProductionDate_ShouldThrowArgumentException()
    {
        var dateOfProduction = "10.01.2023";
        var expirationDate = "01.01.2023";

        var exception = Assert.Throws<ArgumentException>(() =>
            new Box(10, 10, 10, 5, dateOfProduction, expirationDate));

        Assert.Equal("Expiration date must be greater than date of production", exception.Message);
    }

    [Fact]
    public void Constructor_WithoutExpirationDate_ShouldSetItTo100DaysAfterProduction()
    {
        var dateOfProductionString = "01.01.2023";
        var dateOfProduction = DateOnly.Parse("2023-01-01");
        var expectedExpirationDate = dateOfProduction.AddDays(100);

        var box = new Box(10, 10, 10, 5, dateOfProductionString);

        Assert.Equal(dateOfProduction, box.DateOfProduction);
        Assert.Equal(expectedExpirationDate, box.ExpirationDate);
    }

    [Fact]
    public void Constructor_WithId_ShouldAssignCorrectId()
    {
        var id = Guid.NewGuid();
        var dateOfProduction = "01.01.2023";
        var expirationDate = "10.01.2023";

        var box = new Box(id, 10, 10, 10, 5, dateOfProduction, expirationDate);

        Assert.Equal(id, box.Id);
    }
}