using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace MVVMExtension
{
    /// <summary>
    /// MVVM属性通知变化
    /// </summary>
    [Injection(typeof(NotificationAttribute))]
    [Mixin(typeof(INotifyPropertyChanged))]
    [Aspect(Scope.PerInstance)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class NotificationAttribute : Attribute, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变化事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// setter方法后调用RaisePropertyChanged
        /// </summary>
        /// <param name="sender">属性值</param>
        /// <param name="propertyName">属性名称</param>
        [Advice(Kind.After, Targets = Target.Public | Target.Setter)]
        public void AfterSetter([Argument(Source.Instance)] object sender, [Argument(Source.Name)] string propertyName)
        {
            this?.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
