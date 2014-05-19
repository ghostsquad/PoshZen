namespace PoshZen.Test.Unit.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Practices.Unity;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;

    public class CmdletTestBase<TModel, TInterface, TManagerInterface> : ScriptCsCmdletTestBase
        where TInterface : class, ITrackable
        where TModel : TrackableZendeskThingBase, TInterface
        where TManagerInterface : class, IManager<TInterface>
    {
        internal Mock<IEnvironment> EnvironmentMock { get; private set; }

        protected IUnityContainer Container { get; private set; }

        protected IFixture Fixture { get; private set; }

        protected Mock<TManagerInterface> ManagerMock { get; private set; }        

        protected CmdletTestBase()
        {            
            this.EnvironmentMock = new Mock<IEnvironment>();
            this.EnvironmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);

            this.Container = new UnityContainer();
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());

            this.ManagerMock = new Mock<TManagerInterface>();

            this.Container.RegisterType<TInterface, TModel>();
            this.Container.RegisterInstance(this.ManagerMock.Object);

            PoshZenContainer poshZenContainer = PoshZenContainer.Create(this.EnvironmentMock.Object, this.Container);
            poshZenContainer.Client = Mock.Of<ZendeskClientBase>();
        }
    }
}
