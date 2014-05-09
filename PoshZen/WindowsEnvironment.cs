namespace PoshZen
{
    using System;
    using System.IO;

    internal class WindowsEnvironment : IEnvironment
    {
        #region Static Fields

        private static readonly object syncRoot = new object();

        private static volatile WindowsEnvironment instance;

        #endregion

        #region Constructors and Destructors

        private WindowsEnvironment()
        {
        }

        #endregion

        #region Public Properties

        public static WindowsEnvironment Default
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new WindowsEnvironment();
                        }
                    }
                }

                return instance;
            }
        }

        public string ApplicationDataFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteContents(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        #endregion
    }
}