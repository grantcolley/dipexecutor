//-----------------------------------------------------------------------
// <copyright file="StepStatus.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

namespace DipRunner
{
    /// <summary>
    /// Steps status.
    /// </summary>
    public enum StepStatus
    {
        /// <summary>
        /// Status unkown. The step has not yet started.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The step is initialising including validation and 
        /// downloading the <see cref="Step.TargetAssembly"/>
        /// and its <see cref="Step.Dependencies"/>.
        /// </summary>
        Initialise = 1,

        /// <summary>
        /// The step executes its <see cref="Step.TargetAssembly"/>
        /// followed by its <see cref="Step.SubSteps"/>.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The <see cref="Step.TargetAssembly"/> and <see cref="Step.SubSteps"/> 
        /// have been executed. The step is completed and then its <see cref="Step.TransitionSteps"/>
        /// are executed.
        /// </summary>
        Complete = 3
    }
}