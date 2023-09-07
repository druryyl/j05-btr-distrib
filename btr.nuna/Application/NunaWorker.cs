namespace btr.nuna.Application
{
    public interface INunaWriter<T>
    {
        void Save(ref T model);
    }

    public interface INunaBuilder<out T>
    {
        T Build();
    }

    public interface INunaService<out TOut, in TIn>
    {
        TOut Execute(TIn req);
    }
    public interface INunaService<out TOut>
    {
        TOut Execute();
    }
    public interface INunaServiceVoid<in TIn>
    {
        void Execute(TIn req);
    }
}