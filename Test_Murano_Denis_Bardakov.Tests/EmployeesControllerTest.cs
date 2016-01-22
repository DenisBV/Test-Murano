using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using Test_Murano_Denis_Bardakov.Controllers;
using Test_Murano_Denis_Bardakov.Models;

namespace Test_Murano_Denis_Bardakov.Tests
{
    [TestClass]
    public class EmployeesControllerTest
    {
        [TestMethod]
        public void IndexViewModelNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetEmployeesList()).Returns(new List<Employees>());
            EmployeesController controller = new EmployeesController(mock.Object);

            // Act
            ViewResult result = controller.Index(null, 1) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // arrange
            string expected = "Create";
            var mock = new Mock<IRepository>();
            Employees empl = new Employees();
            EmployeesController controller = new EmployeesController(mock.Object);
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // act
            ViewResult result = controller.Create(empl) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_RedirectToIndexView()
        {
            // arrange
            string expected = "Index";
            var mock = new Mock<IRepository>();
            Employees empl = new Employees();
            EmployeesController controller = new EmployeesController(mock.Object);

            // act
            RedirectToRouteResult result = controller.Create(empl) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePostAction_SaveModel()
        {
            // arrange
            var mock = new Mock<IRepository>();
            Employees empl = new Employees();
            EmployeesController controller = new EmployeesController(mock.Object);

            // act
            RedirectToRouteResult result = controller.Create(empl) as RedirectToRouteResult;

            // assert
            mock.Verify(a => a.Create(empl));
            mock.Verify(a => a.Save());
        }

        [TestMethod]
        public void EditViewModelNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetEmployeeById(5)).Returns(new Employees { });
            EmployeesController controller = new EmployeesController(mock.Object);

            // Act
            var result = controller.Edit(5) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
            mock.Verify(a => a.GetEmployeeById(5));
        }

        [TestMethod]
        public void EditPostAction_RedirectToIndexView()
        {
            // arrange
            string expected = "Index";
            var mock = new Mock<IRepository>();
            Employees empl = new Employees();
            EmployeesController controller = new EmployeesController(mock.Object);

            // act
            RedirectToRouteResult result = controller.Edit(empl) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeleteViewModelNotNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetEmployeeById(1)).Returns(new Employees { });
            EmployeesController controller = new EmployeesController(mock.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void DeleteById()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetEmployeesList()).Returns(new List<Employees>());
            EmployeesController controller = new EmployeesController(mock.Object);

            // Act
            controller.DeleteConfirmed(1);

            var employees = mock.Object.GetEmployeesList();
            var result = mock.Object.GetEmployeeById(1);

            // Assert
            CollectionAssert.DoesNotContain(employees, result);
            mock.Verify(a => a.Delete(1));
        }
    }
}
