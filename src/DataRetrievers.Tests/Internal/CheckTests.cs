using DataRetrievers.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRetrievers.Tests.Internal
{
    [TestFixture]
    public class CheckTests
    {
        [Test]
        public void IsPropertyAccessExpression_ShouldReturnTrue_When_ExpressionIsPropertyAccessExpression()
        {
            //arrange
            //act
            var result = Check.IsPropertyAccessExpression<String, int>(s => s.Length);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsPropertyAccessExpression_ShouldReturnFalse_When_ExpressionIsCallExpression()
        {
            //arrange
            //act
            var result = Check.IsPropertyAccessExpression<String, String>(s => s.Trim());

            //assert
            Assert.IsFalse(result);
        }


    }
}
