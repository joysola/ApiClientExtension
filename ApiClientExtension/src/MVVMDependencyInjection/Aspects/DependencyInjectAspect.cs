using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MVVMDependencyInjection
{
    [Aspect(Scope.Global)]
    public class DependencyInjectAspect
    {
        internal DIInfo DI => DependencyInjectStartup.Startup.DI;
        /// <summary>
        /// 调用前
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        [Advice(Kind.Before, Targets = Target.Constructor)]
        public void Before(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Type)] Type type,
            [Argument(Source.Instance)] object instance)
        {
            if (DI.FrameworkElementType != null && DI.FrameworkElementType.IsAssignableFrom(type))
            {
                if (DI.TypePropDict.TryGetValue(type, out List<PropertyInfo> props))
                {
                    props?.ForEach(p =>
                    {
                        var viewModel = DependencyInjectStartup.Startup.GetViewModel(p.PropertyType);
                        DI.SetProperty?.Invoke(instance, p, viewModel);
                        //p.SetValue(instance,viewModel);
                    });
                }
            }
        }

        /// <summary>
        /// 调用后
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnValue"></param>
        /// <param name="instance"></param>
        [Advice(Kind.After, Targets = Target.Constructor)]
        public void After(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Type)] Type type,
            [Argument(Source.ReturnValue)] object returnValue,
            [Argument(Source.Instance)] object instance)
        {
            //
            if (DI.FrameworkElementType != null && DI.FrameworkElementType.IsAssignableFrom(type))
            {
                if (DI.TypeDict.TryGetValue(type, out Type viewModelType))
                {
                    var viewModel = DependencyInjectStartup.Startup.GetViewModel(viewModelType);
                    DI.SetDataContext?.Invoke(instance, viewModel);
                }
            }
        }



    }
}
