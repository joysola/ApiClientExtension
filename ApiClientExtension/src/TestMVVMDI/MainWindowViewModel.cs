using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestMVVMDI
{
    [NotifyAspect]
    public class MainWindowViewModel : ObservableRecipient
    {
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
