﻿// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Dism.Properties;
using Shouldly;
using System;
using Xunit;

namespace Microsoft.Dism.Tests
{
    public class DismExceptionTest
    {
        [Fact]
        public void DismNotInitializedExceptionTest()
        {
            VerifyDismException<DismNotInitializedException>(DismApi.DISMAPI_E_DISMAPI_NOT_INITIALIZED, Resources.DismExceptionMessageNotInitialized);
        }

        [Fact]
        public void DismOpenSessionsExceptionTest()
        {
            VerifyDismException<DismOpenSessionsException>(DismApi.DISMAPI_E_OPEN_SESSION_HANDLES, Resources.DismExceptionMessageOpenSessions);
        }

        [Fact]
        public void DismRebootRequiredExceptionTest()
        {
            VerifyDismException<DismRebootRequiredException>(DismApi.ERROR_SUCCESS_REBOOT_REQUIRED, Resources.DismExceptionMessageRebootRequired);
        }

        [Fact]
        public void GetLastErrorMessageTest()
        {
            const string message = "Hello World";

            DismApi.GetLastErrorMessageTestHook = () => message;

            VerifyDismException<DismException>(DismApi.ERROR_OUTOFMEMORY, message);
        }

        [Theory]
        [InlineData(DismApi.ERROR_CANCELLED)]
        [InlineData(DismApi.ERROR_CANCELLED | 0x80070000)]
        [InlineData(DismApi.ERROR_REQUEST_ABORTED)]
        [InlineData(DismApi.ERROR_REQUEST_ABORTED | 0x80070000)]
        public void OperationCanceledExceptionTest(uint hresult)
        {
            const string errorMessage = "The operation was canceled.";

            DismApi.GetLastErrorMessageTestHook = () => null;

            Exception exception = DismException.GetDismExceptionForHResult((int)hresult);

            exception.ShouldBeOfType<OperationCanceledException>();

            exception.Message.ShouldBe(errorMessage);
        }

        [Fact]
        public void Win32ExceptionTest()
        {
            const int errorCode = unchecked((int)0x80020012);

            const string errorMessage = "Attempted to divide by zero.";

            DismApi.GetLastErrorMessageTestHook = () => null;

            Exception exception = DismException.GetDismExceptionForHResult(errorCode);

            exception.ShouldBeOfType<DivideByZeroException>();

            exception.Message.ShouldBe(errorMessage);
        }

        private void VerifyDismException<T>(uint errorCode, string message)
            where T : DismException
        {
            Exception exception = DismException.GetDismExceptionForHResult((int)errorCode);
            exception.ShouldBeOfType<T>();

            DismException dismException = (DismException)exception;

            exception.Message.ShouldBe(message);

            dismException.ErrorCode.ShouldBe((int)errorCode);
            dismException.NativeErrorCode.ShouldBe((int)errorCode);
        }
    }
}