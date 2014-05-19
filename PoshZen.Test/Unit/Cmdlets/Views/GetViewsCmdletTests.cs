namespace PoshZen.Test.Unit.Cmdlets.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.Practices.Unity;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using SharpZendeskApi;
    using SharpZendeskApi.Management;
    using SharpZendeskApi.Models;    

    using Xunit;

    public class GetViewsCmdletTests : ScriptCsCmdletTestBase
    {
        private readonly Mock<IEnvironment> environmentMock = new Mock<IEnvironment>();

        private readonly IUnityContainer container = new UnityContainer();

        private readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        public GetViewsCmdletTests()
        {            
            this.environmentMock.Setup(x => x.ApplicationDataFolder).Returns(AppDomain.CurrentDomain.BaseDirectory);
        }

        [Fact]
        public void GetViews_NoParam_Expect_GetViewAvailableCalled()
        {
            var clientMock = Mock.Of<ZendeskClientBase>();
            var listingFake = this.fixture.Create<IListing<IView>>();

            var managerMock = new Mock<IViewManager>();
            managerMock.Setup(x => x.GetAvailableViews(false))
                .Returns(listingFake)
                .Verifiable();

            this.container.RegisterType<IView, View>();
            this.container.RegisterInstance(managerMock.Object);

            var poshZenContainer = PoshZenContainer.Create(this.environmentMock.Object, this.container);
            poshZenContainer.Client = clientMock;

            // act
            var invocationData = this.Invoke("Get-Views");

            invocationData.Results.Should().HaveCount(1);
            var resultList = invocationData.Results[0].BaseObject as IList<IView>;
            resultList.Should().NotBeNull();
            resultList.Should().HaveCount(listingFake.Count);
            invocationData.ErrorRecords.Should().HaveCount(0);            
        }
    }
}
