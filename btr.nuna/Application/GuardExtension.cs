using System;
using Dawn;

namespace btr.nuna.Application
{
    public static class GuardExtensions
    {
        public static ref readonly Guard.ArgumentInfo<string> ValidDate(in this Guard.ArgumentInfo<string> argument, string format)
        {
            var isValid = DateTime.TryParseExact(argument.Value, format,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _);
            if (!isValid)
                throw Guard.Fail(
                    new ArgumentException($"{argument.Name} is not valid {format} date time format."));
            return ref argument;
        }
        public static ref readonly Guard.ArgumentInfo<string> ValidTime(in this Guard.ArgumentInfo<string> argument)
        {
            var isValid = TimeSpan.TryParse(argument, out _);
            if (!isValid)
                throw Guard.Fail(
                    new ArgumentException($"{argument.Name} is not valid time format."));
            return ref argument;
        }
    }
}