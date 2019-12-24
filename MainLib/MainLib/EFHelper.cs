﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MainLib
{
    /// <summary>
    /// Entity Framework method encapuslation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EFHelpler<T> where T : class
    {
        //BaseContext dbContext = new BaseContext();

        /// <summary>
        /// main db context
        /// </summary>
        public DbContext dbContext = null;
        /// <summary>
        /// 
        /// </summary>
        public EFHelpler(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /*
         EFHelpler<User> helper = new EFHelpler<User>();
         BaseContext dbContext = new BaseContext();
         // adding
         List<User> listUser = new List<User>();
         for (int i = 0; i < 2; i++)
         {
             User user = new User();
             user.PersonID = i;
             user.UserName = "FlyElehant" + i;
             listUser.Add(user);
         }
         helper.add(listUser.ToArray());
         Console.WriteLine("adding successfully");
             */

        /// <summary>
        /// 实体新增
        /// </summary>
        /// <param name="model"></param>
        public void add(params T[] paramList)
        {
            foreach (var model in paramList)
            {
                dbContext.Entry<T>(model).State = EntityState.Added;
            }
            dbContext.SaveChanges();
        }

        /*
         var query = helper.getSearchList(item => item.UserName.Contains("keso"));
       var queryMulti = helper.getSearchListByPage<int>(item => item.UserName.Contains("FlyElehant"), order => order.PersonID, 2, 1);
       query = queryMulti;
       foreach (User user in query)
       {
           Console.WriteLine(user.UserName);
       }
             */
        /// <summary>
        /// 实体查询
        /// </summary>
        public IEnumerable<T> getSearchList(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return dbContext.Set<T>().Where(where);
        }
        /// <summary>
        /// 实体分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IEnumerable<T> getSearchListByPage<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageSize, int pageIndex)
        {
            return dbContext.Set<T>().Where(where).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /*
         var query = helper.getSearchList(item => item.UserName.Contains("keso"));
       helper.delete(query.ToArray());
             */
        /// <summary>
        /// 实体删除
        /// </summary>
        /// <param name="model"></param>
        public void delete(params T[] paramList)
        {
            foreach (var model in paramList)
            {
                dbContext.Entry<T>(model).State = EntityState.Deleted;
            }
            dbContext.SaveChanges();
        }

        /*
         Dictionary<string,object> dic=new Dictionary<string,object>();
         dic.Add("PersonID",2);
         dic.Add("UserName","keso");
         helper.update(item => item.UserName.Contains("keso"), dic);
         Console.WriteLine("modify data successfully");
        */
        /// <summary>
        /// 按照条件修改数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="dic"></param>
        public void update(Expression<Func<T, bool>> where, Dictionary<string, object> dic)
        {
            IEnumerable<T> result = dbContext.Set<T>().Where(where).ToList();
            Type type = typeof(T);
            List<PropertyInfo> propertyList = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
            //遍历结果集
            foreach (T entity in result)
            {
                foreach (PropertyInfo propertyInfo in propertyList)
                {
                    string propertyName = propertyInfo.Name;
                    if (dic.ContainsKey(propertyName))
                    {
                        //设置值
                        propertyInfo.SetValue(entity, dic[propertyName], null);
                    }
                }
            }
            dbContext.SaveChanges();
        }

    }
}
