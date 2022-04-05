using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMDependencyInjection;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestMVVMDI
{
    [DependencyInject]
    [NotifyAspect]
    public class MainWindowViewModel : ObservableRecipient
    {
        [VM(222)]
        public MainXXXViewModel XXXViewModel { get; set; }
        public string XXX2 { get; set; }
        public MainWindowViewModel(string xxx, int yyy)
        {
            XXX2 = xxx;
            XXX = yyy;
        }
        public int XXX { get; set; } = 2;

        public ICommand AddCommand => new Lazy<AsyncRelayCommand>(() => new AsyncRelayCommand(async () =>
        {
            await Task.Delay(500);
            XXX++;
        })).Value;
    }
}
