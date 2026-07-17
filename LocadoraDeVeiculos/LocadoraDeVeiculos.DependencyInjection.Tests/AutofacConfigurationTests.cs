using Autofac;
using LocadoraDeVeiculos.WindowsApp;
using LocadoraDeVeiculos.WindowsApp.Features.Login;
using LocadoraDeVeiculos.WindowsApp.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocadoraDeVeiculos.DependencyInjection.Tests
{
    [TestClass]
    public class AutofacConfigurationTests
    {
        [TestMethod]
        public void DevePossuirTelaLoginRegistradaNoContainer()
        {
            bool estaRegistrada =
                AutoFac.Container.IsRegistered<TelaLogin>();

            Assert.IsTrue(estaRegistrada);
        }

        [TestMethod]
        public void DevePossuirTelaPrincipalRegistradaNoContainer()
        {
            bool estaRegistrada =
                AutoFac.Container.IsRegistered<TelaPrincipalForm>();

            Assert.IsTrue(estaRegistrada);
        }

        [TestMethod]
        public void DeveResolverTelaLoginPeloContainer()
        {
            using ILifetimeScope scope =
                AutoFac.Container.BeginLifetimeScope();

            using TelaLogin telaLogin =
                scope.Resolve<TelaLogin>();

            Assert.IsNotNull(telaLogin);
        }

        [TestMethod]
        public void DeveResolverTelaPrincipalPeloContainer()
        {
            using ILifetimeScope scope =
                AutoFac.Container.BeginLifetimeScope();

            using TelaPrincipalForm telaPrincipal =
                scope.Resolve<TelaPrincipalForm>();

            Assert.IsNotNull(telaPrincipal);
        }

        [TestMethod]
        public void DeveResolverFactoryDaTelaPrincipal()
        {
            using ILifetimeScope scope =
                AutoFac.Container.BeginLifetimeScope();

            Func<TelaPrincipalForm> factory =
                scope.Resolve<Func<TelaPrincipalForm>>();

            Assert.IsNotNull(factory);

            using TelaPrincipalForm telaPrincipal = factory();

            Assert.IsNotNull(telaPrincipal);
        }

        [TestMethod]
        public void FactoryDeveCriarInstanciasDiferentesDaTelaPrincipal()
        {
            using ILifetimeScope scope =
                AutoFac.Container.BeginLifetimeScope();

            Func<TelaPrincipalForm> factory =
                scope.Resolve<Func<TelaPrincipalForm>>();

            using TelaPrincipalForm primeiraTela = factory();
            using TelaPrincipalForm segundaTela = factory();

            Assert.AreNotSame(primeiraTela, segundaTela);
        }
    }
}