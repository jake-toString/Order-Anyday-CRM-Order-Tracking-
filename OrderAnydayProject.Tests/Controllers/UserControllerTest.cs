using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OrderAnydayProject.Controllers;
using System.Web.Mvc;
using OrderAnydayProject.Models;

namespace OrderAnydayProject.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTest
    {
        OrderAnyDayContext db = new OrderAnyDayContext();
        //[TestMethod()]
        //public void isAdminTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void IndexTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void IndexTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Get_UsersTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void DetailsTest()
        {
            var firstuser = (AspNetUser)(from u in db.AspNetUsers select u).FirstOrDefault();
            var controller = new AspNetUsersController();
            // Invoke controller's action method
            var result = controller.Details(firstuser.Id) as ViewResult;
            var user = (AspNetUser)result.ViewData.Model;
            Assert.AreEqual(firstuser.UserName, user.UserName);
        }

        //[TestMethod()]
        //public void CreateTest()
        //{
        //    Assert.Fail();
        //}
        
        [TestMethod()]
        public void EditTest()
        {
            var firstuser = (AspNetUser)(from u in db.AspNetUsers select u).FirstOrDefault();
            firstuser.FirstName = "Jonathan";
            firstuser.UserName = "jwidner2017";
            db.SaveChanges();
            var controller = new AspNetUsersController();
            // Invoke controller's action method
            var result = controller.Edit(firstuser.Id) as ViewResult;
            var user = (AspNetUser)result.ViewData.Model;
            Assert.AreEqual(firstuser.UserName, user.UserName);
        }

        //[TestMethod()]
        //public void EditTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DeleteTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DeleteConfirmedTest()
        //{
        //    Assert.Fail();
        //}
    }
}
