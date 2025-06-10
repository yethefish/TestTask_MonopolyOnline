namespace Entities;
using DateParser;

public class Box : ContainerBase
{
    public DateOnly ExpirationDate { get; }
    public DateOnly? DateOfProduction { get; }

    public Box(double height, double width, double depth, double weight, string dateOfProduction, string expirationDate) : base(height, width, depth, weight)
    {
        _id = Guid.NewGuid();
        DateOfProduction = DateParser.ParseDateOrThrow(dateOfProduction, "Date of production");
        ExpirationDate = DateParser.ParseDateOrThrow(expirationDate, "Expiration date");
        if (ExpirationDate < DateOfProduction)
        {
            throw new ArgumentException("Expiration date must be greater than date of production");
        }
    }

    public Box(double height, double width, double depth, double weight, string dateOfProduction) : base(height, width, depth, weight)
    {
        _id = Guid.NewGuid();
        var tmp = DateParser.ParseDateOrThrow(dateOfProduction, "Date of production");
        DateOfProduction = tmp;
        ExpirationDate = tmp.AddDays(100);
    }

    public Box(Guid id, double height, double width, double depth, double weight, string dateOfProduction, string expirationDate) : base(height, width, depth, weight)
    {
        _id = id;
        DateOfProduction = DateParser.ParseDateOrThrow(dateOfProduction, "Date of production");
        ExpirationDate = DateParser.ParseDateOrThrow(expirationDate, "Expiration date");
        if (ExpirationDate < DateOfProduction)
        {
            throw new ArgumentException("Expiration date must be greater than date of production");
        }
    }

    public Box(Guid id, double height, double width, double depth, double weight, string dateOfProduction) : base(height, width, depth, weight)
    {
        _id = id;
        var tmp = DateParser.ParseDateOrThrow(dateOfProduction, "Date of production");
        DateOfProduction = tmp;
        ExpirationDate = tmp.AddDays(100);
    }
}