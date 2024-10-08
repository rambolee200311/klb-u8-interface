using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
//using System.Web.Http.Results;
using BidlinkU8Interface.Helper;

namespace BidlinkU8Interface.App_Start
{
    public class APIFilter : AuthorizeAttribute
    {
        private JObject TokenConfig;
        private string TimeStamp;//用于加时
        private string ErrorMessage = "";
        SQLiteHelper site = new SQLiteHelper();

        //构造函数，获取从配置类中的配置
        public APIFilter()
        {
            //string HOTConfigPath = HostingEnvironment.MapPath(@"/HOTApiConfig.Json");
            //using (StreamReader file = System.IO.File.OpenText(HOTConfigPath))
            //{
            //    using (JsonTextReader reader = new JsonTextReader(file))
            //    {
            //        JObject o = (JObject)JToken.ReadFrom(reader);
            //        TokenConfig = o;
            //        TimeStamp = HOTConfig.GetConfig().GetTokenTimeStamp();// TokenConfig["TokenConfig"]["TimeStamp"].ToString();
            //    }
            //}
            TimeStamp = HOTConfig.GetConfig().GetTokenTimeStamp();
            site.buildConnetionString(HOTConfig.GetConfig().GetHOTConnetionString(
               HOTConfig.GetConfig().GetSQLDataSource(),
               HOTConfig.GetConfig().GetSQLInitialCatalog(),
               HOTConfig.GetConfig().GetSQLUserID(),
               HOTConfig.GetConfig().GetSQLPassword()));
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var httpContext = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
            var authHeader = httpContext.Request.Params["token"];
            var from_account= httpContext.Request.Params["from_account"];
            var app_key = httpContext.Request.Params["app_key"];
            if (authHeader == null)
            {
                ErrorMessage = "此接口必须携带token访问！";
                return false;//没有头文件为空，返回失败
            }
            else //字段值不为空

            {
                try
                {
                    //JObject LoginInfo = TokenHelper.GetJwtDecode(authHeader);
                    //string app_secret = LoginInfo["app_secret"].ToString();
                    string app_secret = "";
                    //string datetime =LoginInfo["datetime"].ToString();
                    //string PassWord = LoginInfo["passWord"].ToString();
                    int APIUserID = isAPIUser(from_account.ToString(), app_key.ToString(), app_secret);
                    if (APIUserID != -1)
                    {
                        //验证有效期
                        string TokenCreateTime = getTokenCreateTime(APIUserID, authHeader);
                        if (String.IsNullOrEmpty(TokenCreateTime))
                        {
                            ErrorMessage = "token错误，请重新获取";
                            return false;
                        }
                        if (!compareToken(TokenCreateTime, APIUserID, authHeader))
                        {
                            ErrorMessage = "token错误，请重新获取";
                            return false;
                        }
                        DateTime Requestdt = DateTime.Parse(TokenCreateTime).AddMinutes(int.Parse(TimeStamp));
                        if (Requestdt < DateTime.Now)
                        {
                            ErrorMessage = "token已过期，请重新获取";
                            return false;
                        }
                        else
                        {

                            return true;
                        }
                    }
                    else
                    {
                        ErrorMessage = "token验证失败：您没有权限调用此接口，请联系管理员！";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }

        }


        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            JsonDocument outParm = new JsonDocument();
            outParm.add(new JsonElement("errcode", "20005"));
            outParm.add(new JsonElement("errMsg", ErrorMessage));
            response.Content = new StringContent
                    (
                        outParm.innerText,
                        Encoding.GetEncoding("UTF-8"),
                        "application/json"
                    );

        }


        //辅助方法

        //验证账号密码
        private int isAPIUser(string from_account, string app_key, string app_secret)
        {
            Log.AddInfo("APIFilter.isAPIUser()", "Begin");
            string sqlstr = "SELECT APIUserID FROM T_APIUser where from_account = @from_account and app_key = @app_key";// and app_secret = @app_secret";
            SqlParameter[] par = {
                new SqlParameter("@from_account", from_account),
                new SqlParameter("@app_key", app_key)//,
                //new SqlParameter("@app_secret", app_secret),
                //new SqlParameter("@datetime", datetime)
            };
            DataSet qry = site.SelectData(sqlstr, par);
            if (qry.Tables.Count == 1 && qry.Tables[0].Rows.Count == 0)
            {
                Log.AddError("APIFilter.isAPIUser()", "APIUser账号不存在");
                return -1;
            }
            else
            {
                Log.AddInfo("APIFilter.isAPIUser()", "End");
                return Convert.ToInt32(qry.Tables[0].Rows[0]["APIUserID"].ToString());
            }
        }

        private string getTokenCreateTime(int APIUserID, string authHeader)
        {
            String strResult = "";
            string sqlStr = "SELECT top 1 CreateTime FROM T_TokenTrace where APIUserID = @APIUserID and token=@authHeader ORDER BY TokenTraceID  DESC";
            SqlParameter[] par = {
                new SqlParameter("@APIUserID", APIUserID),
                new SqlParameter("@authHeader", authHeader)
            };
            DataSet qry = site.SelectData(sqlStr, par);
            if (qry.Tables[0].Rows.Count > 0)
            {
                strResult = qry.Tables[0].Rows[0]["CreateTime"].ToString();
            }
            return strResult;
        }
    
        //验证token
        private bool compareToken(string TokenCreateTime,int APIUserID,string token)
        {
            bool bResult=true;
            string sqlstr = "SELECT from_account,app_key,app_secret FROM T_APIUser where APIUserID= @APIUserID";// and app_secret = @app_secret";
            SqlParameter[] par = {
                new SqlParameter("@APIUserID", APIUserID)
            };
            DataSet qry = site.SelectData(sqlstr, par);
            JsonDocument InData = new JsonDocument();

            InData.add(new JsonElement("from_account", qry.Tables[0].Rows[0]["from_account"].ToString()));
            InData.add(new JsonElement("app_key", qry.Tables[0].Rows[0]["app_key"].ToString()));
            InData.add(new JsonElement("app_secret", qry.Tables[0].Rows[0]["app_secret"].ToString()));
            InData.add(new JsonElement("datetime", TokenCreateTime));
            JObject JInData = (JObject)JsonConvert.DeserializeObject(InData.innerText);
            string tokenNew = TokenHelper.SetJwtEncode(JInData);//生成token
            if (token != tokenNew)
            {
                return false;
            }
            return bResult;
        }
    }
}