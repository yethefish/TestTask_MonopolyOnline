using Entities;
using Xunit;

namespace WarehouseApp.Tests;
public class PalletTests
{
    private readonly Pallet _pallet;
    private readonly Box _smallBox;
    private readonly Box _largeBox;

    public PalletTests()
    {
        _pallet = new Pallet(100, 100, 100);
        _smallBox = new Box(10, 10, 10, 5, "01.01.2023", "01.03.2023");
        _largeBox = new Box(10, 110, 110, 10, "01.01.2023", "01.04.2023");
    }

    [Fact]
    public void AddBox_WithValidDimensions_ShouldAddBoxToPallet()
    {
        _pallet.AddBox(_smallBox);

        Assert.Single(_pallet.Boxes);
        Assert.Contains(_smallBox, _pallet.Boxes);
    }

    [Fact]
    public void AddBox_WithInvalidDimensions_ShouldThrowArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => _pallet.AddBox(_largeBox));
        Assert.Equal("Box is too big", exception.Message);
    }

    [Fact]
    public void Weight_ShouldBeSumOfPalletAndBoxesWeights()
    {
        var box1 = new Box(10, 10, 10, 5, "01.01.2023");
        var box2 = new Box(10, 10, 10, 15, "01.01.2023");

        _pallet.AddBox(box1);
        _pallet.AddBox(box2);

        Assert.Equal(30 + 5 + 15, _pallet.Weight);
    }

    [Fact]
    public void Volume_ShouldBeSumOfPalletAndBoxesVolumes()
    {
        var palletVolume = _pallet.Height * _pallet.Width * _pallet.Depth;
        var box = new Box(10, 20, 5, 5, "01.01.2023");
        _pallet.AddBox(box);
        var expectedVolume = palletVolume + box.Volume;

        var actualVolume = _pallet.Volume;

        Assert.Equal(expectedVolume, actualVolume);
    }

    [Fact]
    public void ExpirationDate_ShouldBeEarliestExpirationDateOfBoxes()
    {
        var earlierDate = "10.02.2023";
        var laterDate = "20.03.2023";
        var box1 = new Box(10, 10, 10, 5, "01.01.2023", earlierDate);
        var box2 = new Box(10, 10, 10, 5, "01.01.2023", laterDate);

        _pallet.AddBox(box1);
        _pallet.AddBox(box2);

        var palletExpirationDate = _pallet.ExpirationDate;

        Assert.Equal(DateOnly.Parse("2023-02-10"), palletExpirationDate);
    }

    [Fact]
    public void ExpirationDate_WhenNoBoxes_ShouldThrowException()
    {
        var exception = Assert.Throws<Exception>(() => _pallet.ExpirationDate);
        Assert.Equal("There is no boxes in pallet", exception.Message);
    }
}