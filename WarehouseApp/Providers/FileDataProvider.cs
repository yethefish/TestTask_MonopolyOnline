using System.Text.Json;
using Entities;
using JsonConverters;

namespace Providers;
public class FileDataProvider : IDataProvider<Pallet>
{
    private readonly string _filePath;

    public FileDataProvider(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Pallet> Load()
    {
        if (!File.Exists(_filePath))
            return new List<Pallet>();

        try
        {
            string json = File.ReadAllText(_filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            options.Converters.Add(new PalletJsonConverter());
            options.Converters.Add(new BoxJsonConverter());

            var data = JsonSerializer.Deserialize<List<Pallet>>(json, options);

            return data ?? new List<Pallet>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
            return new List<Pallet>();
        }
    }
}