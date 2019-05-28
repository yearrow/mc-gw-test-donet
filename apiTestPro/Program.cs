using Newtonsoft.Json;
using System;
using System.Data;
using System.Configuration;
using apiTestPro.Class;
using System.Text;
using System.Linq;

namespace apiTestPro
{
    class Program
    {
        public static string GetJson(DataRow r)
        {
            int index = 0;
            StringBuilder json = new StringBuilder();
            string value = "";
            foreach (DataColumn item in r.Table.Columns)
            {
                switch (item.DataType.FullName)
                {
                    case "System.Int32":
                    case "System.Decimal":
                    case "System.Int64":
                        if (r[item.ColumnName].ToString() == "")
                        {
                            json.Append(String.Format("\"{0}\":{1}", item.ColumnName, "0"));
                        }
                        else
                        {
                            json.Append(String.Format("\"{0}\":{1}", item.ColumnName, r[item.ColumnName].ToString()));
                        }
                        break;
                    case "System.Boolean":
                        if (r[item.ColumnName].ToString() == "")
                        {
                            json.Append(String.Format("\"{0}\":{1}", item.ColumnName, "0"));
                        }
                        else
                        {
                            value = r[item.ColumnName].ToString();
                            json.Append(String.Format("\"{0}\":{1}", item.ColumnName, value.First().ToString().ToLower() + value.Substring(1)));
                        }
                        break;
                    default:
                        value = r[item.ColumnName].ToString().Replace("\\", "/").Replace("\"", "");
                        json.Append(String.Format("\"{0}\":\"{1}\"", item.ColumnName, value));
                        break;
                }

                if (index < r.Table.Columns.Count - 1)
                {
                    json.Append(", ");
                }
                index++;
            }
            return "{" + json.ToString() + "}";
        }

