namespace Amolenk.GameATron4000.Model.State;

public interface ISnapshot<T>
{
    T? GetChanges(T baseline);
}