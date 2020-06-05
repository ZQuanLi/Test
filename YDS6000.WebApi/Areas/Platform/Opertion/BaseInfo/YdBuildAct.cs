using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    partial class BaseInfoHelper
    {
        /// <summary>
        /// 获取建筑信息树形结构
        /// </summary>
        /// <returns></returns>
        public APIRst GetBuildList()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetBuildList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取建筑信息树形结构错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置建筑信息
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public APIRst SetBuild(BuildVModel build)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetBuild(build);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置建筑信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除建筑信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public APIRst DelBuild(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelBuild(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除建筑信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}