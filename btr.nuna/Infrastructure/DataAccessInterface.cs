using System.Collections.Generic;

namespace btr.nuna.Infrastructure
{
    public interface IInsert<in T>
    {
        void Insert(T model);
    }
    public interface IInsertWithResponse<out TOut, in TIn> 
    {
        TOut Insert(TIn model);
    }

    public interface IInsertBulk<in T>
    {
        void Insert(IEnumerable<T> listModel);
    }
    public interface IUpdate<in TData, in TKey>
    {
        void Update(TData model, TKey key);
    }
    public interface IUpdate<T>
    {
        void Update(T model);
    }
    
    public interface IDelete<T>
    {
        void Delete(T key);
    }
    
    public interface IGetData<out TResult, in TKey>
    {
        TResult GetData(TKey key);
    }
    
    public interface IListData<out TResult>
    {
        IEnumerable<TResult> ListData();
    }
    public interface IListData<out TResult, in T>
    {
        IEnumerable<TResult> ListData(T filter);
    }
    public interface IListData<out TResult, in T1, in T2>
    {
        IEnumerable<TResult> ListData(T1 filter1, T2 filter2);
    }
    public interface IListData<out TResult, in T1, in T2, in T3>
    {
        IEnumerable<TResult> ListData(T1 filter1, T2 filter2, T3 filter3);
    }
}