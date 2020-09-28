namespace Gillingoz.Chronos.Service
{
    public class CreateDataPackageSettings
    {
        public string JobId { get; set; }
        public string DefinitionGroupId { get; set; }
        public string SourceFolder { get; set; }
        public string TemplateFolder { get; set; }
        public string DestinationFolder { get; set; }
        public string DataFileName { get; set; }
        public string XsltFileName { get; set; }
        public string Schedule { get; set; }
        public bool IsEnabled { get; set; }
    }
}
