namespace btr.nuna.Application
{
    public interface ISaveChange<T>
    {
        void SaveChanges(T model);
    }
    public interface IDeleteEntity<TKey>
    {
        void DeleteEntity(TKey key);
    }

    public interface ILoadEntity<TModel, TKey>
    {
        MayBe<TModel> LoadEntity(TKey key);
    }

}
