using System.Windows.Input;
using ContainerByggaren.Commands;
using ContainerByggaren.Services;

namespace ContainerByggaren.ViewModels
{
    public class MainviewModel
    {
        private readonly WindowService _windowService;

        public ICommand DepartureDetailsCommand { get; }

        public MainviewModel(WindowService windowService)
        {
            _windowService = windowService;
            DepartureDetailsCommand = new RelayCommand(OpenDeparturesView);
        }
        
        private void OpenDeparturesView(object? parameter)
        {
            _windowService.OpenDeparturesView();
        }
    }
}
