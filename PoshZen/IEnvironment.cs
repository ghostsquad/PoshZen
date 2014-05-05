namespace PoshZen
{
    internal interface IEnvironment
    {
        #region Public Properties

        string ApplicationDataFolder { get; }

        #endregion

        #region Public Methods and Operators

        void CreateDirectoryIfNotExists(string path);

        string ReadFile(string filepath);

        void WriteContents(string filepath);

        #endregion
    }
}