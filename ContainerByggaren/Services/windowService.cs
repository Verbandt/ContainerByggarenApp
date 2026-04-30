using ContainerByggaren.Views;
using ContainerByggaren;

namespace ContainerByggaren.Services
{
    public class WindowService
    {
        public void OpenDeparturesView()
        {
            var containerView = new DeparturesDetailsWindow();
            containerView.Show();
        }
    }
}
    