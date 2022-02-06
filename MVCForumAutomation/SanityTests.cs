using NUnit.Framework;
using System;

namespace MVCForumAutomation
{
    public class SanityTests
    {
        public SanityTests()
        {
            TestDefaults = new TestDefaults();
            MVCForum = new MVCForumClient(TestDefaults);
        }

        [SetUp]
        public void Setup()
        {
            var adminUser = MVCForum.LoginAsAdmin();
            var adminPage = adminUser.GoToAdminPage();
            var permissions = adminPage.GetPermissionsFor(TestDefaults.
                StandardMembers);
            permissions.AddToCategory(TestDefaults.ExampleCategory,
                PermissionTypes.CreateTopics);
            adminUser.Logout();
        }

        [Test]
        public void WhenARegisteredUserStartsADiscussionOtherAnonymousUsersCanSeeIt()
        {
            const string body = "dummy body";
            LoggedInUser userA = MVCForum.RegisterNewUserAndLogin();
            Discussion createdDiscussion = userA.
                CreateDiscussion(Discussion.With.Body(body));
            MVCForumClient anonymousUser = new MVCForumClient(TestDefaults);
            DiscussionHeader latestHeader = anonymousUser.
                LatestDiscussions.Top;
            Assert.AreEqual(createdDiscussion.Title, latestHeader.Title,
            "The title of the latest discussion should match the one we created");

            Discussion viewedDiscussion = latestHeader.OpenDiscussion();
            Assert.AreEqual(body, viewedDiscussion.Body,
            "The body of the latest discussion should match the one we created");
        }
        public TestDefaults TestDefaults { get; }

        public MVCForumClient MVCForum { get; }
    }
}