using DataRetrievers.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataRetrievers.Tests.Internal
{
    public class PredicateGuardTests
    {
        [Test]
        [TestCaseSource(nameof(PositiveCases))]
        public void PredicateIsSupported_ShouldPass(Expression<Func<FakeProjection, bool>> predicate)
        {
            //arrange
            //act
            PredicateGuard.PredicateIsSupported(predicate, "predicate");
            //assert
        }


        [Test]
        [TestCaseSource(nameof(NegativeCases))]
        public void PredicateIsSupported_ShouldNotPass(Expression<Func<FakeProjection, bool>> predicate)
        {
            //arrange
            //act
            TestDelegate action = ()=> PredicateGuard.PredicateIsSupported(predicate, "predicate");

            //assert
            Assert.Catch(typeof(ArgumentException), action);
        }

        private static IEnumerable<Expression<Func<FakeProjection, bool>>> NegativeCases()
        {
            
            //...wrong comparison
            
            yield return p => p.ObjectProperty == null;
            yield return p => p.Id + p.Id > 0;
        }

        private static IEnumerable<Expression<Func<FakeProjection, bool>>> PositiveCases()
        {
            //...constant and constant comparison
            int intConstant = 3;

            yield return p => 3 == 3;
            yield return p => intConstant == 3;

            //...property and constant comparison 
            yield return p => p.Id == 3;
            yield return p => p.Id != 3;
            yield return p => p.Id < 3;
            yield return p => p.Id > 3;
            yield return p => p.Id <= 3;
            yield return p => p.Id >= 3;
            yield return p => 3 == p.Id;
            yield return p => 3 != p.Id;
            yield return p => 3 > p.Id;
            yield return p => 3 < p.Id;
            yield return p => 3 >= p.Id;
            yield return p => 3 <= p.Id;

            //...nullable simple type support
            yield return p => p.NullableIntProperty == 3;
            yield return p => p.NullableIntProperty != 3;
            yield return p => p.NullableIntProperty > 3;
            yield return p => p.NullableIntProperty < 3;
            yield return p => p.NullableIntProperty >= 3;
            yield return p => p.NullableIntProperty <= 3;

        }
    }
}
