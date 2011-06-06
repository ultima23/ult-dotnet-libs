using System;
using System.Text;
using System.Windows.Forms;

using Matica.Commons;
using Ult.Commons;

namespace Ult.Util
{
  /// <summary>
  /// 
  /// </summary>
  public static class UIUtils
  {
  
    /// <summary>
    /// Retrieves the title to use in message boxes if the user doesnt specify a custom title
    /// </summary>
    /// <returns></returns>
    private static string GetMessageBoxTitle()
    {
      return String.Format(" {0}", Application.ProductName);
    }  
  
    /// <summary>
    /// Setup the application applying alla the standard properties to alla UI componets and forms
    /// </summary>
    /// <returns></returns>
    public static void Setup()
    {
      //
      GetMainForm().Text = " " + GetTitle();
      // apply icon to alla created forms
      SetDefaultIconToAllForms();
    }
  
    /// <summary>
    /// Retrieves the application title
    /// </summary>
    /// <returns></returns>
    public static string GetTitle()
    {
      return String.Format("{0} v{1}", Application.ProductName, Application.ProductVersion);
    }
    
    /// <summary>
    /// Retrieves the application name
    /// </summary>
    /// <returns></returns>
    public static string GetName()
    {
      return String.Format("{0}", Application.ProductName);
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Form GetMainForm()
    {
      return Application.OpenForms[0];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="form"></param>
    public static void SetDefaultIcon(Form form)
    {
      form.Icon = GetMainForm().Icon;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="form"></param>
    public static void SetDefaultIconToAllForms()
    {
      // apply main form icon to all the forms
      for (int i=1; i<Application.OpenForms.Count; i++)
      {
        SetDefaultIcon(Application.OpenForms[i]);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool Input(string message, ref string input)
    {
      return Input(GetMessageBoxTitle(), message, ref  input, new object[] {});
    }  
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="max_length"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool Input(string message, int max_length, ref string input)
    {
      return Input(GetMessageBoxTitle(), message, max_length, ref  input, new object[] {});
    }  

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool Input(string title, string message, ref string input)
    {
      return Input(title, message, ref  input, new object[] {});
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool Input(string message, ref string input, params object[] args)
    {
      return Input(GetMessageBoxTitle(), message, ref input, args);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool Input(string title, string message, ref string input, params object[] args)
    {
      return InputBox.Show(String.Format(message, args), title, ref input) == DialogResult.OK;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool Input(string title, string message, int max_length, ref string input, params object[] args)
    {
      return InputBox.Show(String.Format(message, args), title, max_length, ref input) == DialogResult.OK;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Message(string message)
    {
      Message(GetMessageBoxTitle(), message, new object[] {});
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Message(string message, params object[] args)
    {
      Message(GetMessageBoxTitle(), message, args);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Message(string title, string message, params object[] args)
    {
      MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool Confirm(string message)
    {
      return Confirm(GetMessageBoxTitle(), message, new object[] {});
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static bool Confirm(string message, params object[] args)
    {
      return Confirm(GetMessageBoxTitle(), message, args);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool Confirm(string title, string message, params object[] args)
    {
      return MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public static void Alert(string message)
    {
      Alert(GetMessageBoxTitle(), message, new object[] {});
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Alert(string message, params object[] args)
    {
      Alert(GetMessageBoxTitle(), message, args);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Alert(string title, string message, params object[] args)
    {
      MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public static void Error(string message)
    {
      Alert(GetMessageBoxTitle(), message, new string[] {});
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Error(string message, params string[] args)
    {
      Error(GetMessageBoxTitle(), message, args);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Error(string title, string message, params object[] args)
    {
      MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool YesNo(string message)
    {
        return YesNo(message, GetMessageBoxTitle(), new object[] {});
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool YesNo(string message, params object[] args)
    {
        return YesNo(message, GetMessageBoxTitle(), args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="buttons"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool YesNo(string message, string title, params object[] args)
    {
      return MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static DialogResult YesNoCancel(string message)
    {
        return YesNoCancel(message, GetMessageBoxTitle(), new object[] {});
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static DialogResult YesNoCancel(string message, params object[] args)
    {
        return YesNoCancel(message, GetMessageBoxTitle(), args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="buttons"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static DialogResult YesNoCancel(string message, string title, params object[] args)
    {
      return MessageBox.Show(String.Format(message, args), title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    }
  
  }
}
