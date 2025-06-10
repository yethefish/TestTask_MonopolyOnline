using Services;
using Providers;
using Entities;
using Repositories;

class WarehouseApp
{
    private static PalletService? _service;
    public static void Main(string[] args)
    {
        Console.WriteLine("Выберите источник данных:");
        Console.WriteLine("1. JSON-файл");
        Console.WriteLine("2. Случайная генерация");
        Console.Write("> ");
        var input = Console.ReadLine();

        IDataProvider<Pallet> provider;
        if (input == "1")
        {
            Console.Write("Введите путь к JSON-файлу: ");
            var path = Console.ReadLine() ?? "data.json";
            provider = new FileDataProvider(path);
        }
        else
        {
            Console.Write("Сколько сгенерировать паллет? ");
            int count = int.TryParse(Console.ReadLine(), out int c) ? c : 10;
            provider = new RandomDataProvider(count);
        }

        _service = new PalletService(new InMemoryPalletRepository(), provider);

        MainMenu();
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Что нужно сделать?:");
            Console.WriteLine("1. Сгруппировать все паллеты по сроку годности");
            Console.WriteLine("2. Вывести 3 паллеты, которые содержат коробки с наибольшим сроком годности");
            Console.WriteLine("3. Вывести подробную информацию о паллете");
            Console.WriteLine("4. Выйти");
            Console.Write("> ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    var groups = _service!.GetGroupedByExpiration();

                    foreach (var group in groups)
                    {
                        Console.WriteLine($"\nГруппа: {group.Key} ({group.Value.Count} шт.)");
                        Console.WriteLine(new string('-', 110));
                        Console.WriteLine($"{"ID",-36} | {"Ширина",7} | {"Высота",7} | {"Глубина",7} | {"Вес",5} | {"Объем",10} | {"Годен до",10}");
                        Console.WriteLine(new string('-', 110));

                        foreach (var item in group.Value)
                        {
                            Console.WriteLine($"{item.Id,-36} | {item.Width,7:F1} | {item.Height,7:F1} | {item.Depth,7:F1} | {item.Weight,5:F1} | {item.Volume,10:F1} | {item.ExpirationDate:dd.MM.yyyy}");
                        }

                        Console.WriteLine(new string('-', 110));
                    }

                    Console.Write("\n> Нажмите любую клавишу чтобы продолжить");
                    Console.ReadLine();
                    break;

                case "2":
                    Console.Clear();
                    var pallets = _service!.GetThreeWithLongestExpirationDate();
                    Console.WriteLine(new string('-', 110));
                    Console.WriteLine($"{"ID",-36} | {"Ширина",7} | {"Высота",7} | {"Глубина",7} | {"Вес",5} | {"Объем",10} | {"Годен до",10}");
                    Console.WriteLine(new string('-', 110));
                    foreach (var item in pallets)
                    {
                        Console.WriteLine($"{item.Id,-36} | {item.Width,7:F1} | {item.Height,7:F1} | {item.Depth,7:F1} | {item.Weight,5:F1} | {item.Volume,10:F1} | {item.ExpirationDate:dd.MM.yyyy}");
                    }
                    Console.Write("\n> Нажмите любую клавишу чтобы продолжить");
                    Console.ReadLine();
                    break;

                case "3":
                    Console.Clear();
                    Console.Write("Введите ID паллеты: ");
                    var idInput = Console.ReadLine();
                    if (Guid.TryParse(idInput, out Guid palletId))
                    {
                        try
                        {
                            var pallet = _service!.GetById(palletId);
                            Console.Clear();
                            Console.WriteLine("Информация о паллете:");
                            Console.WriteLine(new string('-', 110));
                            Console.WriteLine($"{"ID",-36} | {"Ширина",7} | {"Высота",7} | {"Глубина",7} | {"Вес",5} | {"Объем",10} | {"Годен до",10}");
                            Console.WriteLine(new string('-', 110));
                            Console.WriteLine($"{pallet.Id,-36} | {pallet.Width,7:F1} | {pallet.Height,7:F1} | {pallet.Depth,7:F1} | {pallet.Weight,5:F1} | {pallet.Volume,10:F1} | {pallet.ExpirationDate:dd.MM.yyyy}");

                            var boxes = pallet.Boxes;

                            Console.WriteLine("\nСписок коробок:");
                            Console.WriteLine(new string('-', 130));
                            Console.WriteLine($"{"ID",-36} | {"Ширина",7} | {"Высота",7} | {"Глубина",7} | {"Вес",5} | {"Объем",10} | {"Произведена",12} | {"Годен до",12}");
                            Console.WriteLine(new string('-', 130));
                            if (boxes != null && boxes.Any())
                            {
                                foreach (var box in boxes)
                                {
                                    Console.WriteLine($"{box.Id,-36} | {box.Width,7:F1} | {box.Height,7:F1} | {box.Depth,7:F1} | {box.Weight,5:F1} | {box.Volume,10:F1} | {box.DateOfProduction:dd.MM.yyyy}   | {box.ExpirationDate:dd.MM.yyyy}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Нет коробок в паллете.");
                            }
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный формат ID");
                    }

                    Console.Write("\n> Нажмите любую клавишу чтобы продолжить");
                    Console.ReadLine();
                    break;

                case "4":
                    return;
                default:
                    break;
            }
        }
    }

}