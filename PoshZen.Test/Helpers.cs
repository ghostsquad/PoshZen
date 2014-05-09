namespace PoshZen.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Helpers
    {
        public static string GetTestFolder()
        {
            string testRootFolder = string.Format("TestRoot-{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now);
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", testRootFolder);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }
    }
}
