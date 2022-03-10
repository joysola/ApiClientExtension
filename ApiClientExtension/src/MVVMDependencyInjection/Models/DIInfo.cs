using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MVVMDependencyInjection
{
    /// <summary>
    /// 注入相关信息
    /// </summary>
    internal class DIInfo
    {
        /// <summary>
        /// view的父类型（必须含datacontext属性）
        /// </summary>
        internal Type FrameworkElementType { get; set; }
        /// <summary>
        /// DataContext属性
        /// </summary>
        internal string DataContext { get; } = nameof(DataContext);
        /// <summary>
        /// 校验view类型委托
        /// </summary>
        internal Func<Type, bool> ViewTypeFunc { get; set; } = t => true;
        /// <summary>
        /// 校验viewModel类型委托
        /// </summary>
        internal Func<Type, bool> ViewModelTypeFunc { get; set; } = t => true;

        /// <summary>
        /// view和viewmodel类型字典
        /// </summary>
        internal Dictionary<Type, DIType> TypeDict { get; } = new Dictionary<Type, DIType>();
        /// <summary>
        /// view 和 对应 属性的viewmodel
        /// </summary>
        internal Dictionary<Type, List<DIVMType>> TypePropDict { get; } = new Dictionary<Type, List<DIVMType>>();
        /// <summary>
        /// view设置datacontext为viewmodel对象的表达式树(参数1 view，参数2 viewmodel)
        /// </summary>
        internal Action<object, object> SetDataContext { get; set; }
        /// <summary>
        /// 为对应viewmodel赋值（参数1 view，参数2 属性，参数3 viewmodel）
        /// </summary>
        internal Action<object, PropertyInfo, object> SetProperty { get; set; }
    }
}
