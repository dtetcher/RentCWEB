using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentC.WebUI.Controllers;
using RentC.WebUI.Infrastructure.Abstract;
using RentC.WebUI.Models.DAL;
using RentC.WebUI.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace RentC.Tests.Controllers
{
    [TestClass]
    class RegisterTest
    {
        [TestMethod]
        public void CurrencyMoreThan500()
        {
            // Arrange
            var model = new CarRegisterEditVM()
            {
                LocationZipCode = 12345,
                DailyPrice = 646.0M,
                Plate = "AA 01 AAA",
                Manufacturer = "Man",
                Model = "Model"
            };
            var car_repo = UnityConfig.Container.Resolve<Repository<Car>>();
            var loc_repo = UnityConfig.Container.Resolve<Repository<Location>>();
            var controller = new CarController(car_repo, loc_repo);

            // Act
            var result = controller.Register(model).Result;

            // Assert
            
        }
    }
}
