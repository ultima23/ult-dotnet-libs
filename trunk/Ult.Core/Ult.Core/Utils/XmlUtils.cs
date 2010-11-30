using System;
using System.IO;
using System.Xml;
using System.Text;
using Ult.Commons;

namespace Ult.Util
{
  /// <summary>
  /// 
  /// </summary>
  public class XmlUtils
  {
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static XmlNode CreateNode(string xml)
    {
      return (new XmlDocument()).ReadNode(XmlReader.Create(new StringReader(xml)));
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static bool AttributeExists(XmlNode node, string attribute)
    {
      return (node.Attributes[attribute] != null);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static string ReadAttribute(XmlNode node, string attribute, string default_value)
    {
      return (node.Attributes[attribute] != null) ? node.Attributes[attribute].Value : default_value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static int ReadAttributeAsInt(XmlNode node, string attribute, int default_value)
    {
      // Value
      int value = default_value;
      // Checks attribute
      if (AttributeExists(node, attribute))
      {
        // Try to parse
        if (!Int32.TryParse(ReadAttribute(node, attribute, ""), out value))
        {
          value = default_value;
        }
      }
      return value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static float ReadAttributeAsFloat(XmlNode node, string attribute, float default_value)
    {
      // Value
      float value = default_value;
      // Checks attribute
      if (AttributeExists(node, attribute))
      {
        // Try to parse
        if (!Single.TryParse(ReadAttribute(node, attribute, ""), out value))
        {
          value = default_value;
        }
      }
      return value;
    }    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static double ReadAttributeAsDouble(XmlNode node, string attribute, double default_value)
    {
      // Value
      double value = default_value;
      // Checks attribute
      if (AttributeExists(node, attribute))
      {
        // Try to parse
        if (!Double.TryParse(ReadAttribute(node, attribute, ""), out value))
        {
          value = default_value;
        }
      }
      return value;
    }    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static bool ReadAttributeAsBool(XmlNode node, string attribute, bool default_value)
    {
      // Value
      bool value = default_value;
      // Checks attribute
      if (AttributeExists(node, attribute))
      {
        // Try to parse
        if (!Boolean.TryParse(ReadAttribute(node, attribute, ""), out value))
        {
          value = default_value;
        }
      }
      return value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attribute"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static E ReadAttributeAsEnum<E>(XmlNode node, string attribute, E default_value)
    {
      // value
      E value = default_value;
      // Check enum type
      if( !typeof(E).IsEnum )throw new NotSupportedException("E must be an Enum");
      // Checks attribute
      if (AttributeExists(node, attribute))
      {
        value = (E)Enum.Parse( typeof(E), ReadAttribute(node, attribute, ""), true );
        
      }
      return (value != null) ? value : default_value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static bool Exists(XmlNode node, string xpath)
    {
      return Select(node, xpath) != null;
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static XmlNode Select(XmlNode node, string xpath)
    {
      return node.SelectSingleNode(xpath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static XmlNodeList Search(XmlNode node, string xpath)
    {
      return node.SelectNodes(xpath);
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static string Evaluate(XmlNode node, string xpath)
    {
      return Evaluate(node, xpath, null);
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xpath"></param>
    /// <param name="default_value"></param>
    /// <returns></returns>
    public static string Evaluate(XmlNode node, string xpath, string default_value)
    {
      // Retrieve node
      XmlNode selected = Select(node, xpath);
      // Nodevalue
      if (selected != null)
      {
        switch (selected.NodeType)
        {
          case XmlNodeType.Attribute: return selected.Value;
          case XmlNodeType.Element: return selected.InnerText;
          case XmlNodeType.Text: return selected.Value;
          default: return default_value;
        }
      }
      return default_value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    public static XmlDocument Load(string file)
    {
      // Xml DOM creation
      XmlDocument doc = new XmlDocument();
      doc.Load(file);
      return doc;
    }
    
    /// <summary>
    /// Saves an xml string into an xml document
    /// </summary>
    /// <param name="xml">XML string to save into a file</param>
    /// <param name="destination"></param>
    public static void Save(string xml, string path)
    {
      // Xml DOM creation
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xml);
      // Save
      Save(doc, path);
    }
    
    /// <summary>
    /// Saves an xml dom into a file
    /// </summary>
    /// <param name="document"></param>
    /// <param name="destination"></param>
    public static void Save(XmlNode node, string path)
    {
      // Writer
      XmlWriter writer = null;
      // Saving
      try
      {
        // Settings
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = Encoding.UTF8;
        settings.Indent = true;
        // writer
        writer = XmlWriter.Create(path, settings);
        writer.WriteStartDocument(true);
        // Save
        node.WriteTo(writer);
      }
      catch (Exception ex)
      {
        Tracer.Debug(ex);
        throw ex;
      }
      finally
      {
        writer.Close();
      }
    }
    
  }
}
