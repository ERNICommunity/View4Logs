using View4Logs.Core.MVVMComponent;

namespace View4Logs.Components
{
    public class App : Component<AppView, AppViewModel>
    {
        protected override AppViewModel ViewModelFactory()
        {
            return new AppViewModel();
        }
    }
}
