namespace btr.nuna.Application
{
    public interface INunaCounterDal
    {
        string GetNewHexNumber(string prefix);
        void UpdateNewHexNumber(string prefix, string hexValue);
        void InsertNewHexNumber(string prefix, string hexValue);
    }
}