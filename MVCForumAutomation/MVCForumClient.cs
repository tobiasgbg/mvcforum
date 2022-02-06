﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace MVCForumAutomation
{
    public class MVCForumClient
    {
        private readonly TestDefaults _testDefaults;
        private readonly ChromeDriver _webDriver;

        public MVCForumClient(TestDefaults testDefaults)
        {
            _testDefaults = testDefaults;
            // TODO: select the type of browser and the URL from a configuration file
            _webDriver = new ChromeDriver();
            _webDriver.Url = "http://localhost:8080";
        }

        ~MVCForumClient()
        {
            _webDriver.Quit();
        }

        private RegistrationPage GoToRegistrationPage()
        {
            var registerLink = _webDriver.FindElement(By.ClassName("auto-register"));
            registerLink.Click();
            return new RegistrationPage(_webDriver);
        }

        public LoggedInUser RegisterNewUserAndLogin()
        {
            var username = Guid.NewGuid().ToString();
            const string password = "123456";
            const string email = "abc@def.com";

            var registrationPage = GoToRegistrationPage();
            registrationPage.Username = username;
            registrationPage.Password = password;
            registrationPage.ConfirmPassword = password;
            registrationPage.Email = email;

            registrationPage.Register();

            return new LoggedInUser(_webDriver);
        }

        public LoggedInAdmin LoginAsAdmin()
        {
            return LoginAs(_testDefaults.AdminUsername, _testDefaults.AdminPassword, () => new LoggedInAdmin(_webDriver));
        }

        private LoginPage GoToLoginPage()
        {
            var logonLink = _webDriver.FindElement(By.ClassName("auto-logon"));
            logonLink.Click();

            return new LoginPage(_webDriver);
        }

        public LatestDiscussions LatestDiscussions
        {
            get { throw new NotImplementedException(); }
        }

        private TLoggedInUser LoginAs<TLoggedInUser>(string username, string password, Func<TLoggedInUser> createLoggedInUser)
            where TLoggedInUser : LoggedInUser
        {
            var loginPage = GoToLoginPage();
            loginPage.Username = username;
            loginPage.Password = password;
            loginPage.LogOn();

            return createLoggedInUser();
        }
    }
}