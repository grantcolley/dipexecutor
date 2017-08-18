//-----------------------------------------------------------------------
// <copyright file="Step.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System;

namespace DipRunner
{
    /// <summary>
    /// A step to be processed in a workflow. A step is processed by executing 
    /// a <see cref="TargetType"/> associated with the step and / or its <see cref="SubSteps"/>.  
    /// After the step's <see cref="TargetType"/> and / or its <see cref="SubSteps"/> 
    /// have finished executing its <see cref="TransitionSteps"/> are run, thereby progressing
    /// along the workflow. When there are no more steps to process or transition to the workflow is complete.
    /// </summary>
    public class Step
    {
        private string stepUrl;

        /// <summary>
        /// Gets or sets the workflow identifier.
        /// </summary>
        public int RunId { get; set; }

        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        public string RunName { get; set; }

        /// <summary>
        /// Gets or sets the step identifier within the workflow.
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        /// Gets or sets the step name withing the workflow.
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// Gets or sets the payload (arguments) associated with the step.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the target type to be executed for the step. 
        /// The target type must implement the <see cref="IRun"/> interface. 
        /// The target type and its <see cref="Dependencies"/> is 
        /// dynamically loaded when the step is run and its <see cref="IRun.RunAsync"/>
        /// method is called and the step is passed into it. When <see cref="IRun.RunAsync"/>
        /// has finished executing the sub steps are executed.
        /// If <see cref="TargetType"/> is empty then at least one sub step must be provided.
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// Gets or sets the target assembly containing the <see cref="TargetType"/>.
        /// The target assembly must be the full path and file name of the assembly 
        /// e.g. "C:\workflow\step\targetassembly.dll".
        /// </summary>
        public string TargetAssembly { get; set; }

        /// <summary>
        /// Gets or sets the location where the <see cref="TargetAssembly"/> is downloaded to.
        /// </summary>
        public string TargetDownloadLocation { get; set; }

        /// <summary>
        /// Gets or sets the location of the log file associated with the step and or workflow.
        /// The location must include the path to the log file and the log file name.
        /// If one isn't provided then on calling <see cref="Validate"/> it will be set to "DistributorLog.txt" 
        /// and the file will get created in the directory of the executing assembly.
        /// If LogFileLocation is not provided and <see cref="Validate"/> is not called then 
        /// an exception may be thrown by the Distributor.
        /// </summary>
        public string LogFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
        public StepStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the list if dependencies required by the <see cref="TargetAssembly"/>.
        /// A dependency must be listed as the full path and file name of the dependency 
        /// e.g. "C:\workflow\step\dependency1.dll".
        /// </summary>
        public string[] Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the sub steps associated with the step. Sub steps are run 
        /// after the <see cref="TargetType"/> is executed. If there is more than 
        /// one sub step they are executed in parallel. When all the sub steps have
        /// completed then the <see cref="TransitionSteps"/> are executed.
        /// If no sub steps are provided then a <see cref="TargetType"/> must be provided.
        /// </summary>
        public Step[] SubSteps { get; set; }

        /// <summary>
        /// Gets or sets the transition steps. When a step has completed processing 
        /// i.e. its <see cref="TargetType"/> and / or 
        /// <see cref="SubSteps"/> have been executed, then the transition steps are run.
        /// When there are no more steps to process or transition to the workflow is complete.
        /// </summary>
        public Step[] TransitionSteps { get; set; }

        /// <summary>
        /// Gets or sets the url where the step will be run.
        /// StepUrl can be explicitly set to run on a specified url.
        /// If it is null or empty and the step is one of a parents 
        /// <see cref="SubSteps"/> or <see cref="TransitionSteps"/> then 
        /// the parent will set it dynamically at runtime (see <see cref="Urls"/>). 
        /// If StepUrl is null or empty and is not set by a parent step 
        /// then the first url in its <see cref="Urls"/> will be returned.
        /// </summary>
        public string StepUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(stepUrl))
                {
                    return Urls?[0];
                }

