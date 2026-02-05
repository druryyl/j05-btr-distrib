using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.nuna.Application
{
    public readonly struct MayBe<T>
    {
        private readonly T _value;

        public bool HasValue { get; }

        public T Value => HasValue
            ? _value
            : throw new InvalidOperationException("No value present.");

        private MayBe(T value)
        {
            _value = value;
            HasValue = true;
        }

        public static MayBe<T> Some(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new MayBe<T>(value);
        }

        public static MayBe<T> None => new MayBe<T>();

        public MayBe<TResult> Map<TResult>(Func<T, TResult> mapper)
        {
            return HasValue ? MayBe<TResult>.Some(mapper(_value)) : MayBe<TResult>.None;
        }

        public MayBe<TResult> Bind<TResult>(Func<T, MayBe<TResult>> binder)
        {
            return HasValue ? binder(_value) : MayBe<TResult>.None;
        }

        public T GetValueOrDefault(T defaultValue = default)
        {
            return HasValue ? _value : defaultValue;
        }

        public T GetValueOrThrow(string errorMessage)
        {
            return HasValue ? _value : throw new KeyNotFoundException(errorMessage);
        }

        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
        {
            return HasValue ? onSome(_value) : onNone();
        }

        public void Match(Action<T> onSome, Action onNone)
        {
            if (HasValue)
                onSome(_value);
            else
                onNone();
        }

        public override string ToString() => HasValue ? $"Some({_value})" : "None";
    }

    public static class MayBe
    {
        public static MayBe<T> From<T>(T value) where T : class
        {
            return value == null
                ? MayBe<T>.None
                : MayBe<T>.Some(value);
        }
        public static MayBe<T> ToMayBe<T>(this T value) where T : class
            => From(value);
    }

    public static class MaybeMonadExtensions
    {
        public static MayBe<T> ToMaybe<T>(this T value)
        {
            return value == null ? MayBe<T>.None : MayBe<T>.Some(value);
        }

        public static MayBe<T> Where<T>(this MayBe<T> mayBe, Func<T, bool> predicate)
        {
            return mayBe.HasValue && predicate(mayBe.Value) ? mayBe : MayBe<T>.None;
        }
        public static MayBe<T> ThrowIfSome<T>(this MayBe<T> mayBe, Func<T, Exception> exceptionFactory)
        {
            if (mayBe.HasValue)
                throw exceptionFactory(mayBe.Value);

            return mayBe;
        }
        public static Result<T> ToResult<T>(this MayBe<T> maybe, string errorMessage)
        {
            return maybe.HasValue
                ? Result<T>.Success(maybe.Value)
                : Result<T>.Failure(errorMessage);
        }
    }
}
