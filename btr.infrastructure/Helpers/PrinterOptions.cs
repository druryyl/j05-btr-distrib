namespace btr.infrastructure.Helpers
{
    public class PrinterOptions
    {
        public const string SECTION_NAME = "Printer";

        public PrinterOptions()
        {
            Faktur = string.Empty;
            TempFile = string.Empty;
        }

        public string Faktur { get; set; }
        public string TempFile { get; set; }
    }
}
