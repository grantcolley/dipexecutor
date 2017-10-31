//-----------------------------------------------------------------------
// <copyright file="Step.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipRunner.Test")]
namespace DipRunner
{
    /// <summary>
    /// A step to be processed in a workflow. A step is processed by executing 
    /// a <see cref="TargetType"/> associated with the step and / or its <see cref="SubSteps"/>.  
    /// After the step's <see cref="TargetType"/> and / or its <see cref="SubSteps"/> 
    /// have finished executing, its <see cref="TransitionSteps"/> are run, thereby progressing
    /// along the workflow. When there are no more steps to process or transition to the workflow is complete.
    /// </summary>
    public class Step
    {
        private string stepUrl;
        private string notificationUrl;
        private string dependencyUrl;

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
        /// Gets or sets the location where the <see cref="Dependencies"/>,
        /// including <see cref="TargetAssembly"/>, are downloaded to.
        /// </summary>
        public string AssemblyPath { get; set; }

        /// <summary>
        /// Gets or sets a value to tell the step not to remove downloaded assemblies on completion.
        /// </summary>
        public bool SkipAssemblyCleanup { get; set; }

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
                    return GetUrlAction(Urls?[0], "run");
                }

                return GetUrlAction(stepUrl, "run");
            }
            set
            {
                stepUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the url providing the location of the dependencies referenced by the <see cref="TargetAssembly"/>.
        /// </summary>
        public string DependencyUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(dependencyUrl))
                {
                    return GetUrlAction(Urls?[0], "getdependency");
                }

                return GetUrlAction(dependencyUrl, "getdependency");
            }
            set
            {
                dependencyUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the url to be used for notifications and logging steps progress through the workflow.
        /// </summary>
        public string NotificationUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(notificationUrl))
                {
                    return GetUrlAction(Urls?[0], "notify");
                }

                return GetUrlAction(notificationUrl, "notify");
            }
            set
            {
                notificationUrl = value;
            }
        }

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
        /// 1. <see cref="RunName"/> and <see cref="StepName"/> are mandatory.
        /// 2. If <see cref="Urls"/> is not populated then <see cref="StepUrl"/> and <see cref="NotificationUrl"/> must be populated.
        /// 3. If <see cref="Urls"/> is not populated and it has <see cref="Dependencies"/> then <see cref="DependencyUrl"/> must be populated.
        /// 4. If <see cref="TargetType"/> or <see cref="TargetAssembly"/> is not provided then <see cref="SubSteps"/> 
        ///     must contain at least one Step i.e. it is possibly a step only executes its sub steps.
        /// 5. If <see cref="TargetType"/> and <see cref="TargetAssembly"/> is provided then <see cref="Dependencies"/> must contain 
        ///     at least one assembly i.e. the target assembly.
        /// 6. If walkTree is true, then <see cref="SubSteps"/> and <see cref="TransitionSteps"/> will also be validated.
        ///     This will result in evaulating every step until the end of the workflow. It will also make the parameter footprint larger
        ///     and not take advantage of url inheritance.
        ///     
        /// An <see cref="Exception"/> is thrown if the step is not valid.
        /// </summary>
        /// <param name="walkTree">true the <see cref="SubSteps"/> and <see cref="TransitionSteps"/> must also be validated, else false.</param>
        public void Validate(bool walkTree)
        {
            if (string.IsNullOrWhiteSpace(RunName))
            {
                throw new Exception($"RunId: { RunId } - Run name is missing.");
            }

            if (string.IsNullOrWhiteSpace(StepName))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} - Step name is missing.");
            }

            var hasUrls = (Urls?.Length ?? 0) > 0;

            if (!hasUrls && string.IsNullOrWhiteSpace(StepUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Step url is missing.");
            }

            if (!hasUrls && string.IsNullOrWhiteSpace(NotificationUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Notification url is missing.");
            }

            var hasDependencies = (Dependencies?.Length ?? 0) > 0;

            if (!hasUrls
                && hasDependencies
                && string.IsNullOrWhiteSpace(DependencyUrl))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - Dependency url is missing.");
            }

            if ((string.IsNullOrWhiteSpace(TargetAssembly)
                || string.IsNullOrWhiteSpace(TargetType))
                && (SubSteps?.Length ?? 0).Equals(0))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - If TargetType or TargetAssembly is missing then at least one sub step is required.");
            }

            if (!string.IsNullOrWhiteSpace(TargetAssembly) 
                && !string.IsNullOrWhiteSpace(TargetType)
                && (Dependencies == null
                || Dependencies.Length == 0))
            {
                throw new Exception($"RunId: { RunId } Run Name: {RunName} StepId {StepId} Step Name {StepName} - If TargetType and TargetAssembly is missing then at least one dependency required i.e. the target assembly");
            }

            if (walkTree)
            {
                if (SubSteps != null)
                {
                    foreach (var subStep in SubSteps)
                    {
                        if (subStep.Urls == null)
                        {
                            subStep.Urls = Urls;
                        }

                        subStep.Validate(walkTree);
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

                        transitionStep.Validate(walkTree);
                    }
                }
            }
        }

        internal string GetUrlAction(string url, string action)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            if (url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 1);
            }

            if (url.EndsWith($"/{action}"))
            {
                return url;
            }

            return url + $"/{action}";
        }
    }
}