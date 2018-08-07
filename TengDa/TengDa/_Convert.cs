﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TengDa
{
    /// <summary>
    /// 自定义类型转化公用类
    /// </summary>
    public static class _Convert
    {
        public static int StrToInt(string str, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return defaultValue;
            }
        }

        public static DateTime StrToDateTime(string str, DateTime defaultValue)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return defaultValue;
            }
        }



        public static bool StrToBool(string str, bool defaultValue)
        {
            try
            {
                return bool.Parse(str.ToLower());
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return defaultValue;
            }
        }


        public static float StrToFloat(string str, float defaultValue)
        {
            try
            {
                return Convert.ToSingle(str);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                return defaultValue;
            }
        }


        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            return ToDataTable<T>(list, false);
        }

        /// <summary>    
        ///http://blog.csdn.net/emoonight/article/details/6617683
        /// 转化一个DataTable    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, bool headerIsDisplayNameAttribute)
        {

            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();

            //把所有的public属性加入到集合 并添加DataTable的列     
            Array.ForEach<PropertyInfo>(type.GetProperties(), p =>
            {
                pList.Add(p);
                var result = p.Name;
                if (headerIsDisplayNameAttribute)
                {
                    var attribute = p.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                    if (attribute != null)
                    {
                        result = ((DisplayNameAttribute)attribute).DisplayName;
                    }
                }
                dt.Columns.Add(result, p.PropertyType);
            });
            //GetCustomAttribute<DisplayNameAttribute>
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p =>
                {
                    var result = p.Name;
                    if (headerIsDisplayNameAttribute)
                    {
                        var attribute = p.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                        if (attribute != null)
                        {
                            result = ((DisplayNameAttribute)attribute).DisplayName;
                        }
                    }
                    row[result] = p.GetValue(item, null);
                });
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>    
        /// DataTable 转换为List 集合    
        /// </summary>    
        /// <typeparam name="TResult">类型</typeparam>    
        /// <param name="dt">DataTable</param>    
        /// <returns></returns>    
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表    
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口    

            Type t = typeof(T);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表     
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合    

            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例    
                T ob = new T();
                //找到对应的数据  并赋值    
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.    
                oblist.Add(ob);
            }
            return oblist;
        }
    }
}
