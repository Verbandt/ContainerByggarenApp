using ContainerByggaren.Views;

namespace ContainerByggaren.Services.UI
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
    