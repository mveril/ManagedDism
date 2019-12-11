// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Win32.SafeHandles;
using System;
using System.Threading;

namespace Microsoft.Dism
{
    /// <summary>
    /// Represents progress made during time-consuming operations.
    /// </summary>
    /// <remarks>This class also acts as a wrapper to the native callback method and stores the user data given back to the original caller.</remarks>
    public sealed class DismProgressAsync : DismProgressBase
    {
        /// <summary>
        /// The users callback method.
        /// </summary>
        private readonly IProgress<DismProgressAsync> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="DismProgressAsync"/> class.
        /// </summary>
        /// <param name="callback">A <see cref="IProgress{DismProgressAsync}"/> to call when progress is made.</param>
        /// <param name="userData">A custom object to pass to the callback.</param>
        internal DismProgressAsync(IProgress<DismProgressAsync> callback, object userData)
            : base(userData)
        {
            // Save the managed callback method
            _callback = callback;
        }

        /// <inheritdoc/>
        internal override void DismProgressCallbackNative(uint current, uint total, IntPtr userData)
        {
            base.DismProgressCallbackNative(current, total, userData);
            _callback.Report(this);
        }
    }
}