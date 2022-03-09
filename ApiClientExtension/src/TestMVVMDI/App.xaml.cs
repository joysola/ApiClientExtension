using CommunityToolkit.Mvvm.ComponentModel;
using MVVMDependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestMVVMDI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DependencyInjectStartup.CreateStartup(Assembly.Load("TestMVVMDI"),
                Assembly.Load("TestMVVMDI"),
                typeof(FrameworkElement),
                t => typeof(Control).IsAssignableFrom(t),
                t => typeof(ObservableRecipient).IsAssignableFrom(t))
                .Build();

        }
    }
}
