﻿namespace Abbotware.Interop.NUnit
{
    using System;
    using global::NUnit.Framework;
    using global::NUnit.Framework.Interfaces;
    using global::NUnit.Framework.Internal;
    using global::NUnit.Framework.Internal.Commands;

    /// <summary>
    /// A simple ExpectedExceptionAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExpectedExceptionAttribute : NUnitAttribute, IWrapTestMethod
    {
        private static Type inconclusiveException = typeof(InconclusiveException);

        private readonly Type expectedExceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionAttribute"/> class.
        /// </summary>
        /// <param name="type">exception type</param>
        public ExpectedExceptionAttribute(Type type)
        {
            this.expectedExceptionType = type;
        }

        /// <inheritdoc/>
        public TestCommand Wrap(TestCommand command)
        {
            return new ExpectedExceptionCommand(command, this.expectedExceptionType);
        }

        private class ExpectedExceptionCommand : DelegatingTestCommand
        {
            private readonly Type expectedType;

            public ExpectedExceptionCommand(TestCommand innerCommand, Type expectedType)
                : base(innerCommand)
            {
                this.expectedType = expectedType;
            }

            public override TestResult Execute(TestExecutionContext context)
            {
                Type caughtType = null;

                try
                {
                    this.innerCommand.Execute(context);
                }
                catch (Exception ex)
                {
                    if (ex is NUnitException)
                    {
                        ex = ex.InnerException;
                    }

                    caughtType = ex.GetType();
                }

                if (caughtType == this.expectedType)
                {
                    context.CurrentResult.SetResult(ResultState.Success);
                }
                else if (caughtType == inconclusiveException)
                {
                    context.CurrentResult.SetResult(ResultState.Skipped);
                }
                else if (caughtType != null)
                {
                    context.CurrentResult.SetResult(ResultState.Failure, string.Format("Expected {0} but got {1}", this.expectedType.Name, caughtType.Name));
                }
                else
                {
                    context.CurrentResult.SetResult(ResultState.Failure, string.Format("Expected {0} but no exception was thrown", this.expectedType.Name));
                }

                return context.CurrentResult;
            }
        }
    }
}