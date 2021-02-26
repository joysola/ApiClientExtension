using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 用户授权的所有菜单信息
    /// </summary>
    public class MVAuthorizedMenus
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 子菜单集合
        /// </summary>
        [JsonProperty("children")]
        public List<MVAuthorizedSubMenu> Children { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    /// <summary>
    /// 子菜单
    /// </summary>
    public class MVAuthorizedSubMenu
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
