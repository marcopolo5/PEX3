using AccountModule.Helpers;
using AccountModule.Tests.Mocks;
using Domain.HelpersContracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Tests
{
    [TestFixture]
    public class TokenFileSaverTests
    {
        private TokenFileSaver _tokenFileSaver;
        private IAppConfiguration _appCfg;
        [SetUp]
        public void SetUp()
        {
             _appCfg = new AppConfigurationMock();
            File.WriteAllText(_appCfg.GetTokenFileLocation(), "0");
            _tokenFileSaver = new TokenFileSaver(_appCfg);
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                File.Delete(_appCfg.GetTokenFileLocation());
            }
            catch(Exception e)
            {
            }
        }

        [Test]
        public void SaveToken_ValidInput_ReturnsTrue()
        {
            _tokenFileSaver.SaveToken("token");
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_tokenFileSaver.IsTokenSaved);
                Assert.IsTrue(_tokenFileSaver.GetToken() == "token");
            });
        }

        [Test]
        public void DeleteToken_TokenExists()
        {
            _tokenFileSaver.DeleteToken();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(_tokenFileSaver.IsTokenSaved);
                Assert.IsTrue(_tokenFileSaver.GetToken() == "0");
            });
        }
    }
}
