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
        /// When a step has completed processing i.e. its <see cref="TargetType"/> and / or 
        /// <see cref="SubSteps"/> have been executed, then the transition steps are run.
        /// 
        /// </summary>
        public Step[] TransitionSteps { get; set; }

        public string DependencyUrl { get; set; }
        public string LogUrl { get; set; }
        public string[] Urls { get; set; }
    }
}