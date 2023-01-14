using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Base
{
    public class BaseStep : Base
    {
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public BaseStep(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) : base(scenarioContext)
        {
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public virtual void WriteToSpecFlowOutputHelper(string content)
        {
            //_specFlowOutputHelper.WriteLine("-----------------------------------");
            _specFlowOutputHelper.WriteLine(DateTime.Now + "     " + content);
            LogHelpers.Write(DateTime.Now + "     " + content);
            //_specFlowOutputHelper.WriteLine("-----------------------------------");
        }


    }
}
