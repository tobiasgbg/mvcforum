﻿using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class LoggedInAdmin : LoggedInUser
    {
        public LoggedInAdmin(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public AdminPage GoToAdminPage()
        {
            var myToolsMenu = WebDriver.FindElement(By.ClassName("mytoolslink"));
            myToolsMenu.Click();

            var adminLink = WebDriver.FindElement(By.CssSelector(".dropdown .auto-admin"));
            adminLink.Click();

            return new AdminPage(WebDriver);
        }
    }
}