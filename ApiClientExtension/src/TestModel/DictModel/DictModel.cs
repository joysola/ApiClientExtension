using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*（1） 性别：         男 1、女 2
 * (2)  导出状态:      未导出 0 、已导出 1
 * (3)  检查项目状态:  待处理 1、已处理 2
 */
namespace TestModel
{
    /// <summary>
    /// 字典项目
    /// </summary>
    public class DictItem
    {
        /// <summary>
        /// 项目键值
        /// </summary>
        public string dictKey { get; set; }
        /// <summary>
        /// 项目值
        /// </summary>
        public string dictValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
    }
    /// <summary>
    /// 通用字典
    /// </summary>
    public class DictModel
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string dictValue { get; set; }
        /// <summary>
        /// 字典项目
        /// </summary>
        public List<DictItem> children { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
    }
}
