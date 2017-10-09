//-----------------------------------------------------------------------
// <copyright file="IExecutor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System.Threading.Tasks;

namespace DipExecutor
{
    /// <summary>
    /// Executor interface for running a <see cref="Step"/> asynchronously.
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Runs a step asynchronously. 
        /// </summary>
        /// <param name="step">The step to run.</param>
        /// <returns>The step once completed.</returns>
        Task<Step> RunAsync(Step step);
    }
}
