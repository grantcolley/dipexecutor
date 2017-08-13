//-----------------------------------------------------------------------
// <copyright file="IRunner.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace DipRunner
{
    /// <summary>
    /// The interface that must be implemented by the <see cref="Step.TargetType"/>
    /// in the <see cref="Step.TargetAssembly"/>.
    /// </summary>
    public interface IRunner
    {
        /// <summary>
        /// Executed by the step.
        /// </summary>
        /// <param name="step">The step that is being executed.</param>
        /// <returns>The step that has been executed.</returns>
        Task<Step> RunAsync(Step step);
    }
}