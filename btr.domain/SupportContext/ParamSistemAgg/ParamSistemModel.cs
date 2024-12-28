namespace btr.domain.SupportContext.ParamSistemAgg
{
    public class ParamSistemModel : IParamSistemKey
    {
        public ParamSistemModel(string code)
        {
            ParamCode = code;
        }
        public ParamSistemModel()
        {
        }

        public string ParamCode { get; set; }
        public string ParamValue { get; set; }
    }

    public interface IParamSistemKey
    {
        string ParamCode { get; }
    }
}
