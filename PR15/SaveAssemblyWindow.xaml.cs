using System.Windows;

namespace PR15
{
    public partial class SaveAssemblyWindow : Window
    {
        public string AssemblyName => TxtName.Text;
        public string AuthorName => TxtAuthor.Text;

        public SaveAssemblyWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Введите название сборки!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtAuthor.Text))
            {
                MessageBox.Show("Введите имя автора!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}