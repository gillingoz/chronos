namespace Gillingoz.Chronos.Service
{
    public class UploadDataPackageSettings
    {
        public string JobId { get; set; }
        public string EnvironmentId { get; set; }
        public string DefinitionGroupId { get; set; }
        public string SourceFolder { get; set; }
        public string SentFolder { get; set; }
        public string Schedule { get; set; }
        public bool IsEnabled { get; set; }
    }
}
