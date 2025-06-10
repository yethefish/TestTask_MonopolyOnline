namespace Providers;

using Entities;

class RandomDataProvider : IDataProvider<Pallet>
{
    private readonly int _count;
    private readonly Random _random = new();

    public RandomDataProvider(int count = 10)
    {
        _count = count;
    }

    public IEnumerable<Pallet> Load()
    {
        var result = new List<Pallet>();

        for (int i = 0; i < _count; i++)
        {
            double palletHeight = _random.NextDouble() * (100 - 1) + 1;
            double palletWidth = _random.NextDouble() * (100 - 1) + 1;
            double palletDepth = _random.NextDouble() * (100 - 1) + 1;

            var item = new Pallet(palletHeight, palletWidth, palletDepth);

            int boxCount = _random.Next(1, 15);
            for (int j = 0; j < boxCount; j++)
            {
                double boxHeight = _random.NextDouble() * (100 - 1) + 1;
                double boxWidth = _random.NextDouble() * (palletWidth - 1) + 1;
                double boxDepth = _random.NextDouble() * (palletDepth - 1) + 1;
                double boxWeight = _random.NextDouble() * (palletDepth - 1) + 1;

                var dates = GenerateDateOnlyPair();

                var box = new Box(boxHeight, boxWidth, boxDepth, boxWeight,
                                dates.earlier.ToString("dd.MM.yyyy"), dates.later.ToString("dd.MM.yyyy"));

                item.AddBox(box);
            }
            result.Add(item);

        }
        return result;
    }

    private static (DateOnly later, DateOnly earlier) GenerateDateOnlyPair()
    {
        var random = new Random();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var oneYearAgo = today.AddMonths(-1);

        int totalDaysRange = today.DayNumber - oneYearAgo.DayNumber;
        int startOffset = random.Next(totalDaysRange);
        
        var earlier = oneYearAgo.AddDays(startOffset);
        var later = earlier.AddDays(random.Next(1, 1001));

        return (later, earlier);
    }
}