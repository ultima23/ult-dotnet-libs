using System;
using System.Text;
using System.Security;
using System.Collections.Generic;
using Ult.Util;

namespace Ult.Commons
{
  /// <summary>
  /// 
  /// </summary>
  public class XmlStringBuilder
  {
  
    // -----------------------------------------------------------------------------------------------------------
    #region CONSTANTS
    
    // Formats
    private const string __FORMAT_NAMESPACE                   = "xmlns:{0}=\"{1}\"";
    
    private const string __FORMAT_TAG_OPEN                    = "<{0}>";
    private const string __FORMAT_TAG_OPEN_NAMESPACE          = "<{0}:{1}>";
    private const string __FORMAT_TAG_CLOSE                   = "</{0}>";
    private const string __FORMAT_TAG_CLOSE_NAMESPACE         = "</{0}:{1}>";
    private const string __FORMAT_TAG_BEGIN                   = "<{0} ";
    private const string __FORMAT_TAG_BEGIN_NAMESPACE         = "<{0}:{1} ";
    private const string __FORMAT_TAG_END                     = ">";
    private const string __FORMAT_TAG_END_EMPTY               = "/>";
    
    private const string __FORMAT_TAG                         = "<{0}>{1}</{0}>";
    private const string __FORMAT_TAG_CDATA                   = "<{0}><![CDATA[{1}]]></{0}>";    
    private const string __FORMAT_TAG_NAMESPACE               = "<{0}:{1}>{2}</{0}:{1}>";
    private const string __FORMAT_TAG_CDATA_NAMESPACE         = "<{0}:{1}><![CDATA[{2}]]></{0}:{1}>";
    
    private const string __FORMAT_ATTRIBUTE                   = "{0}=\"{1}\" ";
    private const string __FORMAT_ATTRIBUTE_NAMESPACE         = "{0}:{1}=\"{2}\" ";
    private const string __FORMAT_CDATA                       = "<![CDATA[{0}]]> ";
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------
  
    // -----------------------------------------------------------------------------------------------------------
    #region FIELDS
  
    /// <summary>
    /// Internal xml string buffer
    /// </summary>
    private StringBuilder _builder;
  
    #endregion
    // -----------------------------------------------------------------------------------------------------------
  
    // -----------------------------------------------------------------------------------------------------------
    #region CONSTRUCTORS
  
    /// <summary>
    /// 
    /// </summary>
    public XmlStringBuilder()
    {
      _builder = new StringBuilder();
    }

    #endregion
    // -----------------------------------------------------------------------------------------------------------    
    
    // -----------------------------------------------------------------------------------------------------------
    #region PUBLIC METHODS
    
    /// <summary>
    /// Begin wrie a tag into the buffer, without namespace.
    /// Useful to add custom attributes at the tag using the buffer.
    /// To close the tag use EndTag() method
    /// </summary>
    /// <param name="tag_name"></param>
    public void BeginTag(string tag_name)
    {
      BeginTag(null, tag_name);
    }
    
    /// <summary>
    /// Opens a tag with specified namespace and tag name without closing the tag,
    /// to allow to add xml attributes.
    /// To close the tag use EndTag() method
    /// </summary>
    /// <param name="tag_name"></param>
    public void BeginTag(string tag_namespace, string tag_name)
    {
      // Parameters check
      if (!IsValidTag(tag_name)) throw new ArgumentException("tag_name", String.Format("Tag name '{0}' is not valid as xml node name", tag_name));
      //
      if (String.IsNullOrEmpty(tag_namespace))
      {
        _builder.AppendFormat(__FORMAT_TAG_BEGIN, tag_name);
      }
      else
      {
        _builder.AppendFormat(__FORMAT_TAG_BEGIN_NAMESPACE, tag_namespace, tag_name);
      }
    }
    
    /// <summary>
    /// Close a tag with the '>' bracket
    /// </summary>
    public void EndTag()
    {
      _builder.Append(__FORMAT_TAG_END);
    }
    
    /// <summary>
    /// Closes an empty-text tag with <code>/&gt</code> bracket
    /// </summary>
    public void EndEmptyTag()
    {
      _builder.Append(__FORMAT_TAG_END_EMPTY);
    }
    
