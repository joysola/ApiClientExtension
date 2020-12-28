using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /* name      id
     * TCT/HPV   1233733366982651905
     * HPV       1233733106256326658
     * TCT       1233732841448943617
     * 细胞穿刺   1206800484413743105
     * 微生物三项 1315823985142333442
     * B族链球菌  1319179917237800962
     * 叶酸       1249905657332883458
     * 小组织     1317016490469871617
     * 中组织     1317016574762799106
     * 大组织     1317016664758349825
     */

    /// <summary>
    /// 检查项目字典条目
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ProductModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? price { get; set; }
        /// <summary>
        /// 对应路由方法
        /// </summary>
        [JsonIgnore]
        public List<Func<string, string>> GetReportFunc { get; set; } = new List<Func<string, string>>();
        /// <summary>
        /// 拥有的类型
        /// </summary>
        public List<ProductType> productTypes { get; set; }
    }
    /// <summary>
    /// 项目的类型
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string productId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 类型值（中台所需）
        /// </summary>
        public string value { get; set; }
    }
}
