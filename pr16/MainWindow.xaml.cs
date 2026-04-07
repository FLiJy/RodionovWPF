using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using pr16;
using pr16.Views;
using pr16.Model;
using pr16.Services;

namespace pr16
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MainFrame.Navigate(new MainMenu());
        }

        public void Navigate(object page)
        {
            MainFrame.Navigate(page);
        }
    }
}