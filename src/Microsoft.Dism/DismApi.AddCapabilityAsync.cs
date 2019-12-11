// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Dism
{
    /// <summary>
    /// Represents the main entry point into the Deployment Image Servicing and Management (DISM) API.
    /// </summary>
    public static partial class DismApi
    {
        /// <summary>
        /// Add a capability to an image.
        /// </summary>
        /// <param name="session">A valid DISM Session. The DISM Session must be associated with an image. You can associate a session with an image by using the <see cref="OpenOfflineSession(string)"/> method.</param>
        /// <param name="capabilityName">The name of the capability that is being added.</param>
        /// <exception cref="DismException">When a failure occurs.</exception>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation. return <see langword="true"/> if reboot required else <see langword="false"></see></returns>
        public static async Task<bool> AddCapabilityAsync(DismSession session, string capabilityName)
        {
           return await DismApi.AddCapabilityAsync(session, capabilityName, false, null, CancellationToken.None, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a capability to an image.
        /// </summary>
        /// <param name="session">A valid DISM Session. The DISM Session must be associated with an image. You can associate a session with an image by using the <see cref="OpenOfflineSession(string)"/> method.</param>
        /// <param name="capabilityName">The name of the capability that is being added.</param>
        /// <param name="limitAccess">The flag indicates whether WU/WSUS should be contacted as a source location for downloading the payload of a capability. If payload of the capability to be added exists, the flag is ignored.</param>
        /// <param name="sourcePaths">A list of source locations. The function shall look up removed payload files from the locations specified in SourcePaths, and if not found, continue the search by contacting WU/WSUS depending on parameter LimitAccess.</param>
        /// <exception cref="DismException">When a failure occurs.</exception>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation. return <see langword="true"/> if reboot required else <see langword="false"></see></returns>
        public static async Task<bool> AddCapabilityAsync(DismSession session, string capabilityName, bool limitAccess, List<string> sourcePaths)
        {
            return await DismApi.AddCapabilityAsync(session, capabilityName, limitAccess, sourcePaths, CancellationToken.None, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a capability to an image.
        /// </summary>
        /// <param name="session">A valid DISM Session. The DISM Session must be associated with an image. You can associate a session with an image by using the <see cref="OpenOfflineSession(string)"/> method.</param>
        /// <param name="capabilityName">The name of the capability that is being added.</param>
        /// <param name="limitAccess">The flag indicates whether WU/WSUS should be contacted as a source location for downloading the payload of a capability. If payload of the capability to be added exists, the flag is ignored.</param>
        /// <param name="sourcePaths">A list of source locations. The function shall look up removed payload files from the locations specified in SourcePaths, and if not found, continue the search by contacting WU/WSUS depending on parameter LimitAccess.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which permit to cancell the operation</param>
        /// <param name="progressCallback">A progress callback method to invoke when progress is made.</param>
        /// <param name="userData">Optional user data to pass to the DismProgressCallback method.</param>
        /// <exception cref="DismException">When a failure occurs.</exception>
        /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation. return <see langword="true"/> if reboot required else <see langword="false"></see></returns>
        public static async Task<bool> AddCapabilityAsync(DismSession session, string capabilityName, bool limitAccess, List<string> sourcePaths, CancellationToken token, IProgress<Microsoft.Dism.DismProgressAsync> progressCallback, object userData)
        {
            // Get the list of source paths as an array
            string[] sourcePathsArray = sourcePaths?.ToArray() ?? new string[0];

            // Create a DismProgress object to wrap the callback
            var progress = new DismProgressAsync(progressCallback, userData);
            int hresult = await Task.Run(() => NativeMethods.DismAddCapability(session, capabilityName, limitAccess, sourcePathsArray, (uint)sourcePathsArray.Length, token.CanBeCanceled ? token.WaitHandle.SafeWaitHandle : null, progress.DismProgressCallbackNative, IntPtr.Zero)).ConfigureAwait(false);
            if (hresult == ERROR_SUCCESS_REBOOT_REQUIRED)
            {
                return true;
            }

            token.ThrowIfCancellationRequested();
            DismUtilities.ThrowIfFail(hresult, session);
            return false;
        }
    }
}