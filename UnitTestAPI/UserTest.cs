﻿using System;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI;
using OnlineFoodAPI.Controllers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Results;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTestAPI
{
    
    public class UserTest
    {
        User user;
        UsersController controller = new UsersController();
        [SetUp]
        public void Setup()
        {
            user = GetNewMockupUser();
            var result = controller.PostUser(user);
            List<User> listuser = controller.GetAllUsers(2).ToList();
            var tempuser = listuser[(listuser.Count() - 1)];
            user.id = tempuser.id;

        }
        [TearDown]
        public void Teardown()
        {
            try
            {
                controller.ModelState.Clear();
                var result =
                    controller.DeleteUser(user.id);
            }
            catch (Exception e)
            {

            }
           

        }
        [Test]
        public void GetUsersNotAdmin_ReturnsNull()
        {

            // Send id
            var result = controller.GetAllUsers(GetExistingNormalUser().id);

            Assert.IsNull(result);
        }
        [Test]
        public void GetUsersAsAdmin_ReturnsNotNull()
        {

            var result = controller.GetAllUsers(GetExistingAdminUser().id);

            Assert.IsNotNull(result);
        }
        [Test]
        public void CreateNewUserSuccessful_ReturnsContent() 
        {
         
            // Act
            IHttpActionResult actionResult = controller.PostUser(user);
            var createdResult = actionResult as OkNegotiatedContentResult<User>;

            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<User>>(actionResult);
            Assert.AreEqual(user.username, createdResult.Content.username); // User is returned
        }
        [Test]
         public void CreateNewUserBadPassword_ReturnsBadRequest() 
        {
            user.password = "123"; // Bad password

            IHttpActionResult actionResult = controller.PostUser(user);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        } 
        [Test]
        public void DeleteExistingUser_ReturnsOk()
        {
            //TODO: Assign id of user before starting
            IHttpActionResult actionResult = controller.DeleteUser(user.id);
            Assert.IsInstanceOf<OkNegotiatedContentResult<User>>(actionResult);
        }
        [Test]
        public void DeleteNonExistingUser_NotFound()
        {
            int id = Int32.MaxValue;
            IHttpActionResult actionResult = controller.DeleteUser(id); // Random number, no id should be this high yet...

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void GetUserByID_UserExists()
        {
            IHttpActionResult actionResult = controller.GetUser(GetExistingNormalUser().id);       
            Assert.IsInstanceOf<OkNegotiatedContentResult<User>>(actionResult);

        }
        [Test]
        public void GetUserByID_UserDoNotExist()
        {
            int id = GetNonExistingUser().id;

            IHttpActionResult actionResult = controller.GetUser(id);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        [Test]
        public void UpdateUser_PasswordTooShort()
        {
            User user = GetExistingNormalUserForUpdating();
            user.password = "123";

            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }
        [Test]
        public void UpdateUser_IdNotMatching()
        {
            User user = GetExistingNormalUserForUpdating();

            IHttpActionResult actionResult = controller.PutUser(8, user);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }
        [Test]
        public void UpdateUser_UserDoNotExist()
        {
            User user = GetNonExistingUser();
      
            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionResult);
        }
        public void UpdateUser_BadRequestModelNotValid()
        {
            User user = GetNonExistingUser();
            controller.ModelState.AddModelError("test", "test");
            IHttpActionResult actionResult = controller.PutUser(user.id, user);
            Assert.IsInstanceOf<InvalidModelStateResult>(actionResult);
        }

        //TODO: Update fungerar ej
        [Test]
        public void UpdateUser_ReturnsAccepted() 
        {
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
