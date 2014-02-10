using System.Collections.Generic;

namespace GoogleDrive
{
    internal class FileList
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public string nextPageToken { get; set; }
        public string nextLink { get; set; }
        public List<DriveFile> items { get; set; }


        public class Labels
        {
            public bool starred { get; set; }
            public bool hidden { get; set; }
            public bool trashed { get; set; }
            public bool restricted { get; set; }
            public bool viewed { get; set; }
        }

        public class Parent
        {
            public string kind { get; set; }
            public string id { get; set; }
            public string selfLink { get; set; }
            public string parentLink { get; set; }
            public bool isRoot { get; set; }
        }

        public class UserPermission
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public string selfLink { get; set; }
            public string role { get; set; }
            public string type { get; set; }
        }

        public class Picture
        {
            public string url { get; set; }
        }

        public class Owner
        {
            public string kind { get; set; }
            public string displayName { get; set; }
            public Picture picture { get; set; }
            public bool isAuthenticatedUser { get; set; }
            public string permissionId { get; set; }
        }

        public class Picture2
        {
            public string url { get; set; }
        }

        public class LastModifyingUser
        {
            public string kind { get; set; }
            public string displayName { get; set; }
            public Picture2 picture { get; set; }
            public bool isAuthenticatedUser { get; set; }
            public string permissionId { get; set; }
        }

        public class DriveFile
        {
            public string kind { get; set; }
            public string id { get; set; }
            public string etag { get; set; }
            public string selfLink { get; set; }
            public string webContentLink { get; set; }
            public string alternateLink { get; set; }
            public string iconLink { get; set; }
            public string title { get; set; }
            public string mimeType { get; set; }
            public Labels labels { get; set; }
            public string createdDate { get; set; }
            public string modifiedDate { get; set; }
            public string modifiedByMeDate { get; set; }
            public List<Parent> parents { get; set; }
            public UserPermission userPermission { get; set; }
            public string originalFilename { get; set; }
            public string fileExtension { get; set; }
            public string md5Checksum { get; set; }
            public string fileSize { get; set; }
            public string quotaBytesUsed { get; set; }
            public List<string> ownerNames { get; set; }
            public List<Owner> owners { get; set; }
            public string lastModifyingUserName { get; set; }
            public LastModifyingUser lastModifyingUser { get; set; }
            public bool editable { get; set; }
            public bool copyable { get; set; }
            public bool writersCanShare { get; set; }
            public bool shared { get; set; }
            public bool explicitlyTrashed { get; set; }
            public bool appDataContents { get; set; }
            public string headRevisionId { get; set; }
            public string lastViewedByMeDate { get; set; }
        }
    }
}
