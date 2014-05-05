namespace PoshZen
{
    using System;

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

        public void CreateDirectoryIfNotExists(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string filepath)
        {
            throw new NotImplementedException();
        }

        public void WriteContents(string filepath)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}