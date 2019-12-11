// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Win32.SafeHandles;
using System;
using System.Threading;

namespace Microsoft.Dism
{
    /// <summary>
    /// Represents a base class progress made during time-consuming operations.
    /// </summary>
    /// <remarks>This class also acts as a wrapper to the native callback method and stores the user data given back to the original caller.</remarks>
    public abstract class DismProgressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DismProgressBase"/> class.
        /// </summary>
        /// <param name="userData">A custom object to pass to the callback.</param>
        internal DismProgressBase(object userData)
        {
            // Save the user data
            UserData = userData;
        }

        /// <summary>
        /// Gets the current progress value.
        /// </summary>
        public int Current
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the total progress value.
        /// </summary>
        public int Total
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the user defined object for the callback.
        /// </summary>
        public object UserData
        {
            get;
            protected set;
        }

        /// <summary>
        /// Called by the native DISM API during a time-consuming operation.
        /// </summary>
        /// <param name="current">The current progress value.</param>
        /// <param name="total">The total progress.</param>
        /// <param name="userData">Any user data associated with the callback.</param>
        internal virtual void DismProgressCallbackNative(UInt32 current, UInt32 total, IntPtr userData)
        {
            // Save the current progress
            Current = (int)current;

            // Save the total progress
            Total = (int)total;
        }
    }
}