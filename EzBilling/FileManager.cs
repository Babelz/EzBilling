using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace EzBilling
{
    public sealed class FileManager
    {
        public FileManager()
        {
        }

        public void CreateFileIfDoesNotExist(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }
        }
        public void CreateDirectoryIfDoesNotExist(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory/*, new DirectorySecurity(directory, AccessControlSections.All) */);
            }
        }
    }
}
