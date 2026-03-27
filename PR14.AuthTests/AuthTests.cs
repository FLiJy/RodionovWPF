using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR14.Pages;

namespace PR14.UnitTests
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void AuthTest() // базовый тест из задания
        {
            var loginPage = new LoginPage();
            bool result = loginPage.Auth("", "");
            Assert.IsFalse(result, "Авторизация с пустыми полями должна возвращать false");
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            var loginPage = new LoginPage();

            // ←←← ИЗМЕНИ НА РЕАЛЬНЫЕ ДАННЫЕ ИЗ ТВОЕЙ ТАБЛИЦЫ USERS !!!
            bool result1 = loginPage.Auth("admin", "admin123");
            bool result2 = loginPage.Auth("user", "password");

            Assert.IsTrue(result1, "Авторизация с корректными данными admin должна пройти");
            Assert.IsTrue(result2, "Авторизация с корректными данными user должна пройти");
        }

        [TestMethod]
        public void AuthTestFail()
        {
            var loginPage = new LoginPage();

            // Негативные сценарии
            Assert.IsFalse(loginPage.Auth("", ""), "Пустые поля");
            Assert.IsFalse(loginPage.Auth("", "password"), "Пустой логин");
            Assert.IsFalse(loginPage.Auth("admin", ""), "Пустой пароль");
            Assert.IsFalse(loginPage.Auth("nonexistentuser", "12345"), "Несуществующий логин");
            Assert.IsFalse(loginPage.Auth("admin", "wrongpassword"), "Неверный пароль");
        }
    }
}