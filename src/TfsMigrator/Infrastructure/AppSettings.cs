using System.Collections.Generic;
using TfsMigrator.Infrastructure;

namespace TfsMigration.Infrastructure
{
    public class AppSettings
    {
        public string CloudBaseAddress { get; set; }
        public Security CloudSecurity { get; set; }
        public string DatabaseConnection { get; set; }
        public string OnSiteBaseAddress { get; set; }
        public List<string> Projects { get; set; }
        public Security Security { get; set; }
        public WorkItemSetting WorkItemSettings { get; set; }
    }
}