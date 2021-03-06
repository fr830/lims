﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model.ExpePlan;
using Comp;
using Dapper;

namespace DAL.ExpePlan
{
    /// <summary>
    /// 数据访问类:D_tb_ExpePlan
    /// </summary>
    public partial class D_tb_ExpePlan
    {

        /// <summary>
        /// 获取实验计划列表
        /// </summary>
        /// <param name="eExpePlan">查询实体</param>
        /// <returns>返回对应数据集合</returns>
        public List<E_tb_ExpePlan> GetExpePlanList(E_tb_ExpePlan eExpePlan)
        {
            List<E_tb_ExpePlan> list = new List<E_tb_ExpePlan>();

            //拼接查询条件
            StringBuilder strwhere = new StringBuilder();
            if (eExpePlan.SampleID > 0) //样品ID
            {
                strwhere.AddWhere($"SampleID={eExpePlan.SampleID}");
            }

            //主查询Sql
            StringBuilder search = new StringBuilder();
            search.AppendFormat(@"select A.*,B.ProjectName from tb_ExpePlan as A left join tb_Project as B on A.ProjectID=B.ProjectID {0} ", strwhere.ToString());

            //执行查询语句
            using (IDbConnection conn = new SqlConnection(PubConstant.GetConnectionString()))
            {
                list = conn.Query<E_tb_ExpePlan>(search.ToString(), eExpePlan)?.ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取符合要求的样品详情
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <returns>返回对应实体信息</returns>
        public E_tb_ExpePlan GetExpePlanInfo(E_tb_ExpePlan model)
        {
            StringBuilder strWhere = new StringBuilder();
            if (model.SampleID > 0) //样品ID
            {
                strWhere.AddWhere($"SampleID={model.SampleID}");
            }
            if (model.ProjectID > 0) //项目ID
            {
                strWhere.AddWhere($"ProjectID={model.ProjectID}");
            }
            if (!string.IsNullOrEmpty(model.TaskNo)) //样品编号
            {
                strWhere.AddWhere($"TaskNo='{model.TaskNo}'");
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append($@"select top 1 * from tb_ExpePlan {strWhere.ToString()}");
            using (IDbConnection conn = new SqlConnection(PubConstant.GetConnectionString()))
            {
                model = conn.Query<E_tb_ExpePlan>(strSql.ToString(), model)?.FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PlanID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ExpePlan");
            strSql.Append(" where PlanID=@PlanID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanID", SqlDbType.Int,4)
};
            parameters[0].Value = PlanID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(E_tb_ExpePlan model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ExpePlan(");
            strSql.Append("PlanTypeID,ProjectID,SampleID,InspectTime,InspectPlace,InspectMethod,HeadPersonnelID,TaskNo,Status,Remark,AreaID,EditPersonnelID,UpdateTime)");
            strSql.Append(" values (");
            strSql.Append("@PlanTypeID,@ProjectID,@SampleID,@InspectTime,@InspectPlace,@InspectMethod,@HeadPersonnelID,@TaskNo,@Status,@Remark,@AreaID,@EditPersonnelID,@UpdateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ProjectID", SqlDbType.Int,4),
                    new SqlParameter("@SampleID", SqlDbType.Int,4),
                    new SqlParameter("@InspectTime", SqlDbType.DateTime),
                    new SqlParameter("@InspectPlace", SqlDbType.NVarChar,100),
                    new SqlParameter("@InspectMethod", SqlDbType.NVarChar),
                    new SqlParameter("@HeadPersonnelID", SqlDbType.Int,4),
                    new SqlParameter("@TaskNo", SqlDbType.NVarChar,50),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.Text),
                    new SqlParameter("@AreaID", SqlDbType.Int,4),
                    new SqlParameter("@EditPersonnelID", SqlDbType.Int,4),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.PlanTypeID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.SampleID;
            parameters[3].Value = model.InspectTime;
            parameters[4].Value = model.InspectPlace;
            parameters[5].Value = model.InspectMethod;
            parameters[6].Value = model.HeadPersonnelID;
            parameters[7].Value = model.TaskNo;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.EditPersonnelID;
            parameters[12].Value = model.UpdateTime;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(E_tb_ExpePlan model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ExpePlan set ");
            strSql.Append("PlanTypeID=@PlanTypeID,");
            strSql.Append("ProjectID=@ProjectID,");
            strSql.Append("SampleID=@SampleID,");
            strSql.Append("InspectTime=@InspectTime,");
            strSql.Append("InspectPlace=@InspectPlace,");
            strSql.Append("InspectMethod=@InspectMethod,");
            strSql.Append("HeadPersonnelID=@HeadPersonnelID,");
            strSql.Append("TaskNo=@TaskNo,");
            strSql.Append("Status=@Status,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("AreaID=@AreaID,");
            strSql.Append("EditPersonnelID=@EditPersonnelID,");
            strSql.Append("UpdateTime=@UpdateTime");
            strSql.Append(" where PlanID=@PlanID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanTypeID", SqlDbType.Int,4),
                    new SqlParameter("@ProjectID", SqlDbType.Int,4),
                    new SqlParameter("@SampleID", SqlDbType.Int,4),
                    new SqlParameter("@InspectTime", SqlDbType.DateTime),
                    new SqlParameter("@InspectPlace", SqlDbType.NVarChar,100),
                    new SqlParameter("@InspectMethod", SqlDbType.NVarChar),
                    new SqlParameter("@HeadPersonnelID", SqlDbType.Int,4),
                    new SqlParameter("@TaskNo", SqlDbType.NVarChar,50),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.Text),
                    new SqlParameter("@AreaID", SqlDbType.Int,4),
                    new SqlParameter("@EditPersonnelID", SqlDbType.Int,4),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@PlanID", SqlDbType.Int,4)};
            parameters[0].Value = model.PlanTypeID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.SampleID;
            parameters[3].Value = model.InspectTime;
            parameters[4].Value = model.InspectPlace;
            parameters[5].Value = model.InspectMethod;
            parameters[6].Value = model.HeadPersonnelID;
            parameters[7].Value = model.TaskNo;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.EditPersonnelID;
            parameters[12].Value = model.UpdateTime;
            parameters[13].Value = model.PlanID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PlanID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ExpePlan ");
            strSql.Append(" where PlanID=@PlanID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanID", SqlDbType.Int,4)
};
            parameters[0].Value = PlanID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string PlanIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ExpePlan ");
            strSql.Append(" where PlanID in (" + PlanIDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public E_tb_ExpePlan GetModel(int PlanID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PlanID,PlanTypeID,ProjectID,SampleID,InspectTime,InspectPlace,InspectMethod,HeadPersonnelID,TaskNo,Status,Remark,AreaID,EditPersonnelID,UpdateTime from tb_ExpePlan ");
            strSql.Append(" where PlanID=@PlanID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PlanID", SqlDbType.Int,4)
};
            parameters[0].Value = PlanID;

            E_tb_ExpePlan model = new E_tb_ExpePlan();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PlanID"].ToString() != "")
                {
                    model.PlanID = int.Parse(ds.Tables[0].Rows[0]["PlanID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PlanTypeID"].ToString() != "")
                {
                    model.PlanTypeID = int.Parse(ds.Tables[0].Rows[0]["PlanTypeID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ProjectID"].ToString() != "")
                {
                    model.ProjectID = int.Parse(ds.Tables[0].Rows[0]["ProjectID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SampleID"].ToString() != "")
                {
                    model.SampleID = int.Parse(ds.Tables[0].Rows[0]["SampleID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InspectTime"].ToString() != "")
                {
                    model.InspectTime = DateTime.Parse(ds.Tables[0].Rows[0]["InspectTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InspectPlace"] != null)
                {
                    model.InspectPlace = ds.Tables[0].Rows[0]["InspectPlace"].ToString();
                }
                if (ds.Tables[0].Rows[0]["InspectMethod"] != null)
                {
                    model.InspectMethod = ds.Tables[0].Rows[0]["InspectMethod"].ToString();
                }
                if (ds.Tables[0].Rows[0]["HeadPersonnelID"].ToString() != "")
                {
                    model.HeadPersonnelID = int.Parse(ds.Tables[0].Rows[0]["HeadPersonnelID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TaskNo"] != null)
                {
                    model.TaskNo = ds.Tables[0].Rows[0]["TaskNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Remark"] != null)
                {
                    model.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AreaID"].ToString() != "")
                {
                    model.AreaID = int.Parse(ds.Tables[0].Rows[0]["AreaID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EditPersonnelID"].ToString() != "")
                {
                    model.EditPersonnelID = int.Parse(ds.Tables[0].Rows[0]["EditPersonnelID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PlanID,PlanTypeID,ProjectID,SampleID,InspectTime,InspectPlace,InspectMethod,HeadPersonnelID,TaskNo,Status,Remark,AreaID,EditPersonnelID,UpdateTime ");
            strSql.Append(" FROM tb_ExpePlan ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" PlanID,PlanTypeID,ProjectID,SampleID,InspectTime,InspectPlace,InspectMethod,HeadPersonnelID,TaskNo,Status,Remark,AreaID,EditPersonnelID,UpdateTime ");
            strSql.Append(" FROM tb_ExpePlan ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex, ref int total)
        {
            string order = "order by T.PlanID desc";
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                order = $"order by T.{orderby}";
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append($@"
                SELECT * FROM (  
                    SELECT ROW_NUMBER() OVER ({order})AS Row, T.*,A.ProjectName,B.PersonnelName as HeadPersonnelName,C.name as SampleName,D.RecordID,E.ReportID
                    from tb_ExpePlan T  
                    left join tb_Project as A on T.ProjectID=A.ProjectID
                    left join tb_InPersonnel as B on T.HeadPersonnelID=B.PersonnelID 
                    left join tb_Sample as C on T.SampleID=C.id
                    left join tb_OriginalRecord as D on T.TaskNo=D.TaskNo
                    left join tb_TestReport as E on E.SampleNum=C.SampleNum
                    {strWhere}
                ) TT
            ");
            total = DbHelperSQL.GetCount(strSql.ToString());
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 检查是已存在该任务单号
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <returns></returns>
        public int IsExistsTaskNo(string TaskNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ExpePlan where TaskNo='" + TaskNo + "'");
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获得实验计划超出检验时间内容
        /// 作者：章建国
        /// </summary>
        public DataSet GetUNFinishList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT
Count(*) AS unfinish,
dbo.tb_ExpePlan.HeadPersonnelID,
dbo.tb_InPersonnel.PersonnelName

FROM
dbo.tb_ExpePlan
INNER JOIN dbo.tb_InPersonnel ON dbo.tb_InPersonnel.PersonnelID = dbo.tb_ExpePlan.HeadPersonnelID
WHERE
dbo.tb_ExpePlan.Status = 0 AND
DATEDIFF(DAY,InspectTime,GETDATE()) > 5
GROUP BY
dbo.tb_ExpePlan.HeadPersonnelID,dbo.tb_InPersonnel.PersonnelName
   ORDER BY unfinish desc");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetAllUNFinishList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT
Sum(unfinish) AS unfinish,
HeadPersonnelID,
PersonnelName
FROM
View_UnComplateTest
GROUP BY
HeadPersonnelID,PersonnelName
   ORDER BY unfinish desc");
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 获取超时计划
        /// </summary>
        public List<E_tb_ExpePlan> GetTimeOutPlan(int areaid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($@"select top 3 A.planID,A.SampleID,A.updatetime,B.name as samplename,DATEDIFF(day,B.createdate,getdate()) as DiffDate
                        from tb_ExpePlan as A inner join tb_Sample as B on A.SampleID=B.id
                        where A.status=2 and A.areaID={areaid} and DATEDIFF(day,B.createdate,getdate())>=5 order by DATEDIFF(day,B.createdate,getdate()) desc");

            using (IDbConnection conn = new SqlConnection(PubConstant.GetConnectionString()))
            {
                List<E_tb_ExpePlan> result = conn.Query<E_tb_ExpePlan>(strSql.ToString()).ToList();
                return result;
            }
        }

        /// <summary>
        /// 获取未超时计划
        /// </summary>
        public List<E_tb_ExpePlan> GetNoTimeOutPlan(int areaid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($@"select top 7 A.planID,A.SampleID,A.updatetime,B.name as samplename,DATEDIFF(day,B.createdate,getdate()) as DiffDate
                        from tb_ExpePlan as A inner join tb_Sample as B on A.SampleID=B.id
                        where A.status=2 and A.areaID={areaid} and DATEDIFF(day,B.createdate,getdate())<5 order by DATEDIFF(day,B.createdate,getdate()) desc");

            using (IDbConnection conn = new SqlConnection(PubConstant.GetConnectionString()))
            {
                List<E_tb_ExpePlan> result = conn.Query<E_tb_ExpePlan>(strSql.ToString()).ToList();
                return result;
            }
        }

    }
}
