﻿using SEDC.NotesAppFluentApi.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Services.Tests
{
    [TestClass]
    public class ValuesServiceUnitTests
    {
        private readonly ValuesService _valuesService;

        public ValuesServiceUnitTests()
        {
            _valuesService = new ValuesService();
        }

        [TestMethod]
        public void SumPositiveNumbers_should_return_null_on_negative_input()
        {
            //Arange
            int num1 = -2;
            int num2 = 3;

            //Act
            int? result = _valuesService.SumPositiveNumbers(num1, num2);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SumPositiveNumbers_should_return_positiveNumber()
        {
            //Arange
            int num1 = 2;
            int num2 = 3;
            int expectedResult = 5;
            
            //Act
            int? result = _valuesService.SumPositiveNumbers(num1, num2);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void IsFirstNumLarger_should_return_true()
        {
            //Arange
            int num1 = 6;
            int num2 = 3;

            //Act
            bool result = _valuesService.IsFirstNumLarger(num1, num2);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFirstNumLarger_should_return_false()
        {
            //Arange
            int num1 = 6;
            int num2 = 7;

            //Act
            bool result = _valuesService.IsFirstNumLarger(num1, num2);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetNumberName_should_return_name()
        {
            //Arrange
            int num = 8;
            string expectedResult = "Eight";

            //Act
            string result = _valuesService.GetNumberName(num);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetNumberName_should_throw_ArgumentOutOfRangeException()
        {
            //Arrange
            int num = 43;

            //Act and assert
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>  _valuesService.GetNumberName(num));

        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))] //Assert
        [TestMethod]
        public void GetNumberName_should_throw_ArgumentOutOfRangeException_on_invalid_input()
        {
            //Arrange
            int num = 43;

            //Act
            _valuesService.GetNumberName(num);
        }
    }
}
