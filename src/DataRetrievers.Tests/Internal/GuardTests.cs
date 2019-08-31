using DataRetrievers.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRetrievers.Tests.Internal
{
    public class GuardTests
    {
        [Test]
        public void ArgumentHasNoNulls_ShouldPass_forNonNullItems()
        {
            //arrange
            var items = new []{"abra", "cadabra" };
            //act
            Guard.ArgumentHasNoNulls(items, "items");

            //assert
        }

        [Test]
        public void ArgumentHasNoNulls_ShouldThrowArgumentNullException_IfThereAreAnyNullElements()
        {
            //arrange
            var items = new[] { "abra", null, "cadabra" };

            //act
            TestDelegate action = () => Guard.ArgumentHasNoNulls(items, "items");

            //assert
            Assert.Catch(typeof(ArgumentNullException), action, "items[1]");
        }
    }
}
