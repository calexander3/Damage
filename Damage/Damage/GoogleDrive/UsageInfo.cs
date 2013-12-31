using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive
{
    internal class UsageInfo
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public string name { get; set; }
        public User user { get; set; }
        public string quotaBytesTotal { get; set; }
        public string quotaBytesUsed { get; set; }
        public string quotaBytesUsedAggregate { get; set; }
        public string quotaBytesUsedInTrash { get; set; }
        public string largestChangeId { get; set; }
        public string rootFolderId { get; set; }
        public string domainSharingPolicy { get; set; }
        public string permissionId { get; set; }
        public List<ImportFormat> importFormats { get; set; }
        public List<ExportFormat> exportFormats { get; set; }
        public List<AdditionalRoleInfo> additionalRoleInfo { get; set; }
        public List<Feature> features { get; set; }
        public List<MaxUploadSize> maxUploadSizes { get; set; }
        public bool isCurrentAppInstalled { get; set; }



        public class Picture
        {
            public string url { get; set; }
        }

        public class User
        {
            public string kind { get; set; }
            public string displayName { get; set; }
            public Picture picture { get; set; }
            public bool isAuthenticatedUser { get; set; }
            public string permissionId { get; set; }
        }

        public class ImportFormat
        {
            public string source { get; set; }
            public List<string> targets { get; set; }
        }

        public class ExportFormat
        {
            public string source { get; set; }
            public List<string> targets { get; set; }
        }

        public class AdditionalRoleInfo
        {
            public string type { get; set; }
            public List<object> roleSets { get; set; }
        }

        public class Feature
        {
            public string featureName { get; set; }
            public double? featureRate { get; set; }
        }

        public class MaxUploadSize
        {
            public string type { get; set; }
            public string size { get; set; }
        }
    }
}
