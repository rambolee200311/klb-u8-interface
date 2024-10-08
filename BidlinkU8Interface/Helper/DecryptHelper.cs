using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
//解密类
namespace BidlinkU8Interface.Helper
{
    public class DecryptHelper
    {
        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="strContent">被加密文本</param>
        /// <param name="strKey">加密秘钥</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string T_DESDecrypt(string strContent, string strKey, Encoding encoding)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(strKey));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(strContent);
            return encoding.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        //RSA解密算法，第一个参数为私钥，第二个参数为要解密的数据
        public static string RSADecrypt(string PrivateKey, string DecryptString)
        {
            string str2;
            try
            {

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(PrivateKey);
                //FromBase64String( ) 将 Base64 数字编码的等效字符串轮换为8位无符号整数的数组。
                byte[] rgb = Convert.FromBase64String(DecryptString);
                //Decrypt（）方法是使用 RSA 算法对数据进行解密。
                byte[] buffer2 = provider.Decrypt(rgb, false);
                str2 = Encoding.Default.GetString(buffer2);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str2;
        }
    }
}