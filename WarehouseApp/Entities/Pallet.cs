namespace Entities;

public class Pallet : ContainerBase
{
    private List<Box> _boxes;

    public List<Box> Boxes { get => _boxes; }
    public DateOnly ExpirationDate
    {
        get
        {
            if (Boxes == null || !Boxes.Any())
            {
                throw new Exception("There is no boxes in pallet");
            }
            return Boxes.Min(b => b.ExpirationDate);
        }
    }

    public override double Weight
    {
        get
        {
            double weight = 30;
            foreach (var box in _boxes)
            {
                weight += box.Weight;
            }
            return weight;
        }
        protected set => base.Weight = value;
    }

    public override double Volume
    {
        get
        { 
            double volume = Height * Width * Depth;
            foreach (var box in _boxes)
            {
                volume += box.Volume;
            }
            return volume;
        }
    }

    public Pallet(double height, double width, double depth) : base(height, width, depth, 30)
    {
        _id = Guid.NewGuid();
        _boxes = new List<Box>();
    }

    public Pallet(Guid id, double height, double width, double depth) : base(height, width, depth, 30)
    {
        _id = id;
        _boxes = new List<Box>();
    }

    public void AddBox(Box box)
    {
        if (box.Depth < Depth && box.Width < Width)
        {
            _boxes.Add(box);
        }
        else
        {
            Console.WriteLine($"Depth {Depth}, box.Depth {box.Depth}");
            Console.WriteLine($"Width {Width}, box.Width {box.Width}");
            throw new ArgumentException("Box is too big");
        }
    }
}