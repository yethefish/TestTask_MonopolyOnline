namespace Providers;

public interface IDataProvider<T>
{
    IEnumerable<T> Load();
}