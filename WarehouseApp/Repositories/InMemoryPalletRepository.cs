using Entities;

namespace Repositories;

public class InMemoryPalletRepository : IRepository<Pallet>
{
    private List<Pallet> _pallets = new List<Pallet>();

    public List<Pallet> GetAll() => _pallets;

    public void Add(Pallet pallet)
    {
        try
        {
            _pallets.Add(pallet);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Add] Unexpected error: {ex.Message}");
        }
    }

    public void Add(IEnumerable<Pallet> pallets)
    {
        try
        {
            _pallets.AddRange(pallets);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Add] Unexpected error: {ex.Message}");
        }
        
    }

    public void Delete(Guid id)
    {
        var pallet = GetById(id);
        try
        {
            _pallets.Remove(pallet);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Delete] Unexpected error: {ex.Message}");
        }
    }

    public Pallet GetById(Guid id)
    {
        var pallet = _pallets.FirstOrDefault(p => p.Id == id);
        if (pallet == null)
        {
            throw new KeyNotFoundException($"Pallet with Id {id} not found");
        }
        return pallet;
    }

    public void Update(Pallet pallet)
    {
        var index = _pallets.IndexOf(pallet);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Pallet with Id {pallet.Id} not found");
        }
        _pallets[index] = pallet;
    }
}