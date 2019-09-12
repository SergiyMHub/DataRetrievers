using DataRetrievers.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static IEnumerable<Expression<Func<FakeProjection, bool>>> NegativeCases()
        {
            int intConstant = 3;

            //...wrong comparison
            yield return p => 3 == p.Id;
            yield return p => 3 != p.Id;
            yield return p => 3 > p.Id;
            yield return p => 3 < p.Id;
            yield return p => 3 >= p.Id;
            yield return p => 3 <= p.Id;

            //... p => 3==3
            yield return Expression.Lambda<Func<FakeProjection, bool>>(
                Expression.Equal(Expression.Constant(3), Expression.Constant(3)), 
                Expression.Parameter(typeof(FakeProjection))); 
            yield return p => intConstant == 3;

            yield return p => p.ObjectProperty == null;
            yield return p => p.Id + p.Id > 0;

            string stringConst = "Abra";
            yield return p => p.Name.StartsWith("Abra", StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.StartsWith(stringConst, StringComparison.OrdinalIgnoreCase);
            yield return p => p.Name.Contains("Abra", StringComparison.OrdinalIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static IEnumerable<Expression<Func<FakeProjection, bool>>> PositiveCases()
        {
            //...constant and constant comparison
            yield return p => true;



            //...boolean property
            yield return p => p.BoolProperty;
            yield return p => p.BoolProperty == true;

            //...negated bool property
            yield return p => !p.BoolProperty;
            //...property and constant comparison 
            yield return p => p.Id == 3;
            yield return p => p.Id != 3;
            yield return p => p.Id < 3;
            yield return p => p.Id > 3;
            yield return p => p.Id <= 3;
            yield return p => p.Id >= 3;
            

            //...nullable simple type support
            yield return p => p.NullableIntProperty == 3;
            yield return p => p.NullableIntProperty != 3;
            yield return p => p.NullableIntProperty > 3;
            yield return p => p.NullableIntProperty < 3;
            yield return p => p.NullableIntProperty >= 3;
            yield return p => p.NullableIntProperty <= 3;

            //...String.StartWith and String.Contains cases
            string stringConst = "Abra";
            bool booleanConst = true;
            yield return p => p.Name.StartsWith("Abra");
            yield return p => p.Name.StartsWith(stringConst);
            yield return p => p.Name.StartsWith("Abra") == true;
            yield return p => p.Name.StartsWith(stringConst) == booleanConst;

            yield return p => p.Name.Contains("Abra");
            yield return p => p.Name.Contains(stringConst);

        }
    }
}
