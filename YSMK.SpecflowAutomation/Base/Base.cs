using TechTalk.SpecFlow;

namespace YSMK.SpecflowAutomation.Base
{
    public class Base
    {
        public readonly ScenarioContext _scenarioContext;

        public Base(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        public BasePage CurrentPage
        {
            get; set;
        }
        //protected TPage GetInstance<TPage>() where TPage : BasePage, new()
        //{
        //    return (TPage)Activator.CreateInstance(typeof(TPage));
        //}

        public TPage As<TPage>() where TPage : BasePage
        {
            return (TPage)this;
        }

    }
}
