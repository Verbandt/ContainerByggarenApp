using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContainerByggaren.ViewModels;
using ContainerByggaren.Services.UI;

namespace ContainerByggaren
{
    public partial class MainWindow : Window
    {
        private bool _isDarkMode = true;
        private const double AspectRatio = 16.0 / 9.0;

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
            EnsureTrainColumns(15);

            DataContext = new MainviewModel(new WindowService());
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            string newTheme = _isDarkMode ? "Themes/Light.xaml" : "Themes/Dark.xaml";
            _isDarkMode = !_isDarkMode;

            ThemeButton.Content = _isDarkMode ? "🌙  Byt Tema" : "☀️  Byt Tema";

            var dict = new ResourceDictionary { Source = new Uri(newTheme, UriKind.Relative) };


            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
            {
                hwndSource.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SIZING = 0x0214;

            if (msg == WM_SIZING)
            {
                ResizeWindowToAspectRatio(wParam, lParam);
                handled = true;
            }

            return IntPtr.Zero;
        }

        private void ResizeWindowToAspectRatio(IntPtr wParam, IntPtr lParam)
        {
            RECT rect = System.Runtime.InteropServices.Marshal.PtrToStructure<RECT>(lParam);

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            const int WMSZ_LEFT = 1;
            const int WMSZ_RIGHT = 2;
            const int WMSZ_TOP = 3;
            const int WMSZ_TOPLEFT = 4;
            const int WMSZ_TOPRIGHT = 5;
            const int WMSZ_BOTTOM = 6;
            const int WMSZ_BOTTOMLEFT = 7;
            const int WMSZ_BOTTOMRIGHT = 8;

            int edge = wParam.ToInt32();

            switch (edge)
            {
                case WMSZ_LEFT:
                case WMSZ_RIGHT:
                    height = (int)Math.Round(width / AspectRatio);
                    rect.Bottom = rect.Top + height;
                    break;

                case WMSZ_TOP:
                case WMSZ_BOTTOM:
                    width = (int)Math.Round(height * AspectRatio);
                    rect.Right = rect.Left + width;
                    break;

                case WMSZ_TOPLEFT:
                    width = (int)Math.Round(height * AspectRatio);
                    rect.Left = rect.Right - width;
                    break;

                case WMSZ_TOPRIGHT:
                    width = (int)Math.Round(height * AspectRatio);
                    rect.Right = rect.Left + width;
                    break;

                case WMSZ_BOTTOMLEFT:
                    width = (int)Math.Round(height * AspectRatio);
                    rect.Left = rect.Right - width;
                    break;

                case WMSZ_BOTTOMRIGHT:
                    width = (int)Math.Round(height * AspectRatio);
                    rect.Right = rect.Left + width;
                    break;
            }

            System.Runtime.InteropServices.Marshal.StructureToPtr(rect, lParam, true);
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private void CustomerChoiceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeparturesDataGrid == null)
                return;

            var comboBox = sender as ComboBox;
            var selectedCustomer = comboBox?.SelectedItem as ComboBoxItem;
            var selectedCustomerContent = selectedCustomer?.Content as string;

            if (selectedCustomerContent == "Torslanda")
            {
                EnsureTrainColumns(15);
            }
            else if (selectedCustomerContent == "Gent")
            {
                EnsureTrainColumns(12);
            }
        }

        private void EnsureTrainColumns(int trainCount)
        {
            // Remove all old train columns first
            for (int i = DeparturesDataGrid.Columns.Count - 1; i >= 0; i--)
            {
                string? header = DeparturesDataGrid.Columns[i].Header?.ToString();

                if (header != null && header.StartsWith("Tåg "))
                {
                    DeparturesDataGrid.Columns.RemoveAt(i);
                }
            }

            // Add fresh train columns
            for (int trainNumber = 1; trainNumber <= trainCount; trainNumber++)
            {
                DeparturesDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = $"Tåg {trainNumber}",
                    Binding = new Binding($"Departure{trainNumber}"),
                    Width = 60
                });
            }
        }
    }
}