# DipExecutor 
A distributed processing platform.

##### Technologies
*	###### Net Core 2.0 and .Net Standard 2.0, SignalR, WPF
#####

![Executor Monitor](https://raw.github.com/grantcolley/executormonitor/tree/master/README-images/executormonitor.png "Executor Monitor")

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
    public class Counterparties : IRunner
    {
        public async Task<Step> RunAsync(Step step)
        {
          // entry point to executing your code...
        }
    }
```

Define one or more steps to process. These steps form a workflow known as a run. Each [Step](https://github.com/grantcolley/dipexecutor/blob/master/src/DipRunner/Step.cs) can execute a specified target type. A step may contain sub steps. These are executed in parallel after the step's target type has been executed. Finally, a step can also contain transition steps. Transition steps are executed in parallel after the step's target type and sub steps have completed.

```C#         
            var counterparties = new Step();
            counterparties.RunId = 1;
            counterparties.RunName = "Ifrs9";
            counterparties.StepId = 1;
            counterparties.StepName = "Counterparties";
            counterparties.TargetAssembly = "Conterparty.dll";
            counterparties.TargetType = "Counterparty.Counterparties";
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

The [client](https://github.com/grantcolley/executormonitor/tree/master/DevelopmentInProgress.ExecutorMonitor.Wpf) makes a call to the executor, passing the root step to process.

```C#  
            var jsonContent = JsonConvert.SerializeObject(counterparties);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(counterparties.StepUrl, 
                                            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            }
```

The [dipexecutor.dll](https://github.com/grantcolley/dipexecutor/tree/master/src/DipExecutor/Service) exposes a WebHost with endpoints. A hosting application such as a [console app](https://github.com/grantcolley/dipexecutor/blob/master/src/ExecutorHost/Program.cs), service etc. can create an instance of the WebHost e.g.

```C#  
            var webHost = WebHost.CreateDefaultBuilder()
                .UseUrls("http://+:5000")
                .UseExecutorStartup()
                .Build();
                
            var task = webHost.RunAsync();
            task.GetAwaiter().GetResult();
```

The [Executor](https://github.com/grantcolley/dipexecutor/blob/master/src/DipExecutor/Executor.cs) receives a step to process. Processing the step involves the following actions:
1. Initialise the step, including downloading dependencies.
2. Executes the step target type by dynamically loading and initialising an instance of the target type and calling its RunAsync method. 
3. Run the steps sub steps in parallel.
4. On completion of the above, run the steps transition steps in parallel.

Throughout the processing the executor will send [notifications](https://github.com/grantcolley/dipexecutor/tree/master/src/DipExecutor/Notification) of the steps progress.

