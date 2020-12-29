using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestFrameworkbyDLL
{
    public class DictApi : BaseApi<DictApi>
    {
        /// <summary>
        /// 根据键值获取字典(注意每次返回一个数组,但是实际上只会取第一项)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("api/deepsight-system/system/dict/getList")]
        [HttpGet]
        public ApiResponse<List<DictModel>> GetDict(string code) => GetResult();

        /// <summary>
        /// 获取共建医院信息
        /// </summary>
        /// <returns></returns>
        [Url("api/deepsight-fund/fund/hospital/getHospitalByLogin")]
        [HttpGet]
        public Task<ApiResponse<HotpitalModel>> GetHotpitalInfo() => GetResult();

        /// <summary>
        /// 获取该院所有送检医生
        /// </summary>
        /// <returns></returns>
        [Url("api/deepsight-system/user/listDoctorByLoginId")]
        [HttpGet]
        public ApiResponse<List<SubmitDoctorModel>> GetSubmitDoctors() => GetResult();

        /// <summary>
        /// 获取检验项目字典
        /// </summary>
        /// <returns></returns>
        [Url("api/deepsight-fund/fund/product/listDetails")]
        [HttpGet]
        public ApiResponse<List<ProductModel>> GetProductModels() => GetResult();

    }
}
