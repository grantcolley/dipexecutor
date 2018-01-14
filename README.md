# DipExecutor 
A distributed processing platform.

##### Technologies
*	###### Net Core 2.0 and .Net Standard 2.0
#####

#### Table of Contents
* [Components](#components)
* [How It Works](#how-it-works)

## Components
* *DipRunner.dll* – defines a Step and exposes the IRunner interface
* *DipExecutor.dll* – processes steps and provides notifications of progress to monitors
* *ExecutorHost.exe* – hosts the executor for processing steps
* *ExecutorMonitor.dll* – client application that starts and monitors the progress of a run

## How it Works
Create a class that implements the [IRunner](https://github.com/grantcolley/dipexecutor/blob/master/src/DipRunner/IRunner.cs) interface. The RunAsync method is the entry point for executing your code.

```C#
    public class CustomCode : IRunner
    {
        public async Task<Step> RunAsync(Step step)
        {
          // entry point to executing your code...
        }
    }
```

Define one or more steps to process. Each step can execute a specified target type. A step may contain sub steps. These are executed in parallel after the step's target type has been executed. Finally, a step can also contain transition steps. Transition steps are executed in parallel after the step's target type and sub steps have completed.

```C#         
            var counterparties = new Step();
            counterparties.RunId = 1;
            counterparties.RunName = "Ifrs9";
            counterparties.StepId = 1;
            counterparties.StepName = "Counterparties";
            counterparties.TargetAssembly = "Conterparty.dll";
            counterparties.TargetType = "Counterparty.GetCounterparties";
            counterparties.Payload = "args";
            counterparties.Urls = new[] { "http://localhost:5000" };
            counterparties.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\Conterparty.dll",
                @"C:\GitHub\Binaries\Monitor\ConterpartyData.dll"
            };

            var pd = new Step();
            pd.RunId = 1;
            pd.RunName = "Ifrs9";
            pd.StepId = 11;
            pd.StepName = "Probability Of Default";
            pd.TargetAssembly = "PD.dll";
            pd.TargetType = "PD.CalcPd";
            pd.Payload = "args";
            pd.Urls = new[] { "http://localhost:5000" };            
            pd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\PD.dll"
            };

            var lgd = new Step();
            lgd.RunId = 1;
            lgd.RunName = "Ifrs9";
            lgd.StepId = 12;
            lgd.StepName = "Loss Given Default";
            lgd.TargetAssembly = "LGD.dll";
            lgd.TargetType = "LGD.CalcLgd";
            lgd.Payload = "args";
            lgd.Urls = new[] { "http://localhost:5000" };            
            lgd.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\LGD.dll"
            };
            
            var modelling = new Step();
            modelling.RunId = 1;
            modelling.RunName = "Ifrs9";
            modelling.StepId = 2;
            modelling.StepName = "Modelling";
            modelling.TargetAssembly = "Modelling.dll";
            modelling.TargetType = "Modelling.StressTestScenarios";
            modelling.Payload = "args";
            modelling.Urls = new[] { "http://localhost:5000" };            
            modelling.Dependencies = new string[]
            {
                @"C:\GitHub\Binaries\Monitor\Modelling.dll"
            };
            
            counterparties.SubSteps = new Step[] { pd, lgd };
            ifrs9.TransitionSteps = new Step[] { modelling };
```

Run
A run is one or more steps that can be executed as a workflow. A run has a root step, which can contain sub steps and transition steps which defines the workflow.

Executor
The Executor “runs” a step. Running a step involves:
        ///     1. Initialise the step, including downloading dependencies.
        ///     2. Executes the step target type, including dynamically load and 
        ///         initialising an instance of the target type and calling its 
        ///         RunAsync method. 
        ///     3. Running the steps sub steps.
        ///     4. Running the steps transition steps on completion of above.

