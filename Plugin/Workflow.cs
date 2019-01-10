using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Web.Plugin
{
    public class Workflow
    {
        /// <summary>
        /// 工作流触发
        /// </summary>
        /// <param name="workflowName">工作流名称</param>
        /// <param name="state">状态</param>
        /// <param name="action">动作</param>
        /// <param name="role">角色</param>
        /// <param name="workflowInstanceID">工作流实例</param>
        public static void ActionTrigger(string workflowName, string action, string role, int entityID,  string state="null")
        {
            if (state == "null" && action == "Insert")//工作流动作为新增
            {
                CreateWorkflowInstance(workflowName, action, role, entityID);
            }
            else
            {
                WorkflowAction(workflowName, action, role, entityID, state);
            }
        }
        /// <summary>
        /// 新增时创建工作流实例
        /// </summary>
        /// <param name="workflowName">工作流名称</param>
        /// <param name="action">动作</param>
        /// <param name="role">角色</param>
        private static void CreateWorkflowInstance(string workflowName, string action, string role, int entityID)
        {
            string userName = CacheHelper.GetCache("UserName").ToString();//获取当前用户

            string commandText = "CreateWorkflowInstance";//调用存储过程
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("workflowName",workflowName),
                new SqlParameter("action",action),
                new SqlParameter("role",role),
                new SqlParameter("userName",userName),
                new SqlParameter("entityID",entityID)
            };
           SqlHelper.ExecuteSaclar(CommandType.StoredProcedure, commandText, paras);
        }
        /// <summary>
        /// 工作流动动作触发
        /// </summary>
        /// <param name="workflowName">工作流名称</param>
        /// <param name="action">动作</param>
        /// <param name="role">角色</param>
        private static void WorkflowAction(string workflowName, string action, string role, int entityID,string state)
        {
            string userName = CacheHelper.GetCache("UserName").ToString();//获取当前用户

            string commandText = "WorkflowAction";//调用存储过程
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("workflowName",workflowName),
                new SqlParameter("action",action),
                new SqlParameter("role",role),
                new SqlParameter("userName",userName),
                new SqlParameter("entityID",entityID),
                new SqlParameter("state",state)
            };
            SqlHelper.ExecuteSaclar(CommandType.StoredProcedure, commandText, paras);
        }
    }
}