    /// <summary>
    /// Opens a node appending tag <code>&lt;tag_name&gt;</code> to the buffer
    /// </summary>
    /// <param name="tag_name"></param>
    public void BeginNode(string tag_name)
    {
      BeginNode(null, tag_name);
    }
    
    /// <summary>
    /// Begins a node (without end it) appending tag <code>&lt;tag_namespace:tag_name&gt;</code> to the buffer
    /// </summary>
    /// <param name="tag_namespace"></param>
    /// <param name="tag_name"></param>
    public void BeginNode(string tag_namespace, string tag_name)
    {
      // Parameters check
      if (!IsValidTag(tag_name)) throw new ArgumentException("tag_name", String.Format("Tag name '{0}' is not valid as xml node name", tag_name));
      //
      if (String.IsNullOrEmpty(tag_namespace))
      {
        _builder.AppendFormat(__FORMAT_TAG_OPEN, tag_name);
      }
      else
      {
        _builder.AppendFormat(__FORMAT_TAG_OPEN_NAMESPACE, tag_namespace, tag_name);
      }
    }
    
    /// <summary>
    /// Closes a node appending closing <code>tag &lt;/tag_name&gt;</code> to the buffer
    /// </summary>
    /// <param name="tag_namespace"></param>
    /// <param name="tag_nam"></param>
    public void EndNode(string tag_name)
    {
      EndNode(null, tag_name);
    }
    
    /// <summary>
    /// Closes a node appending closing <code>&lt;/tag_namespace:tag_name&gt;</code>to the buffer
    /// </summary>
    /// <param name="tag_namespace"></param>
    /// <param name="tag_nam"></param>
    public void EndNode(string tag_namespace, string tag_name)
    {
      // Parameters check
      if (!IsValidTag(tag_name)) throw new ArgumentException("tag_name", String.Format("Tag name '{0}' is not valid as xml node name", tag_name));
      //
      if (String.IsNullOrEmpty(tag_namespace))
      {
        _builder.AppendFormat(__FORMAT_TAG_CLOSE, tag_name);
      }
      else
      {
        _builder.AppendFormat(__FORMAT_TAG_CLOSE_NAMESPACE, tag_namespace, tag_name);
      }
    }

    /// <summary>
    /// Appends a complete tag to the buffer, adding CDATA section if specified.
    /// </summary>
    /// <param name="tag_name"></param>
    /// <param name="tag_text"></param>
    /// <param name="use_cdata"></param>
    public void AppendTag(string tag_name, string tag_text, bool use_cdata)
    {
      AppendTag(null, tag_name, tag_text, use_cdata);
    }

    /// <summary>
    /// Appends a complete tag to the buffer, adding CDATA section if specified.
    /// </summary>
    /// <param name="tag_namespace"></param>
    /// <param name="tag_name"></param>
    /// <param name="tag_text"></param>
    /// <param name="use_cdata"></param>
    public void AppendTag(string tag_namespace, string tag_name, string tag_text, bool use_cdata)
    {
      // Parameters check
      if (!IsValidTag(tag_name)) throw new ArgumentException("tag_name", String.Format("Tag name '{0}' is not valid as xml node name", tag_name));
      if (!use_cdata && !IsValidText(tag_text)) throw new ArgumentException("tag_text", "Text is not valid as xml node text");
      //       
      if (String.IsNullOrEmpty(tag_namespace))
      {
        if (!use_cdata)
        {
          _builder.AppendFormat(__FORMAT_TAG, tag_name, Escape(tag_text));
        }
        else
        {
          _builder.AppendFormat(__FORMAT_TAG_CDATA, tag_name, tag_text);
        }
      }
      else
      {
        if (!use_cdata)
        {
          _builder.AppendFormat(__FORMAT_TAG_NAMESPACE, tag_namespace, tag_name, Escape(tag_text));
        }
        else
        {
          _builder.AppendFormat(__FORMAT_TAG_CDATA_NAMESPACE, tag_namespace, tag_name, tag_text);
        }
      }
    }

