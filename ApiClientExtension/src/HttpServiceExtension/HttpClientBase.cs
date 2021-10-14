using HttpServiceExtension.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension
{

    public class HttpClientBase
    {
        private string _baseUrl;
        /// <summary>
        /// HttpClient
        /// </summary>
        internal HttpClient Client { get; }

        #region 处理核心
        /// <summary>
        /// 返回响应的预处理
        /// </summary>
        internal RespPreProcess RespPreProcedure { get; } = new RespPreProcess();
        /// <summary>
        /// json处理
        /// </summary>
        internal JsonProcess JsonProcedure { get; } = new JsonProcess();
        /// <summary>
        /// 路由处理
        /// </summary>
        internal RouterProcess RouterProcedure { get; } = new RouterProcess();
        /// <summary>
        /// 测速委托
        /// </summary>
        internal Action<string> BenchmarkAction { get; set; }
        #endregion 处理核心

        /// <summary>
        /// 主要的Url，用于复用
        /// </summary>
        public string BaseUrl
        {
            get => _baseUrl;
            set
            {
                _baseUrl = value;
                if (!string.IsNullOrEmpty(_baseUrl)) // 地址结尾是否带有斜杠，没有自动补齐斜杠
                {
                    _baseUrl = _baseUrl.EndsWith("/") ? _baseUrl : $"{_baseUrl}/";
                }
            }
        }
        /// <summary>
        /// 构造器，依赖注入
        /// </summary>
        /// <param name="client"></param>
        public HttpClientBase(HttpClient client)
        {
            Client = client;
        }
        /// <summary>
        /// 设定自定义请求头
        /// </summary>
        /// <param name="customHeader">请求头名称</param>
        /// <param name="customContent">请求头内容</param>
        public void SetCustomRequestHead(string customHeader, string customContent)
        {
            if (Client.DefaultRequestHeaders.Contains(customHeader)) // 注销后，需要更新token
            {
                Client.DefaultRequestHeaders.Remove(customHeader);
            }
            Client.DefaultRequestHeaders.Add(customHeader, customContent); // 第一次登录获取token
        }
        /// <summary>
        /// 设置超时时间，默认5s
        /// </summary>
        /// <param name="milliseconds"></param>
        public void SetTimeout(int milliseconds = 5000)
        {
            if (Client != null)
            {
                Client.Timeout = TimeSpan.FromMilliseconds(milliseconds);
            }
        }
        /// <summary>
        /// 返回的json结果预处理
        /// </summary>
        /// <param name="preAction">预处理委托</param>
        /// <param name="preType">返回结果反序列化类型</param>
        public void SetJsonPrePorcess(Action<string> preAction/*, Type preType*/)
        {
            if (preAction != null)
            {
                RespPreProcedure.RespPreAction = preAction;
                //RespPreProcedure.RespPreDescType = preType ?? typeof(object);
            }
        }
        /// <summary>
        /// 配置默认json处理方式
        /// </summary>
        /// <param name="deserialize">反序列化委托 （json、反序列化类型、返回对象）</param>
        /// <param name="serialize">序列化（对象，返回字符串）</param>
        public void SetDefaultJsonSerialize(Func<string, Type, object> deserialize, Func<object, string> serialize)
        {
            if (deserialize != null)
            {
                JsonProcedure.Deserialize = deserialize;
            }
            if (serialize != null)
            {
                JsonProcedure.Serialize = serialize;
            }
        }
        /// <summary>
        /// 增加自定义反序列化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deserialize"></param>
        public void AddCustomDeserialize(string key, Func<string, Type, object> deserialize)
        {
            if (!string.IsNullOrEmpty(key) && deserialize != null && !JsonProcedure.CustomDeserializeDict.ContainsKey(key))
            {
                JsonProcedure.CustomDeserializeDict.Add(key, deserialize);
            }
        }
        /// <summary>
        /// 增加自定义序列化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deserialize"></param>
        public void AddCustomSerialize(string key, Func<object, string> serialize)
        {
            if (!string.IsNullOrEmpty(key) && serialize != null && !JsonProcedure.CustomSerializeDict.ContainsKey(key))
            {
                JsonProcedure.CustomSerializeDict.Add(key, serialize);
            }
        }
        /// <summary>
        /// 设置路由处理
        /// 参数1： targetType：发起请求方法所在的类对象
        /// 参数2： name：发起请求方法名称
        /// 参数3： methodBase：发起请求方法信息
        /// 参数4： routeInfo：aop的http标签中url信息
        /// </summary>
        /// <param name="routerFunc"></param>
        public void SetRouterProcess(Func<Type, string, MethodBase, string, string> routerFunc)
        {
            if (routerFunc != null)
            {
                RouterProcedure.RouterFunc = routerFunc;
            }
        }
        /// <summary>
        /// 设置接口调用测速
        /// </summary>
        /// <param name="action"></param>
        public void SetBenchmark(Action<string> action)
        {
            if (action != null)
            {
                BenchmarkAction = action;
            }
        }
    }
}