                return stepUrl;
            }
            set
            {
                stepUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the url providing the location of the dependencies referenced by the <see cref="TargetAssembly"/>.
        /// </summary>
        public string DependencyUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to be used for logging steps progress through the workflow.
        /// </summary>
        public string LogUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of urls availble for distributed processing of steps in the workflow.
        /// </summary>
        public string[] Urls { get; set; }

        /// <summary>
        /// <see cref="Validate(bool)"/>
        /// </summary>
        public void Validate()
        {
            Validate(false);
        }

        /// <summary>
        /// Validates the step for mandatory data. It does not check the validity of the data but merely confirms mandatory 
        /// fields are populated with data. A step will validate itself and then validate its <see cref="SubSteps"/>.
        /// 
        /// Validation Rules:
        /// 1. <see cref="RunName"/> and <see cref="StepName"/> are mandatory (their identifiers can be zero).
        /// 2. If <see cref="Urls"/> is not populated then <see cref="StepUrl"/> and <see cref="LogUrl"/> must be populated.
        /// 3. If <see cref="Urls"/> is not populated and it has <see cref="Dependencies"/> is populated the then 
        ///     <see cref="DependencyUrl"/> must be populated.
        /// 4. If a <see cref="TargetType"/> or <see cref="TargetAssembly"/> is not provided then <see cref="SubSteps"/> 
        ///     must contain at least one Step i.e. it is possibly a step only executes its sub steps.
        /// 5. If a step's <see cref="SubSteps"/> or <see cref="TransitionSteps"/> do not have <see cref="Urls"/> 
        ///     they will be popuated with the step's <see cref="Urls"/>. This way a <see cref="Urls"/> farm can be 
        ///     set at the root step level and will be inherited by all steps in the workflow. 
        ///     <see cref="Urls"/> can be overriden by any step in the workflow, in which case those <see cref="Urls"/> 
        ///     will get inherited by any subsequent steps in the workflow (<see cref="SubSteps"/> or <see cref="TransitionSteps"/>).
        /// 6. If includeTransitions is true, then the <see cref="TransitionSteps"/> will also be validated.
        ///     this will result in evaulating every step until the end of the workflow.
        ///     
        /// An <see cref="Exception"/> is thrown if the step is not valid.
        /// </summary>
        /// <param name="includeTransitions">true if the steps <see cref="TransitionSteps"/> must also be validated, else false.</param>
        public void Validate(bool includeTransitions)
        {
            if (string.IsNullOrWhiteSpace(RunName))
            {
                throw new Exception($"RunId: { RunId } - Run Name is missing.");
            }

            if (string.IsNullOrWhiteSpace(StepName))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} - Step Name is missing.");
            }

            var hasUrls = (Urls?.Length ?? 0) > 0;

            if (!hasUrls && string.IsNullOrWhiteSpace(StepUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Step url is missing.");
            }

            if (!hasUrls && string.IsNullOrWhiteSpace(LogUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Log url is missing.");
            }
            else if (string.IsNullOrWhiteSpace(LogUrl))
            {
                LogUrl = Urls[0];
            }

            var hasDependencies = (Dependencies?.Length ?? 0) > 0;

            if (!hasUrls
                && hasDependencies
                && string.IsNullOrWhiteSpace(DependencyUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Dependency url is missing.");
            }
            else if (hasUrls
                && hasDependencies
                && string.IsNullOrWhiteSpace(DependencyUrl))
            {
                DependencyUrl = Urls[0];
            }

            if ((string.IsNullOrWhiteSpace(TargetAssembly)
                || string.IsNullOrWhiteSpace(TargetType))
                && (SubSteps?.Length ?? 0).Equals(0))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - If TargetType or TargetAssembly is missing then at least one sub step is required.");
            }

            if (string.IsNullOrWhiteSpace(LogFileLocation))
            {
                LogFileLocation = "DistributorLog.txt";
            }

            if (SubSteps != null)
            {
                foreach (var subStep in SubSteps)
                {
                    if (subStep.Urls == null)
                    {
                        subStep.Urls = Urls;
                    }

                    subStep.Validate(includeTransitions);
                }
            }

            if (TransitionSteps != null)
            {
                foreach (var transitionStep in TransitionSteps)
                {
                    if (transitionStep.Urls == null)
                    {
                        transitionStep.Urls = Urls;
                    }

                    if (includeTransitions)
                    {
                        transitionStep.Validate(includeTransitions);
                    }
                }
            }
        }
    }
}