namespace Amolenk.GameATron4000.Model.State;

public interface IState<T>
{
    T? GetChanges(T baseline);
}