using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR14.Pages;
using System;

namespace PR14.UnitTests
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void RegisterTestSuccess()
        {
            var registerPage = new RegisterPage();

            // Генерируем уникальный логин, чтобы тест не падал на "уже существует"
            string uniqueLogin = "testuser_" + Guid.NewGuid().ToString().Substring(0, 8);

            bool result = registerPage.Register("Тестовый Пользователь", uniqueLogin, "TestPass123");

            Assert.IsTrue(result, "Регистрация с корректными данными должна пройти успешно");
        }

        [TestMethod]
        public void RegisterTestFail()
        {
            var registerPage = new RegisterPage();

            // Негативные сценарии
            Assert.IsFalse(registerPage.Register("", "login123", "pass123"), "Пустое ФИО");
            Assert.IsFalse(registerPage.Register("Имя Фамилия", "", "pass123"), "Пустой логин");
            Assert.IsFalse(registerPage.Register("Имя Фамилия", "login123", ""), "Пустой пароль");

            // Проверка на уже существующий логин (замени на реальный логин из твоей БД)
            Assert.IsFalse(registerPage.Register("Имя Фамилия", "admin", "123"),
                "Регистрация с уже существующим логином должна провалиться");
        }
    }
}