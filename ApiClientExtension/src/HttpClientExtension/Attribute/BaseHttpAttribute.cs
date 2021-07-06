using HttpClientExtension.ApiClient;
using HttpClientExtension.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// Http相关Attribute的父类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public abstract class BaseHttpAttribute : System.Attribute
    {
        private static readonly Dictionary<Type, Action<object, dynamic>> setFieldValueDict = new Dictionary<Type, Action<object, dynamic>>();
        /// <summary>
        /// 获取参数的PostContentAttribute 特性的方法
        /// </summary>
        private Lazy<Func<ParameterInfo, PostContentAttribute>> getPostConAttri = new Lazy<Func<ParameterInfo, PostContentAttribute>>(() =>
        {
            return BuildGetAttribute<PostContentAttribute>();
        });
        /// <summary>
        /// 获取参数的ParamNameAttribute 特性的方法
        /// </summary>
        private Lazy<Func<ParameterInfo, ParamNameAttribute>> getParamNameAttri = new Lazy<Func<ParameterInfo, ParamNameAttribute>>(() =>
        {
            return BuildGetAttribute<ParamNameAttribute>();
        });
        /// <summary>
        /// 获取调用tostring方法的实际对象
        /// </summary>
        private Lazy<Func<object, Type>> getToStringType = new Lazy<Func<object, Type>>(() =>
        {
            // 实现如下效果：xx.GetType().GetMethod("ToString", new Type[] { }).DeclaringType;
            var param_obj = Expression.Parameter(typeof(object), "obj");
            var method_getTypeExp = Expression.Call(param_obj, "GetType", null);
            var cons_Str = Expression.Constant("ToString");
            var cons_TypeArr = Expression.Constant(new Type[] { });
            var method_getMethodExp = Expression.Call(method_getTypeExp, "GetMethod", null, cons_Str, cons_TypeArr);
            var propExp = Expression.Property(method_getMethodExp, "DeclaringType");
            var func = Expression.Lambda<Func<object, Type>>(propExp, param_obj).Compile();
            return func;
        });
        private static readonly object locker = new object();

        /// <summary>
        /// url处理后的结果
        /// </summary>
        protected class UrlResult
        {
            /// <summary>
            /// 地址
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// post的实体
            /// </summary>
            public object PostModel { get; set; }
        }
        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        protected UrlResult GetUrl(object[] arguments, MethodBase methodBase)
        {
            var urlAttribute = methodBase.GetCustomAttribute<UrlAttribute>();
            var baseUrl = urlAttribute.Url; // 请求地址
            if (string.IsNullOrEmpty(baseUrl)) // 为配置Api地址则停止
            {
                throw new HttpClientException("请配置Api地址！");
            }
            object postModel = null; // post实体
            // 构建完整url
            var parameters = methodBase.GetParameters();
            var dict = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < arguments.Length; i++)
            {
                //if (!parameters[i].IsDefined(typeof(PostContentAttribute)))
                //{
                //    dict.Add(parameters[i].Name, arguments[i]);
                //}
                //else
                //{
                //    postModel = arguments[i];
                //}
                var postConAttr = getPostConAttri.Value(parameters[i]); // 获取post参数特性
                var parNameAttr = getParamNameAttri.Value(parameters[i]); // 获取改名参数特性
                // 是否是post实体
                if (postConAttr != null)
                {
                    postModel = arguments[i]; // post参数不用拼接url
                }
                else
                {
                    // 判断Url参数是否需要改名字
                    if (parNameAttr != null)
                    {
                        dict.Add(new KeyValuePair<string, object>(parNameAttr.ParamName, arguments[i] ?? string.Empty));
                    }
                    else
                    {
                        dict.Add(new KeyValuePair<string, object>(parameters[i].Name, arguments[i] ?? string.Empty));
                    }
                }
            }
            var paramUrl = string.Empty; // url参数
            if (dict.Count > 0)
            {
                //var paramUrlArray = dict.Select(x => $"{x.Key}={x.Value}").ToArray();
                //paramUrl = $"?{string.Join("&", paramUrlArray)}";
                paramUrl = this.GetUrlParam(dict);
            }
            var url = $"{baseUrl}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }

        /// <summary>
        /// 从response里获取数据，设置数据
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        protected void SetResultData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!IsReturnHttpResponse(httpResponse, instance, rtype)) // 判断返回类型是否是HttpResponseMessage
                {
                    DeserializeJsonData(httpResponse, instance, rtype);
                }
            }
            else
            {
                throw new HttpClientException($"WebApi访问失败！错误代码：{(int)httpResponse.StatusCode}");
            }
        }

        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected HttpResponseMessage Post(string url, HttpContent content)
        {
            try
            {
                var result = HttpClientEx.Singleton.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult(); // post方法获取数据
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiPost方法出错！", ex);
                throw new HttpClientException($"WebApi访问失败！{ex.InnerException?.Message}", ex);
            }
        }
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected HttpResponseMessage Get(string url)
        {
            try
            {
                var result = HttpClientEx.Singleton.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiGet方法出错！", ex);
                throw new HttpClientException($"WebApi访问失败！{ex.InnerException?.Message}", ex);
            }
        }
        /// <summary>
        /// 生成给instance（父类）的baseResult赋值的action
        /// </summary>
        /// <param name="instance"></param>
        private Action<object, dynamic> BuildSetbaseResultAction(object instance/*, dynamic result*/)
        {
            var insType = instance.GetType();
            Action<object, dynamic> action = null;
            if (setFieldValueDict.TryGetValue(insType, out action))
            {
                return action;
            }
            lock (locker)
            {
                // 双检锁。。。
                if (setFieldValueDict.TryGetValue(insType, out action))
                {
                    return action;
                }
                var baseType = instance.GetType().BaseType; // 父类类型
                var param_ins = Expression.Parameter(typeof(object), "ins"); // 输入instance参数
                var param_val = Expression.Parameter(typeof(object), "val"); // 输入需要给字段赋值的数据
                var convertBaseExpre = Expression.Convert(param_ins, baseType); // 子类没有baseResult字段，因此需要转换成父类
                var fieldExp = Expression.Field(convertBaseExpre, baseType, "baseResult"); // 获取baseResult字段(此处可以直接访问私有字段...)
                var setfieldExp = Expression.Assign(fieldExp, param_val); // 赋值
                var setfieldAction = Expression.Lambda<Action<object, dynamic>>(setfieldExp, param_ins, param_val).Compile(); // 构建字段赋值
                setFieldValueDict.Add(insType, setfieldAction);
                return setfieldAction;
            }
            //setfieldAction(instance, result);
            // 查看是否成功赋值
            //var getfieldFunc = Expression.Lambda<Func<object, dynamic>>(fieldExp, param_ins).Compile();
            //var xx = getfieldFunc(instance);
        }
        /// <summary>
        /// instance（父类）的baseResult赋值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        private void SetbaseResult(object instance, dynamic result)
        {
            var action = BuildSetbaseResultAction(instance/*, result*/);
            action(instance, result);
        }
        /// <summary>
        /// instance（父类）的baseResult赋值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <param name="type">返回类型</param>
        private void SetbaseResult(object instance, dynamic result, Type type)
        {
            var action = BuildSetbaseResultAction(instance/*, result*/);
            if (result == null) // 若result（反序列化结果为null，则返回type类型默认实例）
            {
                action(instance, Activator.CreateInstance(type));
            }
            else
            {
                action(instance, result);
            }
        }
        /// <summary>
        /// 构造ParameterInfo的GetAttribute方法的表达式树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Func<ParameterInfo, T> BuildGetAttribute<T>()
        {
            var param_ParameterInfo = Expression.Parameter(typeof(ParameterInfo), "ParamInfo");
            // 非泛型方法
            //var cons_Attr = Expression.Constant(typeof(T)); // 常量
            //var methodInfo = typeof(CustomAttributeExtensions).GetMethod("GetCustomAttribute", new Type[] { typeof(ParameterInfo), typeof(Type) });
            //var methodCallExp = Expression.Call(null, methodInfo, param_ParameterInfo, cons_Attr);
            //var unaryExpression = Expression.Convert(methodCallExp, typeof(T));
            //var func = Expression.Lambda<Func<ParameterInfo, T>>(unaryExpression, param_ParameterInfo).Compile();

            // 泛型方法
            var methodInfo2 = typeof(CustomAttributeExtensions).GetMethod("GetCustomAttribute", new Type[] { typeof(ParameterInfo) }).MakeGenericMethod(typeof(T));
            var methodCallExp2 = Expression.Call(null, methodInfo2, param_ParameterInfo); // 调用静态方法，第一个参数是null
            var func2 = Expression.Lambda<Func<ParameterInfo, T>>(methodCallExp2, param_ParameterInfo).Compile();
            return func2;
        }
        /// <summary>
        /// 获取Url参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetUrlParam(List<KeyValuePair<string, object>> list)
        {
            var resList = new List<string>();
            foreach (var kp in list)
            {
                var toStringType = getToStringType.Value(kp.Value); // 如果value是null，则默认空值
                if (toStringType == typeof(object)) // 未重载toString方法，跳过
                {
                    continue;
                }
                else // 重载了tostring
                {
                    resList.Add($"{kp.Key}={kp.Value}");
                }
            }
            var paramUrlArray = resList.ToArray();
            var paramUrl = $"?{string.Join("&", paramUrlArray)}";
            return paramUrl;
        }

        /// <summary>
        /// 反序列化json数据（核心）
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        private void DeserializeJsonData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var json = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
                                                                                                                // 预判
            if (HttpClientEx.PreProcedure.PreAction != null && HttpClientEx.PreProcedure.PreProcesstype != null)
            {
                dynamic preResult = JsonConvert.DeserializeObject(json, HttpClientEx.PreProcedure.PreProcesstype);
                HttpClientEx.PreProcedure.PreAction(preResult); // 执行预判方法
            }
            //dynamic ins = instance; // 转成动态类型
            //var baseResult = instance.GetType().BaseType.GetField("baseResult", BindingFlags.NonPublic | BindingFlags.Instance); // 获取BaseResult属性（父类中）
            //if (baseResult == null)
            //{
            //    throw new HttpClientException("GetResult方法获取对应字段失败！");
            //}
            try
            {
                if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                {
                    dynamic result = JsonConvert.DeserializeObject(json, rtype.GenericTypeArguments[0]);
                    //var result = Convert.ChangeType(zzz.data, rtype.GenericTypeArguments[0]);
                    var taskResult = Task.FromResult(result); // 结果装入Task
                    SetbaseResult(instance, taskResult, rtype.GenericTypeArguments[0]);
                    //baseResult.SetValue(ins, taskResult, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                }
                else // 同步
                {
                    dynamic result = JsonConvert.DeserializeObject(json, rtype);
                    //baseResult.SetValue(ins, result, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                    SetbaseResult(instance, result, rtype);
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("HttpBase出错！", ex);
                throw;
            }
        }
        /// <summary>
        /// 判断返回类型是否是HttpResponseMessage，如果是，则处理它（返回它），不是，则进行json反序列化处理
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance">实例</param>
        /// <param name="rtype">返回类型</param>
        /// <returns></returns>
        private bool IsReturnHttpResponse(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var res = false;
            dynamic result = null;
            if (rtype == typeof(HttpResponseMessage))
            {
                res = true;
                result = httpResponse;
            }
            else if (rtype == typeof(Task<HttpResponseMessage>))
            {
                res = true;
                result = Task.FromResult(httpResponse); // 结果装入Task
            }
            if (result != null)
            {
                SetbaseResult(instance, result, rtype);
            }
            return res;
        }
    }
}
