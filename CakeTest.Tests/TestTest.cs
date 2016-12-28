using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CakeTest.Tests
{
    [TestFixture]
    public class TestTest
    {
        [Test]
        public void TestNumberOne()
        {
            Assert.True(true);
        }
    }

    [Binding]
    public class DoMathSteps
    {
        [Given(@"I have 2")]
        public void IHaveTwo()
        {
            ScenarioContext.Current["FirstNumber"] = 2;
        }

        [When(@"I add 3")]
        public void WhenIAddThree()
        {
            ScenarioContext.Current["SecondNumber"] = 3;
        }

        [Then("I have 5")]
        public void ThenIHaveFive()
        {
            var firstNumber = (int)ScenarioContext.Current["FirstNumber"];
            var secondNumber = (int)ScenarioContext.Current["SecondNumber"];
            Assert.IsTrue(firstNumber + secondNumber == 5);
        }
    }
}
