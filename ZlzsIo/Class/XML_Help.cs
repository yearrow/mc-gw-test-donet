using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ZlzsIo
{
   public static class XML_Help
    {


       public static string inipath = "";
      
  
       #region 文件是否存在
       public static bool ExistFile()
       {
           return File.Exists(inipath);
       }
       #endregion 

       #region 私有方法
       /// <summary>
       /// 导入XML文件
       /// </summary>
       /// <param name="XMLPath">XML文件路径</param>
       public static XmlDocument XMLLoad()
       {
           XmlDocument xmldoc = new XmlDocument();
           try
           {
               if (ExistFile())
                   xmldoc.Load(inipath);
           }
           catch (Exception e)
           {
               throw e;
           }
           return xmldoc;
       }
       #endregion

       #region 读取数据
       /// <summary>
       /// 读取指定节点的数据
       /// </summary>
       /// <param name="node">节点</param>
       /// 使用示列:
       /// XMLProsess.Read("/Node", "")
       /// XMLProsess.Read("/Node/Element[@Attribute='Name']")
       public static string Read(string node)
       {
           string value = "";
           try
           {
               XmlDocument doc = XMLLoad();
               XmlNode xn = doc.SelectSingleNode(node);
               value = xn.InnerText;
           }
           catch { }
           return value;
       }

         /// <summary>
        /// 获取某一节点的所有子节点
        /// </summary>
        /// <param name="node">要查询的节点</param>
       public static XmlNodeList ReadAllChildNode(string node)
        {
            string[] str = { };
            XmlDocument doc = XMLLoad();
            XmlNode xn = doc.SelectSingleNode(node);
            if (xn != null)
            {

                XmlNodeList nodelist = xn.ChildNodes;
                return nodelist;
            }

            else {
                return null; 
            }
        }
      #endregion
  
       #region  修改数据
       /// <summary>
       /// 修改指定节点的数据
       /// </summary>
       /// <param name="node">节点</param>
       /// <param name="value">值</param>
       public static void Update(string node, string value)
       {
           try
           {
               XmlDocument doc = XMLLoad();
               XmlNode xn = doc.SelectSingleNode(node);
               xn.InnerText = value;
               doc.Save(inipath);
           }
           catch { }
       }
        #endregion
      
       #region 新增node
       /// <summary>
       /// 插入数据
       /// </summary>
       /// <param name="path">路径</param>
       /// <param name="node">节点</param>
       /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
       /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
       /// <param name="value">值</param>
       /// 使用示列:
       /// XMLProsess.Insert(path, "/Node", "Element", "", "Value")
       /// XMLProsess.Insert(path, "/Node", "Element", "Attribute", "Value")
       /// XMLProsess.Insert(path, "/Node", "", "Attribute", "Value")
       public static void Insert(string node, string element, string attribute, string value)
       {
           try
           {
               XmlDocument doc = new XmlDocument();
               doc.Load(inipath);
               XmlNode xn = doc.SelectSingleNode(node);
               if (element.Equals(""))
               {
                   if (!attribute.Equals(""))
                   {
                       XmlElement xe = (XmlElement)xn;
                       xe.SetAttribute(attribute, value);
                   }
               }
               else
               {
                   XmlElement xe = doc.CreateElement(element);
                   if (attribute.Equals(""))
                       xe.InnerText = value;
                   else
                       xe.SetAttribute(attribute, value);
                   xn.AppendChild(xe);
               }
               doc.Save(inipath);
           }
           catch { }
       }

       /// <summary>
       /// 插入数据
       /// </summary>
       /// <param name="path">路径</param>
       /// <param name="node">节点</param>
       /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
       /// <param name="strList">由XML属性名和值组成的二维数组</param>
       public static void Insert( string node, string element, string[][] strList)
       {
           try
           {
               XmlDocument doc = new XmlDocument();
               doc.Load(inipath);
               XmlNode xn = doc.SelectSingleNode(node);
               XmlElement xe = doc.CreateElement(element);
               string strAttribute = "";
               string strValue = "";
               for (int i = 0; i < strList.Length; i++)
               {
                   for (int j = 0; j < strList[i].Length; j++)
                   {
                       if (j == 0)
                           strAttribute = strList[i][j];
                       else
                           strValue = strList[i][j];
                   }
                   if (strAttribute.Equals(""))
                       xe.InnerText = strValue;
                   else
                       xe.SetAttribute(strAttribute, strValue);
               }
               xn.AppendChild(xe);
               doc.Save(inipath);
           }
           catch { }
       }
        #endregion

       #region 反序列化
       /// <summary>
       /// 反序列化
       /// </summary>
       /// <param name="type">类型</param>
       /// <param name="xml">XML字符串</param>
       /// <returns></returns>
       public static object Deserialize(Type type, string xml)
       {
           try
           {
               using (StringReader sr = new StringReader(xml))
               {
                   XmlSerializer xmldes = new XmlSerializer(type);
                   return xmldes.Deserialize(sr);
               }
           }
           catch
           {

               return null;
           }
       }
       /// <summary>
       /// 反序列化
       /// </summary>
       /// <param name="type"></param>
       /// <param name="xml"></param>
       /// <returns></returns>
       public static object Deserialize(Type type, Stream stream)
       {
           XmlSerializer xmldes = new XmlSerializer(type);
           return xmldes.Deserialize(stream);
       }
       #endregion

       #region 序列化
       /// <summary>
       /// 序列化
       /// </summary>
       /// <param name="type">类型</param>
       /// <param name="obj">对象</param>
       /// <returns></returns>
       public static string Serializer(Type type, object obj)
       {
           MemoryStream Stream = new MemoryStream();
           XmlSerializer xml = new XmlSerializer(type);
           try
           {
               //序列化对象
               xml.Serialize(Stream, obj);
           }
           catch (InvalidOperationException)
           {
               throw;
           }
           Stream.Position = 0;
           StreamReader sr = new StreamReader(Stream);
           string str = sr.ReadToEnd();

           sr.Dispose();
           Stream.Dispose();

           return str;
       }

       #endregion
    }
}
