namespace Project.Default
{
    public interface IInputCommand
    {
        void Execute();
    }

    public interface IInputCommand<T>
    {
        void Execute(T value);
    }
}
