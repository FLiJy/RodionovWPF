using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR14.Pages;

namespace PR14.Tests
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new LoginPage();

            bool result = page.Auth("123123", "123");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            var page = new LoginPage();

            // ВАЖНО: укажи реальные данные из БД
            bool result = page.Auth("sa", "1");


            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthTestFail()
        {
            var page = new LoginPage();

            bool result1 = page.Auth("", "");
            bool result2 = page.Auth("wrong", "123");
            bool result3 = page.Auth("admin", "wrong");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }
    }
}
