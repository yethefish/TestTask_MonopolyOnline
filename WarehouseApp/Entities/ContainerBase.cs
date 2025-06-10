namespace Entities;

public abstract class ContainerBase
{
    protected double _height, _width, _depth, _weight;
    protected Guid _id;
    public Guid Id { get => _id; }

    public double Height
    {
        get => _height;
        protected set => _height = value < 0 ? throw new ArgumentException($"Height of {this.GetType().Name} must be greater than zero") : value;
    }

    public double Width
    {
        get => _width;
        protected set => _width = value < 0 ? throw new ArgumentException($"Width of {this.GetType().Name} must be greater than zero") : value;
    }

    public double Depth
    {
        get => _depth;
        protected set => _depth = value < 0 ? throw new ArgumentException($"Depth of {this.GetType().Name} must be greater than zero") : value;
    }

    public virtual double Weight
    {
        get => _weight;
        protected set => _weight = value < 0 ? throw new ArgumentException($"Weight of {this.GetType().Name} must be greater than zero") : value;
    }

    public virtual double Volume { get;}

    protected ContainerBase(double height, double width, double depth, double weight)
    {
        Height = height;
        Width = width;
        Depth = depth;
        Weight = weight;
        checked
        {
            Volume = Height * Width * Depth;
        }   
    }
}