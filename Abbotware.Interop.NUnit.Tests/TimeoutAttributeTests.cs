﻿namespace Abbotware.Interop.NUnit.Tests
{
    using System;
    using System.Threading;
    using Abbotware.Interop.NUnit;
    using global::NUnit.Framework;

    [TestFixture]
    public class TimeoutAttributeTests 
    {
        [Test]
        [Category("Interop.NUnit.UnitTests")]
        [Timeout(2)]
        [ExpectedException(typeof(TimeoutException))]
        public void Timeout_Thrown()
        {
            Thread.Sleep(4000);
        }

        [Test]
        [Category("Interop.NUnit.UnitTests")]
        [Timeout(2)]
        public void NoTimeout()
        {
        }

        [Test]
        [Category("Interop.NUnit.UnitTests")]
        [Timeout(10)]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpectedExceptionAttribute_AfterTimeout()
        {
            throw new ArgumentException();
        }

        [Test]
        [Category("Interop.NUnit.UnitTests")]
        [ExpectedException(typeof(ArgumentException))]
        [Timeout(10)]
        public void ExpectedExceptionAttribute_BeforeTimeout()
        {
            throw new ArgumentException();
        }
    }
}