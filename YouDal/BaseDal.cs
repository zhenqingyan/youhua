using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouDal
{
    public abstract class BaseDal<T> where T:class
    {
        public bool Insert(T entity)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                var id = conn.Insert(entity);
                return true;
            }
        }

        public void test() {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Query("");
            }

        }

        public bool Update(T entity)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                return conn.Update(entity)>0;
            }
        }
        public T GetSingle(string rowGuid)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                return conn.GetList<T>("where RowGuid=@RowGuid", new { RowGuid=rowGuid }).FirstOrDefault();
            }
        }
        public IEnumerable<T> GetData(string sql = null, object whereObj = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                if (sql != null)
                {
                    return conn.GetList<T>(sql, whereObj);
                }
                else
                {
                    return conn.GetList<T>();
                }
            }
        }

        public IEnumerable<T> GetPageData(int pageIndex,int pageSize,out int totalCount,string sql,object whereObj=null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                totalCount = 0;
                totalCount = conn.RecordCount<T>(sql, whereObj);
                return conn.GetListPaged<T>(pageIndex, pageSize, sql, "Id desc", whereObj);
            }
        }
    }
}
