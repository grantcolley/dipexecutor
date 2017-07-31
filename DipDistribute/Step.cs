namespace DipDistribute
{
    public class Step
    {
        public int RunId { get; set; }
        public string RunName { get; set; }
        public int StepId{ get; set; }
        public string StepName { get; set; }
        public string Payload { get; set; }
        public string TargetType { get; set; }
        public string TargetAssembly { get; set; }
        public string[] Dependencies { get; set; }
        public Step[] SubSteps { get; set; }
        public Step[] Transitions { get; set; }
        public string Url { get; set; }
        public string LogUrl { get; set; }
        public string[] UrlFarm { get; set; }
        public StepStatus Status { get; set; }
    }
}