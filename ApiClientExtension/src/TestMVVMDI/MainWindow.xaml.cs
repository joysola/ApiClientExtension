using MVVMDependencyInjection;
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

namespace TestMVVMDI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [DependencyInject(typeof(MainXXXViewModel))]
    public partial class MainWindow : Window
    {
        //[VM]
        //public MainWindowViewModel xxx { get; set; }
        //[VM]
        //public MainXXXViewModel xxx2 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = xxx;
        }
        //public MainWindow(MainWindowViewModel ccc)
        //{

        //}

    }
}
