using Entities;
using Repositories;
using Providers;

namespace Services;

public class PalletService
{
    private readonly IRepository<Pallet> _repository;
    private readonly IDataProvider<Pallet> _provider;

    public PalletService(IRepository<Pallet> repository, IDataProvider<Pallet> provider)
    {
        _repository = repository;
        _provider = provider;

        _repository.Add(_provider.Load());
    }

    public Dictionary<string, List<Pallet>> GetGroupedByExpiration()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var pallets = _repository.GetAll();

        var groupA = new List<Pallet>();
        var groupB = new List<Pallet>();
        var groupC = new List<Pallet>();

        foreach (var pallet in pallets)
        {
            if (pallet.ExpirationDate == default)
                continue;

            if (pallet.ExpirationDate < today)
            {
                groupA.Add(pallet);
            }
            else if ((pallet.ExpirationDate.DayNumber - today.DayNumber) <= 30)
            {
                groupB.Add(pallet);
            }
            else
            {
                groupC.Add(pallet);
            }
        }

        return new Dictionary<string, List<Pallet>>
        {
            ["Просроченные"] = groupA.OrderBy(p => p.Weight).ToList(),
            ["Скоро испортятся (< 30 дней)"] = groupB.OrderBy(p => p.Weight).ToList(),
            ["Срок годности в порядке (>= 30 дней)"] = groupC.OrderBy(p => p.Weight).ToList()
        };
    }

    public List<Pallet> GetThreeWithLongestExpirationDate()
    {
        var pallets = _repository.GetAll()
            .Where(p => p.ExpirationDate != default)
            .OrderByDescending(p => p.ExpirationDate)
            .Take(3)
            .ToList();

        return pallets.OrderBy(p => p.Volume).ToList();
            
    }

    public Pallet GetById(Guid id)
    {
        return _repository.GetById(id);
    }
}