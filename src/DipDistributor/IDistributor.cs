//-----------------------------------------------------------------------
// <copyright file="IDistributor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System.Threading.Tasks;

namespace DipDistributor
{
    /// <summary>
    /// Distributor interface for running a <see cref="Step"/> asynchronously.
    /// </summary>
    public interface IDistributor
    {
        /// <summary>
        /// Runs a step asynchronously. 
        /// </summary>
        /// <param name="step">The step to run.</param>
        /// <returns>The step once completed.</returns>
        Task<Step> RunAsync(Step step);
    }
}
