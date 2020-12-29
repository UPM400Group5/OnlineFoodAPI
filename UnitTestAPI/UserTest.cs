using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI;
using OnlineFoodAPI.Controllers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Results;

namespace UnitTestAPI
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void GetUsersNotAdmin_ReturnsNull()
        {
            var controller = new UsersController();

            // Send id
            var result = controller.GetAllUsers(GetExistingNormalUser().id);

            Assert.IsNull(result);
        }
        [TestMethod]
        public void GetUsersAsAdmin_ReturnsNotNull()
        {
            var controller = new UsersController();

            // Send id
            var result = controller.GetAllUsers(GetExistingAdminUser().id);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CreateNewUserSuccessful_ReturnsContent() 
        {
         
            var controller = new UsersController();

            // Act
            var user = GetNewMockupUser();
            IHttpActionResult actionResult = controller.PostUser(user);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<User>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.IsNotInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult)); // Is not error
            Assert.AreEqual(createdResult.Content.username, user.username); // User is returned
        }
        [TestMethod]
        public void CreateNewUserBadPassword_ReturnsBadRequest() 
        {
            var controller = new UsersController();

            var user = GetNewMockupUser();
            user.password = "123"; // Bad password

            IHttpActionResult actionResult = controller.PostUser(user);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }
        [TestMethod]
        public void DeleteExistingUser_ReturnsOk()
        {
            //TODO: Assign id of user before starting
            int id = 5;
            var controller = new UsersController();

            IHttpActionResult actionResult = controller.DeleteUser(id);
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<User>));
        }
        [TestMethod]
        public void DeleteNonExistingUser_NotFound()
        {
            var controller = new UsersController();
            int id = GetNonExistingUser().id;
            IHttpActionResult actionResult = controller.DeleteUser(id); // Random number, no id should be this high yet...

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUserByID_UserExists()
        {
            var controller = new UsersController();
            IHttpActionResult actionResult = controller.GetUser(GetExistingNormalUser().id);       
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<User>));
        }
        [TestMethod]
        public void GetUserByID_UserDoNotExist()
        {
            var controller = new UsersController();
            int id = GetNonExistingUser().id;

            IHttpActionResult actionResult = controller.GetUser(id);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
        [TestMethod]
        public void UpdateUser_PasswordTooShort()
        {
            var controller = new UsersController();
            User user = GetExistingNormalUserForUpdating();
            user.password = "123";

            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }
        [TestMethod]
        public void UpdateUser_IdNotMatching()
        {
            var controller = new UsersController();
            User user = GetExistingNormalUserForUpdating();

            IHttpActionResult actionResult = controller.PutUser(8, user);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }
        [TestMethod]
        public void UpdateUser_UserDoNotExist()
        {
            var controller = new UsersController();
            User user = GetNonExistingUser();
      
            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        //TODO: Update fungerar ej
        [TestMethod]
        public void UpdateUser_ReturnsAccepted() 
        {
            var controller = new UsersController();
            User user = GetExistingNormalUserForUpdating();
            user.city = "Göteborg";
          

            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            var contentResult = actionResult as NegotiatedContentResult<User>;

            Assert.AreEqual(System.Net.HttpStatusCode.Accepted, contentResult.StatusCode);
        }


        #region Mockup Users
        private User GetNonExistingUser() 
        {
            return new User() { username = "UnitTestUser123456", password = "123456", role = "user", id = 10001};
        }
        private User GetNewMockupUser()
        {
            return new User() { username = "UnitTestUser123456", password = "123456", role = "user" };
        }
        private User GetExistingNormalUser()
        {
            return new User() { username = "mockeduser", password = "123456", id = 1, role = "user"};
        }
        private User GetExistingNormalUserForUpdating()
        {
            return new User() {id = 9, adress = "nygatan 13", password = "12314345", city = "Trollhättan", username = "Harald" };
        }
        private User GetExistingAdminUser()
        {
            return new User() { username = "potionseller", password = "123456", role = "admin", id = 2 };
        }
        #endregion
       
    }
}
