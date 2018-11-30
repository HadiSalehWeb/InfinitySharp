using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfinitySharp;

namespace InfinitySharp.Test
{
    [TestClass]
    public class IntegerTest
    {
        private readonly Integer zero, one, two, ten, thou, inf, nan;

        public IntegerTest()
        {
            zero = 0;
            one = 1;
            two = 2;
            ten = 10;
            thou = 1000;
            inf = Integer.Infinity;
            nan = Integer.NaN;
            Integer.onOverflow = OnIntegerOverflow.Throw;
        }

        [TestMethod]
        public void TestAddition()
        {
            Assert.AreEqual(zero + one, one);
            Assert.AreEqual(one + one, two);
            Assert.AreEqual(zero + zero, zero);
        }

        [TestMethod]
        public void TestSubstraction()
        {
            Assert.AreEqual(two - one, one);
            Assert.AreEqual(one - zero, one);
            Assert.AreEqual(one - one, zero);
            Assert.AreEqual(zero - zero, zero);
        }

        [TestMethod]
        public void TestNegation()
        {
            Assert.AreEqual(-zero, zero);
            Assert.AreEqual(-one, -1);
            Assert.AreEqual(-thou, -1000);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            Assert.AreEqual(one * one, one);
            Assert.AreEqual(thou * zero, zero);
            Assert.AreEqual(ten * ten * ten, thou);
            Assert.AreEqual(two * one, two);
            Assert.AreEqual(-one * one, -one);
            Assert.AreEqual(-two * -one, two);
        }

        [TestMethod]
        public void TestDivision()
        {
            Assert.AreEqual(one / one, one);
            Assert.AreEqual(one / two, zero);
            Assert.AreEqual(zero / thou, zero);
            Assert.AreEqual(thou / (ten * ten), ten);
            Assert.AreEqual(zero / -one, zero);
            Assert.AreEqual(-zero / ten, zero);
            Assert.AreEqual(-ten / -one, ten);
            Assert.AreEqual(one / zero, inf);
        }

        [TestMethod]
        public void TestPower()
        {
            Assert.AreEqual(Integer.Pow(ten, two + one), thou);
            Assert.AreEqual(Integer.Pow(two, zero), one);
            Assert.AreEqual(Integer.Pow(ten, one), ten);
            Assert.AreEqual(Integer.Pow(one, thou), one);
        }

        [TestMethod]
        public void TestInfinity_add_sub()
        {
            Assert.AreEqual(inf + one, inf);
            Assert.AreEqual(inf - ten, inf);
            Assert.AreEqual(-inf + two, -inf);
            Assert.AreEqual(-inf - thou, -inf);
            Assert.AreEqual(inf + -one, inf);
            Assert.AreEqual(inf - -ten, inf);
            Assert.AreEqual(-inf + -two, -inf);
            Assert.AreEqual(-inf - -thou, -inf);
            Assert.AreEqual(inf + inf, inf);
            Assert.AreEqual(-inf + -inf, -inf);
            Assert.AreEqual(inf - inf, nan);
        }

        [TestMethod]
        public void TestInfinity_mult()
        {
            Assert.AreEqual(inf * one, inf);
            Assert.AreEqual(inf * -two, -inf);
            Assert.AreEqual(-inf * ten, -inf);
            Assert.AreEqual(-inf * -thou, inf);
            Assert.AreEqual(inf * inf, inf);
            Assert.AreEqual(inf * -inf, -inf);
            Assert.AreEqual(-inf * -inf, inf);
            Assert.AreEqual(zero * -inf, nan);
        }

        [TestMethod]
        public void TestInfinity_div()
        {
            Assert.AreEqual(two / zero, inf);
            Assert.AreEqual(-ten / zero, -inf);
            Assert.AreEqual(one / inf, zero);
            Assert.AreEqual(-two / inf, zero);
            Assert.AreEqual(ten / -inf, zero);
            Assert.AreEqual(-thou / -inf, zero);
            Assert.AreEqual(inf / one, inf);
            Assert.AreEqual(inf / -two, -inf);
            Assert.AreEqual(-inf / ten, -inf);
            Assert.AreEqual(-inf / -thou, inf);
            Assert.AreEqual(zero / inf, zero);
            Assert.AreEqual(zero / -inf, zero);
            Assert.AreEqual(inf / zero, inf);
            Assert.AreEqual(-inf / zero, -inf);
            Assert.AreEqual(zero / zero, nan);
            Assert.AreEqual(inf / inf, nan);
            Assert.AreEqual(inf / -inf, nan);
            Assert.AreEqual(-inf / inf, nan);
            Assert.AreEqual(-inf / -inf, nan);
        }

        [TestMethod]
        public void TestInfinity_pow()
        {
            Assert.AreEqual(Integer.Pow(one, zero), 1);
            Assert.AreEqual(Integer.Pow(-ten, zero), 1);
            Assert.AreEqual(Integer.Pow(thou, zero), 1);
            Assert.AreEqual(Integer.Pow(zero, zero), nan);
            Assert.AreEqual(Integer.Pow(inf, zero), nan);
            Assert.AreEqual(Integer.Pow(zero, one), zero);
            Assert.AreEqual(Integer.Pow(zero, -two), inf);
            Assert.AreEqual(Integer.Pow(two, inf), inf);
            Assert.AreEqual(Integer.Pow(one, inf), nan);
            Assert.AreEqual(Integer.Pow(zero, inf), zero);
            Assert.AreEqual(Integer.Pow(-ten, inf), -inf);
            Assert.AreEqual(Integer.Pow(two, -inf), zero);
            Assert.AreEqual(Integer.Pow(one, -inf), nan);
            Assert.AreEqual(Integer.Pow(zero, -inf), inf);
            Assert.AreEqual(Integer.Pow(-ten, -inf), zero);
            Assert.AreEqual(Integer.Pow(inf, inf), inf);
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            Assert.AreEqual(zero.GetHashCode(), 0);
            Assert.AreEqual(one.GetHashCode(), 1);
            Assert.AreEqual(thou.GetHashCode(), 1000);
            Assert.AreEqual(inf.GetHashCode(), int.MaxValue);
            Assert.AreEqual((-inf).GetHashCode(), int.MinValue);
            Assert.AreEqual(nan.GetHashCode(), 0);
        }

        [TestMethod]
        public void TestEquals()
        {
            Assert.AreEqual(zero == 0, true);
            Assert.AreEqual(one == 1, true);
            Assert.AreEqual(-thou == -1000, true);
            Assert.AreEqual(inf == Integer.Infinity, true);
            Assert.AreEqual(-inf == -Integer.Infinity, true);
            Assert.AreEqual(nan == Integer.NaN, false);
            Assert.AreEqual(zero == ten, false);
            Assert.AreEqual(two == -two, false);
            Assert.AreEqual(-thou == thou, false);
            Assert.AreEqual(inf == -inf, false);
            Assert.AreEqual(inf == zero, false);
            Assert.AreEqual(-inf == -thou, false);
            Assert.AreEqual(nan == zero, false);
            Assert.AreEqual(nan == one, false);
            Assert.AreEqual(nan == thou, false);
            Assert.AreEqual(nan == -two, false);
            Assert.AreEqual(nan == inf, false);
            Assert.AreEqual(nan == -inf, false);
        }

        //[TestMethod]
        //public void Test()
        //{
        //}
    }
}