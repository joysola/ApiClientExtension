﻿using AspectInjector.Broker;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMExtension
{
    /// <summary>
    /// MVVM属性通知变化
    /// </summary>
    [Injection(typeof(NotificationAttribute))]
    [Aspect(Scope.PerInstance)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class NotificationAttribute : Attribute
    {
        /// <summary>
        /// setter方法后调用RaisePropertyChanged
        /// </summary>
        /// <param name="sender">属性值</param>
        /// <param name="propertyName">属性名称</param>
        [Advice(Kind.After, Targets = Target.Public | Target.Setter)]
        public void AfterSetter([Argument(Source.Instance)] object sender, [Argument(Source.Name)] string propertyName)
        {
            if (sender is ViewModelBase)
            {
                (sender as ViewModelBase).RaisePropertyChanged(propertyName);
            }
        }
    }
}
