using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using ZlzsIo.Class;

namespace ZlzsIo
{
    class Program
    {
        string[] args;
        static string WebServicesurl, ProxyUrl, ProxyUser, ProxyPwd;
        static double DayNum = -2;
        public static void GetParams()
        {
            WebServicesurl = System.Configuration.ConfigurationSettings.AppSettings["WebServicesurl"];
            DayNum = double.Parse(System.Configuration.ConfigurationSettings.AppSettings["DayNum"]);
            //ProxyUrl = System.Configuration.ConfigurationSettings.AppSettings["ProxyUrl"];
            //ProxyUser = System.Configuration.ConfigurationSettings.AppSettings["ProxyUser"];
            //ProxyPwd = System.Configuration.ConfigurationSettings.AppSettings["ProxyPwd"];
        }
        static void Main(string[] args)
        {
            GetParams();
            //设置代理
            //HttpWebRequest.DefaultWebProxy = new WebProxy(ProxyUrl, true);
            //HttpWebRequest.DefaultWebProxy.Credentials =
            //new NetworkCredential { UserName = ProxyUser, Password = ProxyPwd };
            XmlDocument XmlD;
            XmlNode Xnode;
            XmlNodeList XnodeList;
            int Count = 0;
            string SqlStr = string.Empty;
            object result;
            //    string Date= DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string Date = DateTime.Now.AddDays(DayNum).ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                //=======================================================获取产品-资质信息表
                Console.WriteLine("获取产品资质表信息中...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpProdcognizance", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim()=="0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                            {
                            //删除重复   
                            SqlStr = (" delete from  J_TP_PRODCOGNIZANCE where ID='" + item.SelectSingleNode("id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_TP_PRODCOGNIZANCE",
                                                SqlDeal.Build("ID", item.SelectSingleNode("id").InnerText.Trim()) +
                                                SqlDeal.Build("REMS_PRODUCT_ID", item.SelectSingleNode("rems_product_id").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_NAME", item.SelectSingleNode("cognizance_name").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_NUMBER", item.SelectSingleNode("cognizance_number").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_TYPE", item.SelectSingleNode("cognizance_type").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_DATE", item.SelectSingleNode("cognizance_date").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_AWARD", item.SelectSingleNode("cognizance_award").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_DEPARTMENT", item.SelectSingleNode("cognizance_department").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_FILE_NAME", item.SelectSingleNode("cognizance_file_name").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_SOURCE", item.SelectSingleNode("cognizance_source").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_STATE", item.SelectSingleNode("cognizance_state").InnerText.Trim()) +
                                                SqlDeal.Build("CREATE_DATE", item.SelectSingleNode("create_date").InnerText.Trim()) +
                                                SqlDeal.Build("VERSION",SqlDeal.ConvertToDecimal(item.SelectSingleNode("version").InnerText.Trim())) +
                                                SqlDeal.Build("STATUS", item.SelectSingleNode("status").InnerText.Trim()) +
                                                // SqlDeal.Build("REMARK", item.SelectSingleNode("remark").InnerText.Trim()) +
                                                SqlDeal.Build("TEC_REQUIREMENTS", item.SelectSingleNode("tec_requirements").InnerText.Trim()) +
                                                SqlDeal.Build("FORMAT_TYPE", item.SelectSingleNode("format_type").InnerText.Trim()) +
                                                SqlDeal.Build("COGNIZANCE_UNIT", item.SelectSingleNode("cognizance_unit").InnerText.Trim()) +
                                                SqlDeal.Build("PRODUCT_CLASSIFICATION", item.SelectSingleNode("product_classification").InnerText.Trim()) +
                                                SqlDeal.Build("LOCATION", item.SelectSingleNode("location").InnerText.Trim()));

                            //插入信息
                            SqlDeal.NonQuery(SqlStr);
                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入"+ Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion


               

                //=======================================================获取企业信息表信息
                Console.WriteLine("获取企业信息表信息中...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpRemsOrgInfo", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim() == "0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                        {
                            //删除重复   
                            SqlStr = (" delete from  J_REMS_ORG_INFO where ID='" + item.SelectSingleNode("id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_REMS_ORG_INFO",
                                             SqlDeal.Build("ID", item.SelectSingleNode("id").InnerText.Trim()) +
                                            SqlDeal.Build("GMS_LOGIN_ID", item.SelectSingleNode("gms_login_id").InnerText.Trim()) +
                                            SqlDeal.Build("NAME", item.SelectSingleNode("name").InnerText.Trim()) +
                                            SqlDeal.Build("PROJORGCODE", item.SelectSingleNode("projorgcode").InnerText.Trim()) +
                                            SqlDeal.Build("SHOR_NAME", item.SelectSingleNode("shor_name").InnerText.Trim()) +
                                            SqlDeal.Build("REG_LOCUS", item.SelectSingleNode("reg_locus").InnerText.Trim()) +
                                            SqlDeal.Build("LICENE_DATE", item.SelectSingleNode("licene_date").InnerText.Trim()) +
                                            SqlDeal.Build("PROJORGCODE_DATE", item.SelectSingleNode("projorgcode_date").InnerText.Trim()) +
                                            SqlDeal.Build("ADDRESS", item.SelectSingleNode("address").InnerText.Trim()) +
                                            SqlDeal.Build("POST", item.SelectSingleNode("post").InnerText.Trim()) +
                                            SqlDeal.Build("FOUND_DATE", item.SelectSingleNode("found_date").InnerText.Trim()) +
                                            SqlDeal.Build("INTERNET", item.SelectSingleNode("internet").InnerText.Trim()) +
                                            SqlDeal.Build("CORPORATION", item.SelectSingleNode("corporation").InnerText.Trim()) +
                                            SqlDeal.Build("LINKMAN", item.SelectSingleNode("linkman").InnerText.Trim()) +
                                            SqlDeal.Build("LINKMAN_EMAIL", item.SelectSingleNode("linkman_email").InnerText.Trim()) +
                                            SqlDeal.Build("LINKMAN_TEL", item.SelectSingleNode("linkman_tel").InnerText.Trim()) +
                                            SqlDeal.Build("LINKMAN_FAX", item.SelectSingleNode("linkman_fax").InnerText.Trim()) +
                                            SqlDeal.Build("REG_TYPE", item.SelectSingleNode("reg_type").InnerText.Trim()) +
                                            SqlDeal.Build("CODE_SEQ", SqlDeal.ConvertToDecimal(item.SelectSingleNode("code_seq").InnerText.Trim())) +
                                            SqlDeal.Build("SWITCH_DATE", item.SelectSingleNode("switch_date").InnerText.Trim()) +
                                            SqlDeal.Build("DEGREE", item.SelectSingleNode("degree").InnerText.Trim()) +
                                            SqlDeal.Build("CAPITAL", SqlDeal.ConvertToDecimal(item.SelectSingleNode("capital").InnerText.Trim())) +
                                            SqlDeal.Build("CAPITAL_TYPE", item.SelectSingleNode("capital_type").InnerText.Trim()) +
                                            SqlDeal.Build("TAX_NUMBER", item.SelectSingleNode("tax_number").InnerText.Trim()) +
                                            SqlDeal.Build("TAX_RATE", item.SelectSingleNode("tax_rate").InnerText.Trim()) +
                                            SqlDeal.Build("BANK", item.SelectSingleNode("bank").InnerText.Trim()) +
                                            SqlDeal.Build("BANK_NUMBER", item.SelectSingleNode("bank_number").InnerText.Trim()) +
                                            SqlDeal.Build("QUALITY_CERT", item.SelectSingleNode("quality_cert").InnerText.Trim()) +
                                            SqlDeal.Build("INSPECT_ARTIFICE", item.SelectSingleNode("inspect_artifice").InnerText.Trim()) +
                                            SqlDeal.Build("PRODUCT_CLIENT", item.SelectSingleNode("product_client").InnerText.Trim()) +
                                            SqlDeal.Build("DESCRIPTION", item.SelectSingleNode("description").InnerText.Trim()) +
                                            SqlDeal.Build("RAILWAY_DEPARTMENT", item.SelectSingleNode("railway_department").InnerText.Trim()) +
                                            SqlDeal.Build("CREATED_DATE", item.SelectSingleNode("created_date").InnerText.Trim()) +
                                            SqlDeal.Build("CREATED_BY", item.SelectSingleNode("created_by").InnerText.Trim()) +
                                            SqlDeal.Build("LAST_UPDATE_BY", item.SelectSingleNode("last_update_by").InnerText.Trim()) +
                                            SqlDeal.Build("LAST_UPDATE_DATE", item.SelectSingleNode("last_update_date").InnerText.Trim()) +
                                            SqlDeal.Build("FARE_STATUS", item.SelectSingleNode("fare_status").InnerText.Trim()) +
                                            SqlDeal.Build("RELATION_STATE", item.SelectSingleNode("relation_state").InnerText.Trim()) +
                                            SqlDeal.Build("RAIL_CODE", item.SelectSingleNode("rail_code").InnerText.Trim()) +
                                            SqlDeal.Build("RELATION_MAN", item.SelectSingleNode("relation_man").InnerText.Trim()) +
                                            SqlDeal.Build("RELATION_DATE", item.SelectSingleNode("relation_date").InnerText.Trim()) +
                                            SqlDeal.Build("VERSION", SqlDeal.ConvertToDecimal(item.SelectSingleNode("version").InnerText.Trim())));


                            //插入信息
                            SqlDeal.NonQuery(SqlStr);
                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入" + Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion



               

                //=======================================================获取标准信息表信息
                Console.WriteLine("获取标准信息表信息中...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpStandardInfo", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim() == "0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                        {
                            //删除重复   
                            SqlStr = (" delete from  J_TP_STANDARDINFO where STANDARDINFO_ID='" + item.SelectSingleNode("standardinfo_id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_TP_STANDARDINFO",
                                            SqlDeal.Build("STANDARDINFO_ID", item.SelectSingleNode("standardinfo_id").InnerText.Trim()) +
                                            SqlDeal.Build("STANDARD_TYPE", item.SelectSingleNode("standard_type").InnerText.Trim()) +
                                            SqlDeal.Build("PROFESSIONAL_TYPE", item.SelectSingleNode("professional_type").InnerText.Trim()) +
                                            SqlDeal.Build("STANDARD_CODE", item.SelectSingleNode("standard_code").InnerText.Trim()) +
                                            SqlDeal.Build("STANDARD_NAME", item.SelectSingleNode("standard_name").InnerText.Trim()) +
                                            SqlDeal.Build("REPLACE_STANDARD_NUM", item.SelectSingleNode("replace_standard_num").InnerText.Trim()) +
                                            SqlDeal.Build("STANDARD_STATE", item.SelectSingleNode("standard_state").InnerText.Trim()) +
                                            SqlDeal.Build("PUBLISH_DATE", item.SelectSingleNode("publish_date").InnerText.Trim()) +
                                            SqlDeal.Build("IMPLETMENT_DATE", item.SelectSingleNode("impletment_date").InnerText.Trim()) +
                                            SqlDeal.Build("FILE_STATE", item.SelectSingleNode("file_state").InnerText.Trim()) +
                                            SqlDeal.Build("FILE_PATH", item.SelectSingleNode("file_path").InnerText.Trim()) +
                                            SqlDeal.Build("CREATE_DATE", item.SelectSingleNode("create_date").InnerText.Trim()) +
                                            SqlDeal.Build("CREATE_USER_ID", item.SelectSingleNode("create_user_id").InnerText.Trim()) +
                                            SqlDeal.Build("MODIFY_USER_ID", item.SelectSingleNode("modify_user_id").InnerText.Trim()) +
                                            SqlDeal.Build("MODIFY_DATE", item.SelectSingleNode("modify_date").InnerText.Trim()));

                            //插入信息
                            SqlDeal.NonQuery(SqlStr);
                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入" + Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion


               

                //=======================================================获取标准抽查状态信息表信息
                Console.WriteLine("获取标准抽查状态信息表信息...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpCheckStatusInfo", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim() == "0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                        {
                            //删除重复   
                            SqlStr = (" delete from  J_TP_CHECKSTATUSINFO where CHECKSTATUSINFO_ID='" + item.SelectSingleNode("checkstatusinfo_id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_TP_CHECKSTATUSINFO",
                                         SqlDeal.Build("CHECKSTATUSINFO_ID", item.SelectSingleNode("checkstatusinfo_id").InnerText.Trim()) +
                                            SqlDeal.Build("PRODUCT_CODE", item.SelectSingleNode("product_code").InnerText.Trim()) +
                                            SqlDeal.Build("PRODUCT_NAME", item.SelectSingleNode("product_name").InnerText.Trim()) +
                                            SqlDeal.Build("QUALITY_AUDIT_CHECK_STATE", item.SelectSingleNode("quality_audit_check_state").InnerText.Trim()) +
                                            SqlDeal.Build("QUALITY_AUDIT_CHECK_CODE", item.SelectSingleNode("quality_audit_check_code").InnerText.Trim()) +
                                            SqlDeal.Build("CREATE_DATE", item.SelectSingleNode("create_date").InnerText.Trim()) +
                                            SqlDeal.Build("CREATE_USER_ID", item.SelectSingleNode("create_user_id").InnerText.Trim()) +
                                            SqlDeal.Build("MODIFY_DATE", item.SelectSingleNode("modify_date").InnerText.Trim()) +
                                            SqlDeal.Build("MODIFY_USER_ID", item.SelectSingleNode("modify_user_id").InnerText.Trim()) +
                                            SqlDeal.Build("FLAG", SqlDeal.ConvertToDecimal(item.SelectSingleNode("flag").InnerText.Trim())));

                            //插入信息
                            SqlDeal.NonQuery(SqlStr);
                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入" + Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion


             
                //=======================================================获取产品标识代码表信息

                Console.WriteLine("获取产品标识代码表信息中...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpRemsProduct", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim() == "0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                        {
                            //删除重复   
                            SqlStr = (" delete from  J_REMS_PRODUCT where ID='" + item.SelectSingleNode("id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_REMS_PRODUCT",
                                          SqlDeal.Build("ID", item.SelectSingleNode("id").InnerText.Trim()) +
                                        SqlDeal.Build("REMS_ATTRIBUTE_TABLE_ID", item.SelectSingleNode("rems_attribute_table_id").InnerText.Trim()) +
                                        SqlDeal.Build("STD_NAME", item.SelectSingleNode("std_name").InnerText.Trim()) +
                                        SqlDeal.Build("POPULAR_NAME", item.SelectSingleNode("popular_name").InnerText.Trim()) +
                                        SqlDeal.Build("ORG_OWN_CODE", item.SelectSingleNode("org_own_code").InnerText.Trim()) +
                                        SqlDeal.Build("USED_SCOPE", item.SelectSingleNode("used_scope").InnerText.Trim()) +
                                        SqlDeal.Build("NOTE", item.SelectSingleNode("note").InnerText.Trim()) +
                                        SqlDeal.Build("M_CODE", item.SelectSingleNode("m_code").InnerText.Trim()) +
                                        SqlDeal.Build("STATUS", item.SelectSingleNode("status").InnerText.Trim()) +
                                        SqlDeal.Build("CREATED_DATE", item.SelectSingleNode("created_date").InnerText.Trim()) +
                                        SqlDeal.Build("CREATED_BY", item.SelectSingleNode("created_by").InnerText.Trim()) +
                                        SqlDeal.Build("LAST_UPDATE_BY", item.SelectSingleNode("last_update_by").InnerText.Trim()) +
                                        SqlDeal.Build("LAST_UPDATE_DATE", item.SelectSingleNode("last_update_date").InnerText.Trim()) +
                                        SqlDeal.Build("AUDITED_DATE", item.SelectSingleNode("audited_date").InnerText.Trim()) +
                                        SqlDeal.Build("ORG_NAME", item.SelectSingleNode("org_name").InnerText.Trim()) +
                                        SqlDeal.Build("PRODUCT_CODE", item.SelectSingleNode("product_code").InnerText.Trim()) +
                                        SqlDeal.Build("VERSION", SqlDeal.ConvertToDecimal(item.SelectSingleNode("version").InnerText.Trim())) +
                                        SqlDeal.Build("PUBLIC_", item.SelectSingleNode("public_").InnerText.Trim()) +
                                        SqlDeal.Build("PRO_SP_STATUS", item.SelectSingleNode("pro_sp_status").InnerText.Trim()) +
                                        SqlDeal.Build("PRO_SP_NOTE", item.SelectSingleNode("pro_sp_note").InnerText.Trim()) +
                                        SqlDeal.Build("PRO_AUTH_RANGE", item.SelectSingleNode("pro_auth_range").InnerText.Trim()) +
                                        SqlDeal.Build("CHECK_UNIT", item.SelectSingleNode("check_unit").InnerText.Trim()) +
                                        SqlDeal.Build("PICTURE_NUMBER", item.SelectSingleNode("picture_number").InnerText.Trim()) +
                                        SqlDeal.Build("PERFORM_STANDAR", item.SelectSingleNode("perform_standar").InnerText.Trim()) +
                                        SqlDeal.Build("OLD_MCODE", item.SelectSingleNode("old_mcode").InnerText.Trim()) +
                                        SqlDeal.Build("HIDDEN_LEADER", item.SelectSingleNode("hidden_leader").InnerText.Trim()) +
                                        SqlDeal.Build("HIDDEN_ORG", item.SelectSingleNode("hidden_org").InnerText.Trim()) +
                                        SqlDeal.Build("IS_EXPORT", item.SelectSingleNode("is_export").InnerText.Trim()) +
                                        SqlDeal.Build("CAN_EXPORT", item.SelectSingleNode("can_export").InnerText.Trim()) +
                                        SqlDeal.Build("PUBLISH_TIME", item.SelectSingleNode("publish_time").InnerText.Trim()) +
                                        SqlDeal.Build("FMZT", item.SelectSingleNode("fmzt").InnerText.Trim()) +
                                        SqlDeal.Build("KFSDW", item.SelectSingleNode("kfsdw").InnerText.Trim()));

                            //插入信息
                            SqlDeal.NonQuery(SqlStr);


                            //根据产品标识代码表信息查询产品对应的属性表信息
                            Console.WriteLine("获取产品对应的属性表信息...");
                            args = new string[1];
                            args[0] = item.SelectSingleNode("id").InnerText.Trim();
                            result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpRemsProductTec", args);
                            XmlD = new XmlDocument();
                            XmlD.LoadXml(result.ToString());
                            Xnode = XmlD.SelectSingleNode("results/flag");
                            if (Xnode != null)
                            {
                                if (Xnode.InnerText.Trim() == "0")
                                {
                                    //下载数据

                                    XmlNodeList XnodeList1 = XmlD.SelectNodes("results/messages/data/row");
                                    foreach (XmlNode iitem in XnodeList1)
                                    {
                                        SqlStr = (" delete from  J_REMS_PRODUCT_TEC where ID='" + iitem.SelectSingleNode("id").InnerText.Trim() + "'");
                                        SqlStr += "  ";
                                        //添加新值
                                        SqlStr += SqlDeal.InsertStr("J_REMS_PRODUCT_TEC",
                                                        SqlDeal.Build("ID", iitem.SelectSingleNode("id").InnerText.Trim()) +
                                                        SqlDeal.Build("REMS_PRODUCT_ID", iitem.SelectSingleNode("rems_product_id").InnerText.Trim()) +
                                                        SqlDeal.Build("MAIN_TEC", iitem.SelectSingleNode("main_tec").InnerText.Trim()) +
                                                        SqlDeal.Build("COMMON_TEC", iitem.SelectSingleNode("common_tec").InnerText.Trim()) +
                                                        SqlDeal.Build("OTHER_TEC", iitem.SelectSingleNode("other_tec").InnerText.Trim()) +
                                                        SqlDeal.Build("TEC", iitem.SelectSingleNode("tec").InnerText.Trim()) +
                                                        SqlDeal.Build("VERSION", iitem.SelectSingleNode("version").InnerText.Trim()));

                                        //插入信息
                                        SqlDeal.NonQuery(SqlStr);
                                    }
                                }
                            }

                            //获取产品与企业的关联表信息
                            Console.WriteLine("获取产品与企业的关联表信息...");
                            args = new string[1];
                            args[0] = item.SelectSingleNode("id").InnerText.Trim();
                            result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpRemsOrgInfoProduct", args);
                            XmlD = new XmlDocument();
                            XmlD.LoadXml(result.ToString());
                            Xnode = XmlD.SelectSingleNode("results/flag");
                            if (Xnode != null)
                            {
                                if (Xnode.InnerText.Trim() == "0")
                                {
                                    //下载数据

                                    XmlNodeList XnodeList2 = XmlD.SelectNodes("results/messages/data/row");
                                    foreach (XmlNode iitem2 in XnodeList2)
                                    {
                                        SqlStr = (" delete from  J_REMS_ORG_PRODUCT where ID='" + iitem2.SelectSingleNode("id").InnerText.Trim() + "'");
                                        SqlStr += "  ";
                                        //添加新值
                                        SqlStr += SqlDeal.InsertStr("J_REMS_ORG_PRODUCT",
                                                        SqlDeal.Build("ID", iitem2.SelectSingleNode("id").InnerText.Trim()) +
                                                        SqlDeal.Build("REMS_PRINT_BILL_ID", iitem2.SelectSingleNode("rems_print_bill_id").InnerText.Trim()) +
                                                        SqlDeal.Build("REMS_ORG_INFO_ID", iitem2.SelectSingleNode("rems_org_info_id").InnerText.Trim()) +
                                                        SqlDeal.Build("REMS_PRODUCT_ID", iitem2.SelectSingleNode("rems_product_id").InnerText.Trim()) +
                                                        SqlDeal.Build("IS_PROXY", iitem2.SelectSingleNode("is_proxy").InnerText.Trim()) +
                                                        SqlDeal.Build("BATCH", SqlDeal.ConvertToDecimal(iitem2.SelectSingleNode("batch").InnerText.Trim())) +
                                                        SqlDeal.Build("PRODUCT_CODE", iitem2.SelectSingleNode("product_code").InnerText.Trim()) +
                                                        SqlDeal.Build("RELATION_STATE", iitem2.SelectSingleNode("relation_state").InnerText.Trim()) +
                                                        SqlDeal.Build("ONESELF", iitem2.SelectSingleNode("oneself").InnerText.Trim()) +
                                                        SqlDeal.Build("IS_OPPOSITE", iitem2.SelectSingleNode("is_opposite").InnerText.Trim()) +
                                                        SqlDeal.Build("VERSION", SqlDeal.ConvertToDecimal(iitem2.SelectSingleNode("version").InnerText.Trim())));

                                        //插入信息
                                        SqlDeal.NonQuery(SqlStr);
                                    }
                                }
                            }



                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入" + Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion


                //=======================================================获取实体信息表
                Console.WriteLine("获取实体信息表...");
                #region
                args = new string[1];
                args[0] = Date;
                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getTpEntity", args);
                XmlD = new XmlDocument();
                XmlD.LoadXml(result.ToString());
                Xnode = XmlD.SelectSingleNode("results/flag");
                Count = 0;
                if (Xnode != null)
                {
                    if (Xnode.InnerText.Trim() == "0")
                    {
                        //下载数据
                        XnodeList = XmlD.SelectNodes("results/messages/data/row");
                        foreach (XmlNode item in XnodeList)
                        {
                            //删除重复   
                            SqlStr = (" delete from  J_TP_ENTITY where ID='" + item.SelectSingleNode("id").InnerText.Trim() + "'");
                            SqlStr += "  ";
                            //添加新值
                            SqlStr += SqlDeal.InsertStr("J_TP_ENTITY",
                                    SqlDeal.Build("ID", item.SelectSingleNode("id").InnerText.Trim()) +
                                    SqlDeal.Build("SERIAL_CODE", item.SelectSingleNode("serial_code").InnerText.Trim()) +
                                    SqlDeal.Build("S_DELIVERY_ID", item.SelectSingleNode("s_delivery_id").InnerText.Trim()) +
                                    SqlDeal.Build("S_DELIVERY_NO", item.SelectSingleNode("s_delivery_no").InnerText.Trim()) +
                                    SqlDeal.Build("S_ODD_ID", item.SelectSingleNode("s_odd_id").InnerText.Trim()) +
                                    SqlDeal.Build("S_SPD_CODE", item.SelectSingleNode("s_spd_code").InnerText.Trim()) +
                                    SqlDeal.Build("S_PDD_ID", item.SelectSingleNode("s_pdd_id").InnerText.Trim()) +
                                    SqlDeal.Build("S_SPD_SEQ", item.SelectSingleNode("s_spd_seq").InnerText.Trim()) +
                                    SqlDeal.Build("S_MATL_CODE", item.SelectSingleNode("s_matl_code").InnerText.Trim()) +
                                    SqlDeal.Build("S_MATL_NAME", item.SelectSingleNode("s_matl_name").InnerText.Trim()) +
                                    SqlDeal.Build("PRODUCT_CODE", item.SelectSingleNode("product_code").InnerText.Trim()) +
                                    SqlDeal.Build("NUM", SqlDeal.ConvertToDecimal(item.SelectSingleNode("num").InnerText.Trim())) +
                                    SqlDeal.Build("ORG_CODE", item.SelectSingleNode("org_code").InnerText.Trim()) +
                                    SqlDeal.Build("CODE_TIME", item.SelectSingleNode("code_time").InnerText.Trim()) +
                                    SqlDeal.Build("CODE_USER_ID", item.SelectSingleNode("code_user_id").InnerText.Trim()) +
                                    SqlDeal.Build("UPDATE_TIME", item.SelectSingleNode("update_time").InnerText.Trim()) +
                                    SqlDeal.Build("COLUMN1", item.SelectSingleNode("column1").InnerText.Trim()) +
                                    SqlDeal.Build("COLUMN2", item.SelectSingleNode("column2").InnerText.Trim()) +
                                    SqlDeal.Build("S_PDM_ID", item.SelectSingleNode("s_pdm_id").InnerText.Trim()));

                            //插入信息
                            SqlDeal.NonQuery(SqlStr);
                            Count++;
                            Console.WriteLine("插入数据成功....");
                        }
                        Console.WriteLine("共插入" + Count + "条数据！");
                        Console.WriteLine("=============================================================================");
                    }
                    else
                    {
                        Console.WriteLine("获取数据失败！");
                    }
                }
                #endregion

                //=======================================================获取订单主表信息
                Console.WriteLine("获取订单主表信息...");
                #region

                //现获取本地所有的企业信息
                DataTable DT = new DataTable();
                SqlStr = "  select  PROJORGCODE  from  J_REMS_ORG_INFO ";
                DT = SqlDeal.Query(SqlStr).Tables[0];
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    //获取订单主表信息
                    Console.WriteLine("获取订单主表信息...");
                    #region 获取订单数据
                    args = new string[2];
                    args[0] = DT.Rows[i]["PROJORGCODE"].ToString();
                    args[1] = Date;
                    result = WebServiceHelper.InvokeWebService(WebServicesurl, "getOrderMainInfo", args);
                    XmlD = new XmlDocument();
                    XmlD.LoadXml(result.ToString());
                    Xnode = XmlD.SelectSingleNode("results/flag");
                    if (Xnode != null)
                    {
                        if (Xnode.InnerText.Trim() == "0")
                        {
                            //下载数据
                            XmlNodeList XnodeList1 = XmlD.SelectNodes("results/messages/data/row");
                            foreach (XmlNode item in XnodeList1)
                            {
                                SqlStr = (" delete from  J_ORDER_MAIN where S_PDM_ID='" + item.SelectSingleNode("s_pdm_id").InnerText.Trim() + "'");
                                SqlStr += "  ";
                                //添加新值
                                SqlStr += SqlDeal.InsertStr("J_ORDER_MAIN",
                                            SqlDeal.Build("S_PDM_ID", item.SelectSingleNode("s_pdm_id").InnerText.Trim()) +
                                            SqlDeal.Build("S_SPD_CODE", item.SelectSingleNode("s_spd_code").InnerText.Trim()) +
                                            SqlDeal.Build("S_BM_NAME", item.SelectSingleNode("s_bm_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_PU_NAME", item.SelectSingleNode("s_pu_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_TAX_FLAG_NAME", item.SelectSingleNode("s_tax_flag_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_APUNIT_NAME", item.SelectSingleNode("s_apunit_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_RPT_ACCPTDT_CODE", item.SelectSingleNode("s_rpt_accptdt_code").InnerText.Trim()) +
                                            SqlDeal.Build("S_RPT_ACCPTDT_NAME", item.SelectSingleNode("s_rpt_accptdt_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_PLANT_NAME", item.SelectSingleNode("s_plant_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_DIRECT_FLAG", item.SelectSingleNode("s_direct_flag").InnerText.Trim()) +
                                            SqlDeal.Build("S_SETTLE_FLAG", item.SelectSingleNode("s_settle_flag").InnerText.Trim()) +
                                            SqlDeal.Build("S_PUR_PLANT_NAME", item.SelectSingleNode("s_pur_plant_name").InnerText.Trim()) +
                                            SqlDeal.Build("S_SU_CODE", item.SelectSingleNode("s_su_code").InnerText.Trim()) +
                                            SqlDeal.Build("S_SU_NAME", item.SelectSingleNode("s_su_name").InnerText.Trim()) +
                                            SqlDeal.Build("D_SUB_DATE", item.SelectSingleNode("d_sub_date").InnerText.Trim()) +
                                            SqlDeal.Build("S_CONFIRM_FLAG", item.SelectSingleNode("s_confirm_flag").InnerText.Trim()) +
                                            SqlDeal.Build("D_CONFIRM_DATE", item.SelectSingleNode("d_confirm_date").InnerText.Trim()) +
                                            SqlDeal.Build("S_CHEQUE_NO", item.SelectSingleNode("s_cheque_no").InnerText.Trim()) +
                                            SqlDeal.Build("D_CHEQUE_DATE", item.SelectSingleNode("d_cheque_date").InnerText.Trim()) +
                                            //SqlDeal.Build("S_DO_FLAG", item.SelectSingleNode("s_do_flag").InnerText.Trim()) +
                                            SqlDeal.Build("S_BM_CODE", item.SelectSingleNode("s_bm_code").InnerText.Trim()) +
                                            SqlDeal.Build("S_SYNC_FLAG", item.SelectSingleNode("s_sync_flag").InnerText.Trim()));
                                //插入信息
                                SqlDeal.NonQuery(SqlStr);
                                //循环插入订货单明细
                                Console.WriteLine("通过循环获取订单明细表信息...");
                                #region
                                args = new string[1];
                                args[0] = item.SelectSingleNode("s_pdm_id").InnerText.Trim();
                                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getOrderDetailInfo", args);
                                XmlD = new XmlDocument();
                                XmlD.LoadXml(result.ToString());
                                Xnode = XmlD.SelectSingleNode("results/flag");
                                if (Xnode != null)
                                {
                                    if (Xnode.InnerText.Trim() == "0")
                                    {
                                        XmlNodeList XnodeList2 = XmlD.SelectNodes("results/messages/data/row");
                                        foreach (XmlNode item1 in XnodeList2)
                                        {
                                            SqlStr = (" delete from  J_ORDER_DETAIL where S_PDD_ID='" + item1.SelectSingleNode("s_pdd_id").InnerText.Trim() + "'");
                                            SqlStr += "  ";
                                            //添加新值
                                            SqlStr += SqlDeal.InsertStr("J_ORDER_DETAIL",
                                                        SqlDeal.Build("S_PDD_ID", item1.SelectSingleNode("s_pdd_id").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SPD_SEQ", item1.SelectSingleNode("s_spd_seq").InnerText.Trim()) +
                                                        SqlDeal.Build("S_PDM_ID", item1.SelectSingleNode("s_pdm_id").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SPD_CODE", item1.SelectSingleNode("s_spd_code").InnerText.Trim()) +
                                                        SqlDeal.Build("S_RPT_PLAN_CODE", item1.SelectSingleNode("s_rpt_plan_code").InnerText.Trim()) +
                                                        SqlDeal.Build("S_ARR_NAME", item1.SelectSingleNode("s_arr_name").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MATL_CODE", item1.SelectSingleNode("s_matl_code").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MATL_NAME", item1.SelectSingleNode("s_matl_name").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MATL_TYPE", item1.SelectSingleNode("s_matl_type").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MATL_UNIT", item1.SelectSingleNode("s_matl_unit").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MALTP_CODE", item1.SelectSingleNode("s_maltp_code").InnerText.Trim()) +
                                                        SqlDeal.Build("S_MATL_RAW", item1.SelectSingleNode("s_matl_raw").InnerText.Trim()) +
                                                        SqlDeal.Build("N_PUR_NUM", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_pur_num").InnerText.Trim())) +
                                                        SqlDeal.Build("N_CONT_PRICE", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_cont_price").InnerText.Trim())) +
                                                        SqlDeal.Build("S_MADE_BRAND", item1.SelectSingleNode("s_made_brand").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SIGNIN_NAME", item1.SelectSingleNode("s_signin_name").InnerText.Trim()) +
                                                        SqlDeal.Build("D_SIGNIN_DATE", item1.SelectSingleNode("d_signin_date").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SIGN_FLAG", item1.SelectSingleNode("s_sign_flag").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SIGNIN_DETAIL", item1.SelectSingleNode("s_signin_detail").InnerText.Trim()) +
                                                        SqlDeal.Build("S_TAX_FLAG", item1.SelectSingleNode("s_tax_flag").InnerText.Trim()) +
                                                        SqlDeal.Build("N_TAX_RATE", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_tax_rate").InnerText.Trim())) +
                                                        SqlDeal.Build("D_DELIVER_DATE", item1.SelectSingleNode("d_deliver_date").InnerText.Trim()) +
                                                        SqlDeal.Build("S_DO_FLAG", item1.SelectSingleNode("s_do_flag").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SPD_ID", item1.SelectSingleNode("s_spd_id").InnerText.Trim()) +
                                                        SqlDeal.Build("S_SIGNIN_CODE", item1.SelectSingleNode("s_signin_code").InnerText.Trim()) +
                                                        SqlDeal.Build("N_DELIVER_NUM", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_deliver_num").InnerText.Trim())) +
                                                        SqlDeal.Build("N_REJECT_NUM", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_reject_num").InnerText.Trim())) +
                                                        SqlDeal.Build("S_SYNC_FLAG", item1.SelectSingleNode("s_sync_flag").InnerText.Trim()) +
                                                        SqlDeal.Build("S_DELIVER_FLAG", item1.SelectSingleNode("s_deliver_flag").InnerText.Trim()) +
                                                        SqlDeal.Build("N_SIGNIN_NUM", SqlDeal.ConvertToDecimal(item1.SelectSingleNode("n_signin_num").InnerText.Trim())));

                                            //插入信息
                                            SqlDeal.NonQuery(SqlStr);
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                    }


                    #endregion

                    //循环插入发货单主表
                    Console.WriteLine("循环插入发货单主表...");

                    #region
                    args = new string[2];
                    args[0] = DT.Rows[i]["PROJORGCODE"].ToString();
                    args[1] = Date;
                    result = WebServiceHelper.InvokeWebService(WebServicesurl, "getOrderDeliveryInfoChange", args);
                    XmlD = new XmlDocument();
                    XmlD.LoadXml(result.ToString());
                    Xnode = XmlD.SelectSingleNode("results/flag");
                    if (Xnode != null)
                    {
                        if (Xnode.InnerText.Trim() == "0")
                        {
                            XmlNodeList XnodeList3 = XmlD.SelectNodes("results/messages/data/row");
                            foreach (XmlNode item2 in XnodeList3)
                            {
                                SqlStr = (" delete from  J_DELIVERY_MAIN where S_DELIVERY_ID='" + item2.SelectSingleNode("s_delivery_id").InnerText.Trim() + "'");
                                SqlStr += "  ";
                                //添加新值
                                SqlStr += SqlDeal.InsertStr("J_DELIVERY_MAIN",
                                           SqlDeal.Build("S_DELIVERY_ID", item2.SelectSingleNode("s_delivery_id").InnerText.Trim()) +
                                            SqlDeal.Build("S_DELIVERY_NO", item2.SelectSingleNode("s_delivery_no").InnerText.Trim()) +
                                            SqlDeal.Build("S_PDM_ID", item2.SelectSingleNode("s_pdm_id").InnerText.Trim()) +
                                            SqlDeal.Build("S_SPD_CODE", item2.SelectSingleNode("s_spd_code").InnerText.Trim()) +
                                            SqlDeal.Build("D_DELIVER_DATE", item2.SelectSingleNode("d_deliver_date").InnerText.Trim()) +
                                            SqlDeal.Build("S_TRANS_COMPANY", item2.SelectSingleNode("s_trans_company").InnerText.Trim()) +
                                            SqlDeal.Build("S_TRANS_MODE", item2.SelectSingleNode("s_trans_mode").InnerText.Trim()) +
                                            SqlDeal.Build("D_CREATE_DATE", item2.SelectSingleNode("d_create_date").InnerText.Trim()) +
                                            SqlDeal.Build("D_MODIFY_DATE", item2.SelectSingleNode("d_modify_date").InnerText.Trim()));

                                //插入信息
                                SqlDeal.NonQuery(SqlStr);


                                //循环插入发货单明细
                                Console.WriteLine("循环插入发货单明细...");

                                #region
                                args = new string[1];
                                args[0] = item2.SelectSingleNode("s_delivery_id").InnerText.Trim();
                                result = WebServiceHelper.InvokeWebService(WebServicesurl, "getOrderDeliveryDetailInfo", args);
                                XmlD = new XmlDocument();
                                XmlD.LoadXml(result.ToString());
                                Xnode = XmlD.SelectSingleNode("results/flag");
                                if (Xnode != null)
                                {
                                    if (Xnode.InnerText.Trim() == "0")
                                    {
                                        XmlNodeList XnodeList4 = XmlD.SelectNodes("results/messages/data/row");
                                        foreach (XmlNode item3 in XnodeList4)
                                        {
                                            SqlStr = (" delete from  J_DELIVERY_DETAIL where S_PDD_ID='" + item3.SelectSingleNode("s_pdd_id").InnerText.Trim() + "'");
                                            SqlStr += "  ";
                                            //添加新值
                                            SqlStr += SqlDeal.InsertStr("J_DELIVERY_DETAIL",
                                                      SqlDeal.Build("S_ODD_ID", item3.SelectSingleNode("s_odd_id").InnerText.Trim()) +
                                                    SqlDeal.Build("S_DELIVERY_ID", item3.SelectSingleNode("s_delivery_id").InnerText.Trim()) +
                                                    SqlDeal.Build("S_DELIVERY_NO", item3.SelectSingleNode("s_delivery_no").InnerText.Trim()) +
                                                    SqlDeal.Build("S_PDD_ID", item3.SelectSingleNode("s_pdd_id").InnerText.Trim()) +
                                                    SqlDeal.Build("S_SPD_SEQ", item3.SelectSingleNode("s_spd_seq").InnerText.Trim()) +
                                                    SqlDeal.Build("S_SPD_CODE", item3.SelectSingleNode("s_spd_code").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MATL_CODE", item3.SelectSingleNode("s_matl_code").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MATL_NAME", item3.SelectSingleNode("s_matl_name").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MATL_TYPE", item3.SelectSingleNode("s_matl_type").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MATL_UNIT", item3.SelectSingleNode("s_matl_unit").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MALTP_CODE", item3.SelectSingleNode("s_maltp_code").InnerText.Trim()) +
                                                    SqlDeal.Build("S_MATL_RAW", item3.SelectSingleNode("s_matl_raw").InnerText.Trim()) +
                                                    SqlDeal.Build("D_DELIVER_DATE", item3.SelectSingleNode("d_deliver_date").InnerText.Trim()) +
                                                    SqlDeal.Build("N_DELIVER_NUM", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_deliver_num").InnerText.Trim())) +
                                                    SqlDeal.Build("S_SPD_ID", item3.SelectSingleNode("s_spd_id").InnerText.Trim()) +
                                                    SqlDeal.Build("S_PDM_ID", item3.SelectSingleNode("s_pdm_id").InnerText.Trim()) +
                                                    SqlDeal.Build("S_RPT_PLAN_CODE", item3.SelectSingleNode("s_rpt_plan_code").InnerText.Trim()) +
                                                    SqlDeal.Build("S_ARR_NAME", item3.SelectSingleNode("s_arr_name").InnerText.Trim()) +
                                                    SqlDeal.Build("N_PUR_NUM", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_pur_num").InnerText.Trim())) +
                                                    SqlDeal.Build("N_CONT_PRICE", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_cont_price").InnerText.Trim())) +
                                                    SqlDeal.Build("S_MADE_BRAND", item3.SelectSingleNode("s_made_brand").InnerText.Trim()) +
                                                    SqlDeal.Build("S_SIGNIN_CODE", item3.SelectSingleNode("s_signin_code").InnerText.Trim()) +
                                                    SqlDeal.Build("S_SIGNIN_NAME", item3.SelectSingleNode("s_signin_name").InnerText.Trim()) +
                                                    SqlDeal.Build("D_SIGNIN_DATE", item3.SelectSingleNode("d_signin_date").InnerText.Trim()) +
                                                    SqlDeal.Build("N_SIGNIN_NUM", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_signin_num").InnerText.Trim())) +
                                                    SqlDeal.Build("S_SIGNIN_DETAIL", item3.SelectSingleNode("s_signin_detail").InnerText.Trim()) +
                                                    SqlDeal.Build("S_TAX_FLAG", item3.SelectSingleNode("s_tax_flag").InnerText.Trim()) +
                                                    SqlDeal.Build("N_TAX_RATE", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_tax_rate").InnerText.Trim())) +
                                                    SqlDeal.Build("S_DELIVER_MEMO", item3.SelectSingleNode("s_deliver_memo").InnerText.Trim()) +
                                                    SqlDeal.Build("S_SIGN_FLAG", item3.SelectSingleNode("s_sign_flag").InnerText.Trim()) +
                                                    SqlDeal.Build("N_REJECT_NUM", SqlDeal.ConvertToDecimal(item3.SelectSingleNode("n_reject_num").InnerText.Trim())));

                                            //插入信息
                                            SqlDeal.NonQuery(SqlStr);
                                        }
                                    }
                                }

                                #endregion

                            }
                        }
                    }

                    #endregion

                }
                #endregion

                //=======================================================结束程序
                Console.WriteLine("执行结束...");
              //  Console.ReadLine();
                Environment.Exit(0);
            }

            catch (Exception ex)
            {
                throw;
            }

          

        }

    }
}