        public static void  UpdateFlag(DataTable DT, string TableName) {
            string OrgId = ConfigurationSettings.AppSettings["OrgId"];
            string idStr = "(";
            if (DT.Rows.Count > 0) { 
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    if (i + 1 != DT.Rows.Count)
                    {
                        idStr += "'" + DT.Rows[i]["id"].ToString() + "',";
                    }
                    else
                    {
                        idStr += "'" + DT.Rows[i]["id"].ToString() + "')";
                    }
                }
                string SqlStr = "update " + TableName + " set flag = 1 where orgid = '"+ OrgId + "' and id  in " + idStr;
                SqlDeal.NonQuery(SqlStr);
            }
        }
        public static DataTable UploadData(McHttpDto dto,string SqlStr, string resource) {
            Int64 _OrgId = Int64.Parse(ConfigurationSettings.AppSettings["_OrgId"]);
            string jsonData = string.Empty;
            DataTable DT = SqlDeal.Query(SqlStr).Tables[0];
            // 处理转化
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (i + 1 != DT.Rows.Count)
                {
                    jsonData += GetJson(DT.Rows[i]) + ",";
                }
                else
                {
                    jsonData += GetJson(DT.Rows[i]) ;
                }
            }
            // jsonData = JsonConvert.SerializeObject(DT);
            Console.WriteLine(jsonData);
            jsonData ="{\""+ resource+"\":{\"orgId\":"+ _OrgId + ",\"data\": [" + jsonData + "] }}";
            //dto.apiStr = "external/mquantitys-upload?tag=mix&resource=" + resource + "&version=1";
            //dto.ParasStr = "external/mquantitys-upload?tag=mix&resource=" + resource + "&version=1";
            dto.apiStr = "/api/mquantity/mquantitys-upload?resource=" + resource + "&tag=mix&version=1";
            dto.ParasStr = "/api/mquantity/mquantitys-upload?&resource=" + resource + "&tag=mixversion=1";
            YLHttpHelperMc client = new YLHttpHelperMc(null);
            ResultDto result = JsonConvert.DeserializeObject<ResultDto>(client.HttpPost(dto, jsonData));
            if (result.success)
            {
                return DT;
            }
            else {
                return new DataTable();
            }
            
        }
        static void Main()
        {
            string SqlStr = string.Empty;
            McHttpDto dto = new McHttpDto();
            int DataCount = int.Parse(ConfigurationSettings.AppSettings["DataCount"]);
            string OrgId= ConfigurationSettings.AppSettings["OrgId"];
            dto.AccessId = ConfigurationSettings.AppSettings["AccessId"];
            dto.SecrectKey = ConfigurationSettings.AppSettings["SecrectKey"];
            dto.Verb = "POST";
            dto.ContentType = "application/json; charset=utf-8";
            dto.BaseUrl = ConfigurationSettings.AppSettings["BaseUrl"];
            try
            {
                //=======================================================
                Console.WriteLine("获取手工打料信息并上传...");
                #region

                    SqlStr = (" select top "+DataCount*10+ " [orgId],[id] ,dosageId,[orgName] ,[proLine] ,[station] ,[datTim] ,[material] ,[operator] ,[factAmnt] ,[watFull] from manual  where orgid ='" + OrgId + "' and isnull(flag,0) != 1");
                     UpdateFlag(UploadData(dto, SqlStr, "qManual"), "manual");
                    // 发送成功
                    Console.WriteLine("手工打料数据上传成功！");

                #endregion

                //=======================================================
                Console.WriteLine("获取车信息并上传...");
                #region
                // 一次性上传所有车
                SqlStr = (" select top " + DataCount + "[orgId],[id] ,[orgName],[proLine] ,[station],[scheduleId],[taskNo],[consPos] ,[betLev],[vehicle],[driver],[pieCnt] ,[projectName],[customer],[datTim] ,[operator],[lands],[planMete],[morMete],[prodMete],[reciepeNo],[carAmnt] ,[pour],[qualitor] ,[watFull],[TransMete] from produce  where orgid = '" + OrgId + "' and  isnull(flag,0) != 1");
                DataTable produceDT = UploadData(dto, SqlStr, "qProduce");
                UpdateFlag(produceDT, "produce");

                Console.WriteLine("车统计数据上传成功！");

                for (int i = 0; i < produceDT.Rows.Count; i++)
                {
                    //循环上传盘数据
                    String SqlStrPiece = (" select [orgId],[id] ,[orgName],[proLine],[station],[scheduleId] ,[taskNo] ,[consPos] ,[betLev],[vehicle],[driver],[pieCnt],[projectName],[customer],[datTim],[operator],[lands],[planMete],[morMete],[prodMete],[reciepeNo],[pour],[qualitor],[watFull],[TransMete],[pieceId],[pieAmnt] from piece  where orgid = '" + OrgId + "' and  isnull(flag,0) != 1 and proLine='" + produceDT.Rows[i]["proLine"].ToString() + "' and scheduleId='" + produceDT.Rows[i]["scheduleId"].ToString() + "'");
                    DataTable PieceDT = UploadData(dto, SqlStrPiece, "qPiece");
                    UpdateFlag(PieceDT, "piece");
                    Console.WriteLine("盘统计数据上传成功！");

                    //循环上传明细数据
                    String SqlStrDosage = (" select [orgId] ,[id] ,[orgName],[proLine],[station],[scheduleId],[taskNo] ,[consPos],[betLev] ,[vehicle],[driver],[pieCnt] ,[projectName],[customer],[datTim],[operator] ,[lands],[planMete],[morMete],[prodMete],[reciepeNo],[pour] ,[qualitor] ,[carAmnt] ,[watFull],[pieceId] ,[pieAmnt],[dosageId] ,[fimTim],[planAmn],[factAmnt],[material],[TransMete] from dosage  where orgid = '" + OrgId + "' and  isnull(flag,0) != 1 and proLine='" + produceDT.Rows[i]["proLine"].ToString() + "' and scheduleId='" + produceDT.Rows[i]["scheduleId"].ToString() + "'");
                    DataTable DosageDT = UploadData(dto, SqlStrDosage, "qDosage");
                    UpdateFlag(DosageDT, "dosage");
                    Console.WriteLine("明细数据上传成功！");

                }
                #endregion

                //=======================================================结束程序
                Console.WriteLine("执行结束...");
                //Console.ReadLine();
            }

            catch (Exception ex)
            {
                throw;
            }

          

        }

    }
}
