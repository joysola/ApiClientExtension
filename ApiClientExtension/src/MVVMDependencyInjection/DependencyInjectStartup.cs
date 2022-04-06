using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDependencyInjection
{
    public class DependencyInjectStartup
    {
        internal static DependencyInjectStartup Startup { get; } = new DependencyInjectStartup();
        internal DIInfo DI { get; } = new DIInfo();
        private IHost _host;
        /// <summary>
        /// 构建开始
        /// </summary>
        /// <param name="viewAssembly">view程序集</param>
        /// <param name="viewModelAssembly">viewmodel程序集</param>
        /// <param name="frameworkElementType">view类型</param>
        /// <param name="judgeViewFunc">判断view</param>
        /// <param name="judgeViewModelFunc">判断viewmodel</param>
        /// <returns></returns>
        public static DependencyInjectStartup CreateStartup(Assembly viewAssembly, Assembly viewModelAssembly,
            Type frameworkElementType, Func<Type, bool> judgeViewFunc, Func<Type, bool> judgeViewModelFunc)
        {
            Startup.SetTypesJudge(frameworkElementType, judgeViewFunc, judgeViewModelFunc)
                .RegisterTypes(viewAssembly, viewModelAssembly);
            return Startup;
        }
        /// <summary>
        /// 构建完成
        /// </summary>
        public DependencyInjectStartup Build()
        {
            GetDataContextExp();
            GetSetPropExp();
            Startup._host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    Startup.ConfigureServices(services);
                }).Build();
            return Startup;
        }

        /// <summary>
        /// 获取对应viewmodel实例
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        internal dynamic GetViewModel(Type viewModelType)
        {
            return Startup._host.Services.GetRequiredService(viewModelType);
        }

        /// <summary>
        /// 注册view和viewmodel
        /// </summary>
        /// <param name="viewAssembly"></param>
        /// <param name="viewModelAssembly"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DependencyInjectStartup RegisterTypes(Assembly viewAssembly, Assembly viewModelAssembly)
        {
            if (viewAssembly == null || viewModelAssembly == null)
            {
                throw new Exception("View或ViewModel程序集不能为空！");
            }
            DI.TypeDict.Clear();
            DI.TypePropDict.Clear();
            var viewAllTypes = viewAssembly.GetTypes();
            var viewModelTypes = viewModelAssembly.GetTypes();
            foreach (var viewType in viewAllTypes)
            {
                var diAttr = viewType.GetCustomAttribute<DependencyInjectAttribute>();
                if (diAttr != null)
                {
                    var allProps = viewType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    // 模式1 dependency 和 vm
                    foreach (var prop in allProps)
                    {
                        var attr = prop.GetCustomAttribute<VMAttribute>();
                        if (attr != null)
                        {
                            if (DI.TypePropDict.TryGetValue(viewType, out List<DIVMType> existedDIVMTypes))
                            {
                                existedDIVMTypes.Add(new DIVMType { Prop = prop, Params = attr.Params });
                            }
                            else
                            {
                                DI.TypePropDict.Add(viewType, new List<DIVMType> { new DIVMType { Prop = prop, Params = attr.Params } });
                            }
                        }
                    }
                    // 模式2：只有dependency
                    if (!DI.TypePropDict.ContainsKey(viewType))
                    {
                        Type viewModelType = null;
                        if (diAttr.ViewModelType != null) // 备注了类型，直接使用
                        {
                            viewModelType = diAttr.ViewModelType;
                        }
                        else // 未备注类型，则查找同名的ViewModel
                        {
                            viewModelType = viewModelTypes.FirstOrDefault(t => $"{viewType.Name}ViewModel" == t.Name);
                        }
                        if (viewModelType != null)
                        {
                            if (DI.ViewTypeFunc(viewType) && DI.ViewModelTypeFunc(viewModelType))
                            {
                                DI.TypeDict.Add(viewType, new DIType { VMType = viewModelType, Params = diAttr.Params });
                            }
                        }
                    }
                }
            }
            // 注册Viewmodel，用于在viewmodel中使用其他viewmodel
            foreach (var vmType in viewModelTypes)
            {
                var diAttr = vmType.GetCustomAttribute<DependencyInjectAttribute>();
                if (diAttr != null)
                {
                    var allProps = vmType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var prop in allProps)
                    {
                        var attr = prop.GetCustomAttribute<VMAttribute>();
                        if (attr != null)
                        {
                            if (DI.VMTypePropDict.TryGetValue(vmType, out List<DIVMType> existedDIVMTypes))
                            {
                                existedDIVMTypes.Add(new DIVMType { Prop = prop, Params = attr.Params });
                            }
                            else
                            {
                                DI.VMTypePropDict.Add(vmType, new List<DIVMType> { new DIVMType { Prop = prop, Params = attr.Params } });
                            }
                        }
                    }
                }
            }
            return Startup;
        }
        /// <summary>
        /// 设置判断类型
        /// </summary>
        /// <param name="frameworkElementType"></param>
        /// <param name="judgeViewFunc"></param>
        /// <param name="judgeViewModelFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DependencyInjectStartup SetTypesJudge(Type frameworkElementType, Func<Type, bool> judgeViewFunc, Func<Type, bool> judgeViewModelFunc)
        {
            if (frameworkElementType == null && frameworkElementType.GetProperty(DI.DataContext) == null)
            {
                throw new Exception("View的父类型不可以为空且必须包含DataContext属性！");
            }
            if (judgeViewFunc == null || judgeViewModelFunc == null)
            {
                throw new Exception("判断view和viewmodel类型的委托不可为空！");
            }
            DI.FrameworkElementType = frameworkElementType;
            DI.ViewTypeFunc = judgeViewFunc;
            DI.ViewModelTypeFunc = judgeViewModelFunc;
            return Startup;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            await _host.StartAsync();
        }
        /// <summary>
        /// 结束
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            foreach (var keyValue in DI.TypeDict)
            {
                //services.AddTransient(keyValue.Key); // 注册view

                if (keyValue.Value.Params != null) // 有参构造器
                {
                    services.AddTransient(keyValue.Value.VMType, x => ActivatorUtilities.CreateInstance(x, keyValue.Value.VMType, keyValue.Value.Params));
                }
                else // 无参构造器
                {
                    services.AddTransient(keyValue.Value.VMType);
                }
            }
            foreach (var keyValue in DI.TypePropDict)
            {
                foreach (var divmType in keyValue.Value)
                {
                    if (divmType.Params != null) // 有参构造器
                    {
                        services.AddTransient(divmType.Prop.PropertyType, x => ActivatorUtilities.CreateInstance(x, divmType.Prop.PropertyType, divmType.Params));
                    }
                    else // 无参构造器
                    {
                        services.AddTransient(divmType.Prop.PropertyType);
                    }
                }
            }
            foreach (var keyValue in DI.VMTypePropDict)
            {
                foreach (var divmType in keyValue.Value)
                {
                    if (divmType.Params != null) // 有参构造器
                    {
                        services.AddTransient(divmType.Prop.PropertyType, x => ActivatorUtilities.CreateInstance(x, divmType.Prop.PropertyType, divmType.Params));
                    }
                    else // 无参构造器
                    {
                        services.AddTransient(divmType.Prop.PropertyType);
                    }
                }
            }
        }
        /// <summary>
        /// 获取表达式树：view.GetType().GetProperty(DI.DataContext).SetValue(view, viewModel);
        /// </summary>
        private void GetDataContextExp()
        {
            var param_view = Expression.Parameter(typeof(object), "view"); // 入参
            var param_viewModel = Expression.Parameter(typeof(object), "viewModel"); // 入参
            var convert_view = Expression.Convert(param_view, DI.FrameworkElementType); // 将object转为FrameworkElementType
            var prop_exp = Expression.Property(convert_view, DI.DataContext);
            var exp = Expression.Assign(prop_exp, param_viewModel);
            var action = Expression.Lambda<Action<object, object>>(exp, param_view, param_viewModel).Compile();
            DI.SetDataContext = action;
        }
        /// <summary>
        /// 获取PropertyInfo.SetValue方法的表达式树
        /// </summary>
        private void GetSetPropExp()
        {
            var param_view = Expression.Parameter(typeof(object), "view"); // 入参
            var param_prop = Expression.Parameter(typeof(PropertyInfo), "prop"); // 入参
            var param_viewModel = Expression.Parameter(typeof(object), "viewModel"); // 入参
            var setValue_exp = Expression.Call(param_prop, nameof(PropertyInfo.SetValue), null, param_view, param_viewModel);
            var action = Expression.Lambda<Action<object, PropertyInfo, object>>(setValue_exp, param_view, param_prop, param_viewModel).Compile();
            DI.SetProperty = action;
        }
    }
}
