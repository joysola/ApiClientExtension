using HttpServiceExtension.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension.Expressions
{
    class HttpBaseExps
    {
        public static HttpBaseExps Singleton { get; } = new HttpBaseExps();
        private HttpBaseExps() { }
        /// <summary>
        /// 获取参数的PostContentAttribute 特性的方法
        /// </summary>
        internal Func<ParameterInfo, PostContentAttribute> GetPostContentAttribute { get; } = new Lazy<Func<ParameterInfo, PostContentAttribute>>(()
            => BuildGetParamAttribute<PostContentAttribute>()).Value;
        /// <summary>
        /// 获取参数的ParamNameAttribute 特性的方法
        /// </summary>
        internal Func<ParameterInfo, ParamNameAttribute> GetParamNameAttribute { get; } = new Lazy<Func<ParameterInfo, ParamNameAttribute>>(()
            => BuildGetParamAttribute<ParamNameAttribute>()).Value;
        /// <summary>
        /// 获取调用tostring方法的实际对象
        /// </summary>
        internal Func<object, Type> GetToStringType { get; } = new Lazy<Func<object, Type>>(() =>
        {
            // 实现如下效果：xx?.GetType().GetMethod("ToString", new Type[] { }).DeclaringType;

            var param_obj = Expression.Parameter(typeof(object), "obj"); // 入参
            var const_Null = Expression.Constant(null, typeof(object)); // null常量

            var euqalNull_exp = Expression.Equal(param_obj, const_Null); // 判断param_obj是否是null

            // param_obj不为null时,调用toString方法
            var method_getTypeExp = Expression.Call(param_obj, "GetType", null);
            var const_toStr = Expression.Constant("ToString");
            var cons_TypeArr = Expression.Constant(new Type[] { });
            var method_getMethodExp = Expression.Call(method_getTypeExp, "GetMethod", null, const_toStr, cons_TypeArr);
            var propExp = Expression.Property(method_getMethodExp, "DeclaringType");

            // 如果param_obj == null，则 返回 null 否则 调用 ToString方法
            var exp = Expression.Condition(euqalNull_exp, const_Null, propExp);

            var func = Expression.Lambda<Func<object, Type>>(exp, param_obj).Compile();
            return func;
        }).Value;

        /// <summary>
        /// 构造方法参数ParameterInfo的GetAttribute方法的表达式树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Func<ParameterInfo, T> BuildGetParamAttribute<T>() where T : Attribute
        {
            var param_ParameterInfo = Expression.Parameter(typeof(ParameterInfo), "ParamInfo");
            // 泛型方法
            var methodInfo2 = typeof(CustomAttributeExtensions).GetMethod("GetCustomAttribute", new Type[] { typeof(ParameterInfo) }).MakeGenericMethod(typeof(T));
            var methodCallExp2 = Expression.Call(null, methodInfo2, param_ParameterInfo); // 调用静态方法，第一个参数是null
            var func2 = Expression.Lambda<Func<ParameterInfo, T>>(methodCallExp2, param_ParameterInfo).Compile();
            return func2;
        }

    }
}
