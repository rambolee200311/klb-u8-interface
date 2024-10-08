using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models;
using BidlinkU8Interface.App_Start;

namespace BidlinkU8Interface.Controllers
{
    public class TokenController : ApiController
    {
        //定义变量
        string LocalDBPath = HOTConfig.GetConfig().GetHOTConnetionString(
               HOTConfig.GetConfig().GetSQLDataSource(),
               HOTConfig.GetConfig().GetSQLInitialCatalog(),
               HOTConfig.GetConfig().GetSQLUserID(),
               HOTConfig.GetConfig().GetSQLPassword());


        SQLiteHelper site = new SQLiteHelper();

        public TokenController()
        {
            site.buildConnetionString(LocalDBPath);//读取本地数据库数据

        }
        /// <summary>
        /// 客户端调用此接口，用来获取token，创建token路由。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [Route("token")]
        [HttpGet]
        //public HttpResponseMessage CreateToken(string from_account, string app_key, string app_secret)
        public Result CreateToken(string from_account, string app_key, string app_secret)
        {
            Log.AddInfo("TokenController.CreateToken()", "Begin");
            string return_code;
            string return_msg;
            string datetime = DateTime.Now.ToString();
            Result result = new Result();
            //Utf8JsonReader reader = new Utf8JsonReader();
            try
            {
                JsonDocument InData = new JsonDocument();

                InData.add(new JsonElement("from_account", from_account));
                InData.add(new JsonElement("app_key", app_key));
                InData.add(new JsonElement("app_secret", app_secret));
                InData.add(new JsonElement("datetime", datetime));
                JObject JInData = (JObject)JsonConvert.DeserializeObject(InData.innerText);
                Log.AddInfo("TokenController.CreateToken()", "inData:" + InData.innerText);
                //Log.AddInfo("TokenController.CreateToken()", "判断用户合法性...");
                int APIUser = isAPIUser(from_account, app_key, app_secret); //查询API用户信息
                if (APIUser != -1)
                {
                    Log.AddInfo("TokenController.CreateToken()", " 准备生成token");
                    string token = TokenHelper.SetJwtEncode(JInData);//生成token
                    Log.AddInfo("TokenController.CreateToken()", " token生成完毕：" + token);
                    return_code = "0";
                    return_msg = "成功";
                    result.errcode = return_code;
                    result.errmsg = return_msg;
                    result.token = new Token();
                    result.token.appKey = app_key;
                    result.token.id = token;
                    result.token.expiresIn = Convert.ToInt32(HOTConfig.GetConfig().GetTokenTimeStamp());
                    //写入数据库日志
                    writeTokenLog(APIUser, token, datetime);
                    /*
                    JsonDocument outParam = new JsonDocument();//出参部分
                    outParam.add(new JsonElement("errcode", return_code));
                    outParam.add(new JsonElement("errmsg", return_msg));
                    JsonDocument outToken = new JsonDocument();//出参部分
                    outToken.add(new JsonElement("appKey", app_key));
                    outToken.add(new JsonElement("id", token));
                    outToken.add(new JsonElement("expiresIn", HOTConfig.GetConfig().GetTokenTimeStamp()));
                    outParam.add(new JsonElement("token", outToken.innerText.ToString()));
                    //outParam.add(new JsonElement("expires_in", HOTConfig.GetConfig().GetTokenTimeStamp()));
                   
                    //返回纯文本text/plain  ,返回json application/json  ,返回xml text/xml
                    HttpResponseMessage result = new HttpResponseMessage
                    {

                        Content = new StringContent
                        (
                            outParam.innerText,
                            Encoding.GetEncoding("UTF-8"),
                            "application/json"
                        )
                    };
                    */
                    Log.AddInfo("TokenController.CreateToken()", "outData:" + result.ToJson());
                    Log.AddInfo("TokenController.CreateToken()", " End");
                    return result;
                }
                else
                {
                    return_code = "30042";
                    return_msg = "用户名或密码错误";
                    result.errcode = return_code;
                    result.errmsg = return_msg;
                    //Log.AddError("TokenController.CreateToken()", "生成token失败：无效用户用与密码");
                    /*
                    JsonDocument outParam = new JsonDocument();//出参部分
                    outParam.add(new JsonElement("errcode", return_code));
                    outParam.add(new JsonElement("errmsg", return_msg));
                    //返回纯文本text/plain  ,返回json application/json  ,返回xml text/xml
                    HttpResponseMessage result = new HttpResponseMessage
                    {

                        Content = new StringContent
                        (
                            outParam.innerText,
                            Encoding.GetEncoding("UTF-8"),
                            "application/json"
                        )
                    };
                    */
                    return result;
                }


            }
            catch (Exception ex)
            {
                return_code = "20006";
                return_msg = "获取token异常:" + ex.Message;
                result.errcode = return_code;
                result.errmsg = return_msg;
                /*
                JsonDocument outParam = new JsonDocument();//出参部分
                outParam.add(new JsonElement("errcode", return_code));
                outParam.add(new JsonElement("errmsg", return_msg ));
                //返回纯文本text/plain  ,返回json application/json  ,返回xml text/xml
                HttpResponseMessage result = new HttpResponseMessage
                {

                    Content = new StringContent
                    (
                        outParam.innerText,
                        Encoding.GetEncoding("UTF-8"),
                        "application/json"
                    )
                };
                */
                return result;
            }

        }
        
        //验证账号密码
        private int isAPIUser(string from_account, string app_key, string app_secret)
        {
            //Log.AddInfo("TokenController.isAPIUser()", "Begin");
            string sqlstr = "SELECT APIUserID FROM T_APIUser where from_account = @from_account and app_key = @app_key and app_secret = @app_secret";
            SqlParameter[] par = {
                new SqlParameter("@from_account", from_account),
                new SqlParameter("@app_key", app_key),
                new SqlParameter("@app_secret", app_secret)
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

        //写入token获取日志
        private void writeTokenLog(int APIUser, string token, string datetime)
        {
            //Log.AddInfo("TokenController.writeTokenLog()", "Begin");
            string sqlstr = "INSERT into T_TokenTrace (APIUserID, CreateTime, Token, Available) VALUES (@APIUser,@nTime,@token,0)";
            SqlParameter[] par = {
                new SqlParameter("@APIUser", APIUser),
                new SqlParameter("@nTime",datetime),
                new SqlParameter("@token",token),
            };
            int i = site.ChangeData(sqlstr, par);
            //Log.AddInfo("TokenController.writeTokenLog()", "End");
        }
    }
}
