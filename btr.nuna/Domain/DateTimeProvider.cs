using System;

namespace btr.nuna.Domain
{
    public class DateTimeProvider
    {
        public virtual DateTime Now { get; } = DateTime.Now;
    }
}