    /// <summary>
    /// Appends a namespace declaration to xml buffer
    /// </summary>
    /// <param name="tag"></param>
    public void AppendNamespace(string namespace_alias, string namespace_uri)
    {
      _builder.AppendFormat(__FORMAT_NAMESPACE, namespace_alias, namespace_uri);
    }
    
    /// <summary>
    /// Appends an attribute to xml buffer
    /// </summary>
    /// <param name="tag"></param>
    public void AppendAttribute(string attribute_name, string attribute_value)
    {
      AppendAttribute(null, attribute_name, attribute_value);
    }
    
    /// <summary>
    /// Appends an attribute to xml buffer, if specified adds attribute namespace
    /// </summary>
    /// <param name="tag"></param>
    public void AppendAttribute(string attribute_namespace, string attribute_name, string attribute_value)
    {
      // Parameters check
      if (!IsValidAttributeName(attribute_name)) throw new ArgumentException("attribute_name", "Attribute name is not valid for an xml attribute");
      if (!IsValidAttributeValue(attribute_value)) throw new ArgumentException("attribute_value", "Attribute value is not valid for an xml attribute");
      //
      if (String.IsNullOrEmpty(attribute_namespace))
      {
        _builder.AppendFormat(__FORMAT_ATTRIBUTE, attribute_name, Escape(attribute_value));
      }
      else
      {
        _builder.AppendFormat(__FORMAT_ATTRIBUTE_NAMESPACE, attribute_namespace, attribute_name, Escape(attribute_value));
      }
    }

    /// <summary>
    /// Appends text to xml buffer
    /// </summary>
    /// <param name="tag"></param>
    public void AppendText(string text)
    {
      if (!IsValidText(text)) throw new ArgumentException("text", "Text is not valid xml node text");
      _builder.Append(Escape(text));
    }
    
    /// <summary>
    /// Appends CDDATA section to the buffer
    /// </summary>
    /// <param name="tag"></param>
    public void AppendCDataText(string text)
    {
      _builder.AppendFormat(__FORMAT_CDATA, text);
    }    

    /// <summary>
    /// Appends row xml string to the buffer
    /// </summary>
    /// <param name="xml"></param>
    public void AppendXml(string xml)
    {
      AppendXml(xml, true);
    }

    /// <summary>
    /// Append an xml chunk to the buffer
    /// </summary>
    /// <param name="xml"></param>
    public void AppendXml(string xml, bool validate)
    {
      if (validate && !IsValidXml(xml)) throw new ArgumentException("xml", "Xml string is not a valid and well-formed xml string");
      _builder.Append(xml);
    }
    
    /// <summary>
    /// Clears internal xml string buffer
    /// </summary>
    public void Clear()
    {
      _builder.Length =0;
    }
    
    /// <summary>
    /// Clears internal xml string buffer
    /// </summary>
    public new string ToString()
    {
      return _builder.ToString();
    }
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------
  
    // -----------------------------------------------------------------------------------------------------------
    #region STATIC METHODS
  
    /// <summary>
    /// Validates an xml string
    /// </summary>
    /// <returns></returns>
    public static bool IsValidXml(string xml)
    {
      try
      {
        XmlUtils.CreateNode(xml);
        return true;
      }
      catch (Exception)
      {
        return true;
      }
    }
    
    /// <summary>
    /// XML forbidden values/chars escaping
    /// </summary>
    /// <returns></returns>
    public static string Escape(string xml)
    {
      return SecurityElement.Escape(xml);
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag_name"></param>
    /// <returns></returns>
    public static bool IsValidText(string tag_text)
    {
      return SecurityElement.IsValidText(tag_text);
    }    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag_name"></param>
    /// <returns></returns>
    public static bool IsValidTag(string tag_name)
    {
      return SecurityElement.IsValidTag(tag_name);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="attr_name"></param>
    /// <returns></returns>
    public static bool IsValidAttributeName(string attr_name)
    {
      return SecurityElement.IsValidAttributeName(attr_name);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValidAttributeValue(string attr_value)
    {
      return SecurityElement.IsValidAttributeValue(attr_value);
    }
    
    #endregion
    // -----------------------------------------------------------------------------------------------------------
  
  }
}
