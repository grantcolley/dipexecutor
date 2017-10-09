//-----------------------------------------------------------------------
// <copyright file="IExecutorService.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore;

namespace DipExecutor.Service
{
    /// <summary>
    /// Interface for the <see cref="ExecutorService"/>.
    /// </summary>
    public interface IExecutorService
    {
        /// <summary>
        /// Runs a <see cref="WebHost"/>.
        /// </summary>
        /// <param name="url">The url for the web host.</param>
        void Run(string url);
    }
}
