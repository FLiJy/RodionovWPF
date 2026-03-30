using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR14.Pages;

namespace PR14.Tests
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void RegisterTestSuccess()
        {
            var page = new RegisterPage();

            bool result = page.Register("Test User", "test_user_123", "123");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegisterTestFail()
        {
            var page = new RegisterPage();

            bool result1 = page.Register("", "", "");
            bool result2 = page.Register("User", "admin", "123"); 

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }
    }
}
