using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
namespace BidlinkU8Interface.Helper
{
    public class ObjValSetHelper
    {
        /// <summary>
        /// 给现有对象属性赋值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="nameValue">{ 属性名, 属性值 }</param>
        public static void SetPropertyValue(object obj, Dictionary<string, object> nameValue) 
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties()) 
            {
                object outObj; 
                if (nameValue.TryGetValue(pi.Name, out outObj))
                {
                    Type outType = outObj.GetType();
                    if (outType == pi.PropertyType) 
                    {
                        pi.SetValue(obj, outObj, null);
                    }
                }
            }
        }
        public static Object GetPropertyValue(object obj, String nameValue)
        {
            object outObj=null; 
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.Name == nameValue)
                {
                    outObj = obj.GetType().GetProperty(nameValue).GetValue(obj, null);
                    return outObj;
                }
            }
            return outObj;
        }
    }
}