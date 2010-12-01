using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Matica.Commons
{
  /// <summary>
  /// 
  /// </summary>
  public class InputBox
  {
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DialogResult Show(string message, string title, ref string value)
    {
      return Show(message, title, Int32.MaxValue, ref value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="promptText"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DialogResult Show(string message, string title, int max_length, ref string value)
    {
      Form form = new Form();
      Label label = new Label();
      TextBox textBox = new TextBox();
      Button buttonOk = new Button();
      Button buttonCancel = new Button();

      form.Text = title;
      label.Text = message;
      textBox.Text = value;
      textBox.MaxLength = max_length;

      buttonOk.Text = "OK";
      buttonCancel.Text = "Cancel";
      buttonOk.DialogResult = DialogResult.OK;
      buttonCancel.DialogResult = DialogResult.Cancel;

      label.SetBounds(9, 20, 372, 13);
      textBox.SetBounds(12, 36, 372, 20);
      buttonOk.SetBounds(228, 72, 75, 23);
      buttonCancel.SetBounds(309, 72, 75, 23);

      label.AutoSize = true;
      textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
      buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

      form.ClientSize = new Size(396, 107);
      form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
      form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.MinimizeBox = false;
      form.MaximizeBox = false;
      form.AcceptButton = buttonOk;
      form.CancelButton = buttonCancel;

      DialogResult dialogResult = form.ShowDialog();
      value = textBox.Text;
      return dialogResult;
    }
  
  }
}
