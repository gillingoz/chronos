namespace Gillingoz.Chronos.Service.Model
{
    public class ExtractDataPackageSettings
    {
        public string JobId { get; set; }
        public string DefinitionGroupId { get; set; }
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public string TempFolder { get; set; }
        public string ArchieveFolder { get; set; }
        public string[] DataFileName { get; set; }
        public string Schedule { get; set; }
        public bool IsEnabled { get; set; }

    }
}