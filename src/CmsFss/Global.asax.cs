using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Castle.Core;
using Castle.Facilities.FactorySupport;
using Castle.Facilities.WcfIntegration;
using Castle.Facilities.WcfIntegration.Rest;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using HealthBus.CmsFss2011;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace HealthBus.CmsFss
{
	public class Global : HttpApplication
	{
		public Global()
		{
			DocumentStore = new DocumentStore {ConnectionStringName = "RavenDB"};
			DocumentStore.Initialize();

			IndexCreation.CreateIndexes(typeof(Global).Assembly, DocumentStore);

			Container = new WindsorContainer()
				.AddFacility<WcfFacility>();
			Container.Kernel.AddFacility("factory", new FactorySupportFacility());

			Container
				.Register(
					Component.For<IDocumentStore>()
						.Instance(DocumentStore),
					Component.For<IDocumentSession>()
						.UsingFactoryMethod(() => CurrentSession)
						.LifeStyle.PerWebRequest,
					Component.For<ICmsFssNonFacility2011d>()
						.Named("HealthBus.CmsFss.CmsFssNonFacility2011d")
						.ImplementedBy<CmsFssNonFacility2011d>()
						.LifeStyle.Is(LifestyleType.Transient),
					Component.For<ICmsFssFacility2011d>()
						.Named("HealthBus.CmsFss.CmsFssFacility2011d")
						.ImplementedBy <CmsFssFacility2011d>()
						.LifeStyle.Is(LifestyleType.Transient),
					Component.For<ICmsFssNonFacility2012a>()
						.Named("HealthBus.CmsFss.CmsFssNonFacility2012a")
						.ImplementedBy<CmsFssNonFacility2012a>()
						.LifeStyle.Is(LifestyleType.Transient),
					Component.For<ICmsFssFacility2012a>()
						.Named("HealthBus.CmsFss.CmsFssFacility2012a")
						.ImplementedBy<CmsFssFacility2012a>()
						.LifeStyle.Is(LifestyleType.Transient));

			BeginRequest += (_, __) => { CurrentSession = DocumentStore.OpenSession(); };
			EndRequest += (_, __) =>
			              	{
			              		IDocumentSession session = CurrentSession;
			              		if (session != null)
			              		{
			              			session.Dispose();
			              		}
			              	};
		}

		protected static IDocumentSession CurrentSession
		{
			get { return (IDocumentSession) HttpContext.Current.Items["RavenSession"]; }
			set { HttpContext.Current.Items["RavenSession"] = value; }
		}

		protected IWindsorContainer Container { get; set; }

		protected DocumentStore DocumentStore { get; set; }

		private void Application_Start(object sender, EventArgs e)
		{
			var factory = new WindsorServiceHostFactory<RestServiceModel>(Container.Kernel);

			RouteTable.Routes.Add(new ServiceRoute("nonfacility_2011d",
				factory,
				typeof(ICmsFssNonFacility2011d)));
			RouteTable.Routes.Add(new ServiceRoute("facility_2011d",
				factory,
				typeof(ICmsFssFacility2011d)));
			RouteTable.Routes.Add(new ServiceRoute("nonfacility_2012a",
				factory,
				typeof(ICmsFssNonFacility2012a)));
			RouteTable.Routes.Add(new ServiceRoute("facility_2012a",
				factory,
				typeof(ICmsFssFacility2012a)));
		}

		private void Application_End(object sender, EventArgs e)
		{
			//  Code that runs on application shutdown
		}

		private void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs
		}

		private void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started
		}

		private void Session_End(object sender, EventArgs e)
		{
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.
		}
	}
}