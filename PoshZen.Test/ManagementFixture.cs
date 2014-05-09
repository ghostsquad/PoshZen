namespace PoshZen.Test
{
    using Microsoft.Practices.Unity;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class ManagementFixture
    {
        public IUnityContainer UnityContainer { get; private set; }

        public IFixture Fixture { get; private set; }

        public ManagementFixture()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());

            this.UnityContainer = new UnityContainer();            
        }        
    }
}