using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UserProfileDemo.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void SaveDataTest()
        {
            //Arrange
            Login(TestData.TESTUSER1, TestData.TESTPASSWORD);

            //ACT
            FillProfile(TestData.TESTNAME, TestData.TESTADDRESS);
            app.WaitForElement(c => c.Marked(TestHelper.SIGNOUTBUTTON));
            app.Tap(c => c.Marked(TestHelper.SIGNOUTBUTTON));

            //ASSERT
            //log into the second test user to clear fields and make sure form is reset
            Login(TestData.TESTUSER2, TestData.TESTPASSWORD);
            ValidateProfileValues("", "");
            app.WaitForElement(c => c.Marked(TestHelper.SIGNOUTBUTTON));
            app.Tap(c => c.Marked(TestHelper.SIGNOUTBUTTON));

            //log in as original user and validate values are set
            Login(TestData.TESTUSER1, TestData.TESTPASSWORD);
            ValidateProfileValues(TestData.TESTNAME, TestData.TESTADDRESS);
            app.WaitForElement(c => c.Marked(TestHelper.SIGNOUTBUTTON));
            app.Tap(c => c.Marked(TestHelper.SIGNOUTBUTTON));
        }

        private void ValidateProfileValues(string fullName, string address)
        {
            app.WaitForElement(c => c.Text(TestHelper.PROFILEHEADER));

            var nameValue = app.Query(c => c.Marked(TestHelper.NAMEFIELD)).FirstOrDefault().Text;
            var addressValue = app.Query(c => c.Marked(TestHelper.ADDRESSFIELD)).FirstOrDefault().Text;
            Assert.AreEqual(fullName, nameValue);
            Assert.AreEqual(address, addressValue);
        }

        private void FillProfile(string fullName, string address)
        {
            app.WaitForElement(c => c.Text(TestHelper.PROFILEHEADER));

            app.Screenshot("Your Profile Before");
            app.ClearText(c => c.Marked(TestHelper.NAMEFIELD));
            app.DismissKeyboard();
            app.ClearText(c => c.Marked(TestHelper.ADDRESSFIELD));
            app.DismissKeyboard();

            app.EnterText(c => c.Marked(TestHelper.NAMEFIELD), TestData.TESTNAME);
            app.DismissKeyboard();
            app.EnterText(c => c.Marked(TestHelper.ADDRESSFIELD), TestData.TESTADDRESS);
            app.DismissKeyboard();

            //todo - add image selection support

            //select university
            SelectUniversity(TestData.TESTUNIVERSITYSEARCHNAME, TestData.TESTUNIVERSITYCSEARCHCOUNTRY);

            //wait for the university screen to leave and profile screen to return
            app.WaitForElement(c => c.Text(TestHelper.PROFILEHEADER));

            app.Tap(c => c.Marked(TestHelper.SAVEBUTTON));
            app.DismissKeyboard();

            app.WaitForElement(c => c.Text(TestHelper.OKBUTTON));
            app.Tap(c => c.Marked(TestHelper.OKBUTTON));
        }

        private void SelectUniversity(string searchName, string searchCountry)
        {
            app.Tap(c => c.Text("Select University"));
            //wait for search results
            System.Threading.Tasks.Task.Delay(3000);
            app.WaitForElement(c => c.Marked(TestHelper.LOOKUPBUTTON));

            //fill search name
            app.Tap(c => c.Marked(TestHelper.NAMESEARCHBAR));
            app.EnterText(c => c.Marked(TestHelper.NAMESEARCHBAR), TestData.TESTUNIVERSITYSEARCHNAME);
            app.DismissKeyboard();

            app.Tap(c => c.Marked(TestHelper.COUNTRYSEARCHBAR));
            app.EnterText(c => c.Marked(TestHelper.COUNTRYSEARCHBAR), TestData.TESTUNIVERSITYCSEARCHCOUNTRY);
            app.DismissKeyboard();

            app.Tap(c => c.Marked(TestHelper.LOOKUPBUTTON));

            //wait for search results
            System.Threading.Tasks.Task.Delay(5000);

            app.Tap(c => c.Text("Harvard University"));
        }

        private void Login(string username, string password)
        {
            app.WaitForElement(c => c.Marked(TestHelper.USERNAMEFIELD));
            app.Screenshot("Login Screen");
            app.EnterText(c => c.Marked(TestHelper.USERNAMEFIELD), username);
            app.EnterText(c => c.Marked(TestHelper.PASSWORDFIELD), password);
            app.Screenshot("Login Screen Filled Out");
            app.Tap(c => c.Marked(TestHelper.SIGNINBUTTON));
        }
    }
}