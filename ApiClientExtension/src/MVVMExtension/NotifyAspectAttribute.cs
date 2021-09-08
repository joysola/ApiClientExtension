using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVMExtension
{
    /// <summary>
    /// WPF的INotifyPropertyChanged通用属性变化通知
    /// </summary>
    [Injection(typeof(NotifyAspectAttribute))]
    [Aspect(Scope.PerInstance)]
    [Mixin(typeof(INotifyPropertyChanged))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class NotifyAspectAttribute : Attribute, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变化事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// setter方法后触发PropertyChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="propertyName"></param>
        [Advice(Kind.After, Targets = Target.Public | Target.Setter)]
        public void AfterSetter([Argument(Source.Instance)] object sender, [Argument(Source.Name)] string propertyName)
        {
            this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
