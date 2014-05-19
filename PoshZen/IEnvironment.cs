namespace PoshZen
{
    internal interface IEnvironment
    {
        #region Public Properties

        string ApplicationDataFolder { get; }

        #endregion

        #region Public Methods and Operators

        void CreateDirectory(string path);

        string ReadFile(string path);

        void WriteContents(string path, string contents);

        bool FileExists(string path);

        #endregion
    }
}