using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ult.Commons;
using Ult.Util;

namespace Ult.Core.UI.Forms
{
    public partial class MessageDialog : Form
    {

        // -----------------------------------------------------------------------------------------------------------
        #region CONSTANTS

        // Windows constant to disable the closing button and handling
        private const int CP_NOCLOSE_BUTTON = 512; // 0x200

        // Padding constants
        private const int PADDING_TOP     = 12;
        private const int PADDING_LEFT    = 12;
        private const int PADDING_RIGHT   = 12;
        private const int PADDING_BOTTOM  = 12;

        // Buttons constants
        private const int BUTTONS_WIDTH   = 75;
        private const int BUTTONS_HEIGHT  = 23;


        private const int MARGIN_BUTTONS  = 6;
        private const int MARGIN_TEXTS    = 6;

        private const bool  DEFAULT_AUTOSIZE        = true;
        private const int   DEFAULT_FIXED_WIDTH     = 240;

        private const int   MIN_WIDTH               = 240;
        private const int   MIN_HEIGHT              = 120;

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region FIELDS

        private bool _canClose;

        private string _message;

        private MessageDialogOptions _options;

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region CONSTRUCTORS

        public MessageDialog() :this(null)
        {
        }

        public MessageDialog(Form owner)
        {
            // Setup the window owner
            Owner = owner;
            // 
            if (Owner != null)
            {
                Icon = Owner.Icon;
                Text = Owner.Text;
            }
            else
            {
                Icon = UIUtils.GetMainForm().Icon;
                Text = UIUtils.GetTitle();
            }
            // Init fields
            _canClose = false;
            _message = "";
            _options = new MessageDialogOptions();
            // Init compoenents
            InitializeComponent();
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PROPERTIES
        
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public MessageDialogOptions Options
        {
            get { return _options; }
            set 
            { 
                _options = value; 
                UpdateOptionsUI();
                if (Visible) UpdateUI();
            }
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PRIVATE METHODS

        /// <summary>
        /// Method overridden to block the closing button of the message box
        /// without loosing the form icon
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams mdiCp = base.CreateParams;
                mdiCp.ClassStyle = mdiCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return mdiCp;
            }
        }

        protected virtual Bitmap GetMessageIcon()
        {
            // Icon loading
            switch (_options.Icon)
            {
                case MessageDialogIcons.Information:    return SystemIcons.Information.ToBitmap();
                case MessageDialogIcons.Question:       return SystemIcons.Question.ToBitmap();
                case MessageDialogIcons.Alert:          return SystemIcons.Warning.ToBitmap();
                case MessageDialogIcons.Error:          return SystemIcons.Error.ToBitmap();
                default: return null;
            }
        }

        protected virtual int GetMessageIconWidth()
        {
            int width = 0;
            // Retreving bitmap
            using (Bitmap bmp = GetMessageIcon())
            {
                width = (bmp != null) ? bmp.Size.Width : 0;
            }
            return width;
        }

        protected virtual void UpdateUI()
        {
            UpdateIconUI();
            UpdateMessageUI();
            UpdateButtonsUI();
        }

        protected void UpdateOptionsUI()
        {
            if (_options.Layout != MessageDialogLayout.Automatic) Size = new Size(_options.Width, _options.Height);
            lblMessage.Font = _options.Font;
        }

        protected virtual void UpdateIconUI()
        {
            if (IsIconToShow())
            {
                // Retreving bitmap
                using (Bitmap bmp = GetMessageIcon())
                {
                    // Dimensions
                    picIcon.Location = new Point(PADDING_LEFT, PADDING_TOP);
                    picIcon.Size = bmp.Size;
                    picIcon.Image = (Image)bmp.Clone();
                    picIcon.Visible = true;

                }
            }
            else
            {
                // Icon hide
                picIcon.Image = null;
                picIcon.Location = new Point(0, 0);
                picIcon.Size = new Size(0, 0);
                picIcon.Visible = false;
            }
        }

        protected virtual void UpdateMessageUI()
        {
            // Check if icon is present
            bool icon = IsIconToShow();
            // Location
            lblMessage.Location = icon ? new Point(picIcon.Right + MARGIN_TEXTS, PADDING_TOP)
                                       : new Point(PADDING_LEFT, PADDING_TOP);
            // Size calculation based on layout
            switch (_options.Layout)
            {

                case MessageDialogLayout.Fixed:

                    lblMessage.MaximumSize = new Size(0, 0);
                    lblMessage.AutoSize = false;
                    lblMessage.Size = new Size(ClientSize.Width - lblMessage.Left - PADDING_RIGHT, 
                                               ClientSize.Height - PADDING_TOP - BUTTONS_HEIGHT - (MARGIN_BUTTONS * 2) - MARGIN_TEXTS);
                    lblMessage.Text = _message;
                break;

                case MessageDialogLayout.FixedWidth:

                    // Width forcing
                    lblMessage.AutoSize = false;
                    lblMessage.Size = new Size(ClientSize.Width - lblMessage.Left - PADDING_RIGHT, 24);
                    lblMessage.MaximumSize = new Size(lblMessage.Size.Width, 0);
                    lblMessage.AutoSize = true;
                    // Apply text message, label automatically calc his his height
                    lblMessage.Text = _message;
                    // Update form height
                    Height = PADDING_TOP + lblMessage.Height + MARGIN_TEXTS + MARGIN_BUTTONS + BUTTONS_HEIGHT + PADDING_BOTTOM +
                             UIUtils.GetTitlebarHeight(this) + ( UIUtils.GetBorderWidth(this) * 2 );
                break;

                case MessageDialogLayout.Automatic:

                    lblMessage.MaximumSize = new Size(0, 0);
                    lblMessage.AutoSize = true;
                    // setup text, label automatically calc his size
                    lblMessage.Text = _message;
                    // Calc total size
                    int width  = lblMessage.Right + PADDING_RIGHT + ( UIUtils.GetBorderWidth(this) * 2 );
                    int height = PADDING_TOP + lblMessage.Height + MARGIN_TEXTS + MARGIN_BUTTONS + BUTTONS_HEIGHT + PADDING_BOTTOM +
                                 UIUtils.GetTitlebarHeight(this) + ( UIUtils.GetBorderWidth(this) * 2 );
                    // Update form size
                    Width = width;
                    Height = height;

                break;
            }
        }

        protected virtual void UpdateButtonsUI()
        {
            switch (_options.Buttons)
            {
                case MessageDialogButtons.Ok:

                    // Text
                    btn01.Text = "Ok";
                    btn02.Text = "";
                    btn03.Text = "";
                    // Location
                    btn01.Location = new Point(ClientSize.Width  - BUTTONS_WIDTH  - PADDING_RIGHT , 
                                               ClientSize.Height - BUTTONS_HEIGHT - PADDING_BOTTOM);
                    btn02.Location = new Point(0, 0);
                    btn03.Location = new Point(0, 0);
                    // Result
                    btn01.DialogResult = DialogResult.OK;
                    btn02.DialogResult = DialogResult.None;
                    btn03.DialogResult = DialogResult.None;
                    // Visibility
                    btn01.Visible = true;
                    btn02.Visible = false;
                    btn03.Visible = false;
                    // Defaults
                    AcceptButton = btn01;
                    CancelButton = btn01;

                break;

                case MessageDialogButtons.OkCancel:

                    // Text
                    btn01.Text = "Ok";
                    btn02.Text = "Cancel";
                    btn03.Text = "";
                    // Location
                    btn01.Location = new Point(ClientSize.Width  - (BUTTONS_WIDTH * 2) - PADDING_RIGHT   - MARGIN_BUTTONS, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn02.Location = new Point(ClientSize.Width  - BUTTONS_WIDTH       - PADDING_RIGHT, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn03.Location = new Point(0, 0);
                    // Result
                    btn01.DialogResult = DialogResult.OK;
                    btn02.DialogResult = DialogResult.Cancel;
                    btn03.DialogResult = DialogResult.None;
                    // Visibility
                    btn01.Visible = true;
                    btn02.Visible = true;
                    btn03.Visible = false;
                    // Defaults
                    AcceptButton = btn01;
                    CancelButton = btn02;

                break;

                case MessageDialogButtons.Yes:

                    // Text
                    btn01.Text = "Yes";
                    btn02.Text = "";
                    btn03.Text = "";
                    // Location
                    btn01.Location = new Point(ClientSize.Width  - BUTTONS_WIDTH  - PADDING_RIGHT , 
                                               ClientSize.Height - BUTTONS_HEIGHT - PADDING_BOTTOM);
                    btn02.Location = new Point(0, 0);
                    btn03.Location = new Point(0, 0);
                    // Result
                    btn01.DialogResult = DialogResult.Yes;
                    btn02.DialogResult = DialogResult.None;
                    btn03.DialogResult = DialogResult.None;
                    // Visibility
                    btn01.Visible = true;
                    btn02.Visible = false;
                    btn03.Visible = false;
                    // Defaults
                    AcceptButton = btn01;
                    CancelButton = btn01;

                break;

                case MessageDialogButtons.YesNo:

                    // Text
                    btn01.Text = "Yes";
                    btn02.Text = "No";
                    btn03.Text = "";
                    // Location
                    btn01.Location = new Point(ClientSize.Width  - (BUTTONS_WIDTH * 2) - PADDING_RIGHT   - MARGIN_BUTTONS, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn02.Location = new Point(ClientSize.Width  - BUTTONS_WIDTH       - PADDING_RIGHT, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn03.Location = new Point(0, 0);
                    // Result
                    btn01.DialogResult = DialogResult.Yes;
                    btn02.DialogResult = DialogResult.No;
                    btn03.DialogResult = DialogResult.None;
                    // Visibility
                    btn01.Visible = true;
                    btn02.Visible = true;
                    btn03.Visible = false;
                    // Defaults
                    AcceptButton = btn01;
                    CancelButton = btn02;

                break;

                case MessageDialogButtons.YesNoCancel:

                    // Text
                    btn01.Text = "Yes";
                    btn02.Text = "No";
                    btn03.Text = "Cancel";
                    // Location
                    btn01.Location = new Point(ClientSize.Width  - (BUTTONS_WIDTH * 3) - PADDING_RIGHT - MARGIN_BUTTONS * 2, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn02.Location = new Point(ClientSize.Width  - (BUTTONS_WIDTH * 2) - PADDING_RIGHT - MARGIN_BUTTONS, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    btn03.Location = new Point(ClientSize.Width  - BUTTONS_WIDTH       - PADDING_RIGHT, 
                                               ClientSize.Height - BUTTONS_HEIGHT      - PADDING_BOTTOM);
                    // Result
                    btn01.DialogResult = DialogResult.Yes;
                    btn02.DialogResult = DialogResult.No;
                    btn03.DialogResult = DialogResult.Cancel;
                    // Visibility
                    btn01.Visible = true;
                    btn02.Visible = true;
                    btn03.Visible = true;
                    // Defaults
                    AcceptButton = btn01;
                    CancelButton = btn03;

                break;

            }

            // Set the focus on the first button
            btn01.Focus();

        }

        protected bool IsIconToShow()
        {
            return _options.Icon != MessageDialogIcons.None;
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region STATIC METHODS

        /// <summary>
        /// Show the dialog message, applying alle the required parameters
        /// </summary>
        /// <param name="owner">Form tha owns the message, optional. if passed as null the automatic centering of the form doesn't work</param>
        /// <param name="options">Message options</param>
        /// <param name="message">text to display with the dialog</param>
        /// <param name="message_args">Arguments to format the message text</param>
        /// <returns></returns>
        public static DialogResult Show(Control control, MessageDialogOptions options, string message, params object[] message_args)
        {
            // Gets the form who owns the control
            Form owner = control.FindForm();
            // show mesage dialog
            return Show(owner, options, message, message_args);
        }

        /// <summary>
        /// Show the dialog message, applying alle the required parameters
        /// </summary>
        /// <param name="owner">Form tha owns the message, optional. if passed as null the automatic centering of the form doesn't work</param>
        /// <param name="options">Message options</param>
        /// <param name="message">text to display with the dialog</param>
        /// <param name="message_args">Arguments to format the message text</param>
        /// <returns></returns>
        public static DialogResult Show(Form owner, MessageDialogOptions options, string message, params object[] message_args)
        {
            // Dialog creation
            MessageDialog dlg = new MessageDialog(owner);
            dlg.Options = options ?? new MessageDialogOptions();
            dlg.Message = String.Format(message, message_args ?? new object[] {});
            DialogResult result = dlg.ShowDialog();
            dlg.Close();
            return result;
        }

        #region INFO

        public static void Info(string message)
        {
            Info(null, message);
        }

        public static void Info(Form owner, string message, params object[] message_args)
        {
            Info(owner, MessageDialogOptions.Info, message, message_args);
        }

        public static void Info(Form owner, MessageDialogOptions options, string message, params object[] message_args)
        {
            // Options forcing
            options.Icon = MessageDialogIcons.Information;
            options.Buttons = MessageDialogButtons.Ok;
            // Dialog creation
            Show(owner, options, message, message_args);
        }

        #endregion INFO

        #region ALERT

        public static void Alert(string message)
        {
            Alert(null, message);
        }

        public static void Alert(Form owner, string message, params object[] message_args)
        {
            Alert(owner, MessageDialogOptions.Alert, message, message_args);
        }

        public static void Alert(Form owner, MessageDialogOptions options, string message, params object[] message_args)
        {
            // Options forcing
            options.Icon = MessageDialogIcons.Alert;
            options.Buttons = MessageDialogButtons.Ok;
            // Dialog creation
            Show(owner, options, message, message_args);
        }

        #endregion ALERT

        #region ERROR

        public static void Error(string message)
        {
            Error(null, message);
        }

        public static void Error(Form owner, string message, params object[] message_args)
        {
            Error(owner, MessageDialogOptions.Error, message, message_args);
        }

        public static void Error(Form owner, MessageDialogOptions options, string message, params object[] message_args)
        {
            // Options forcing
            options.Icon = MessageDialogIcons.Error;
            options.Buttons = MessageDialogButtons.Ok;
            // Dialog creation
            Show(owner, options, message, message_args);
        }

        #endregion ERROR

        #region YES-NO

        public static bool YesNo(string question)
        {
            return YesNo(null, question);
        }

        public static bool YesNo(string question, params object[] question_args)
        {
            return YesNo(null, question, question_args);
        }

        public static bool YesNo(Form owner, string question, params object[] question_args)
        {
            return Show(owner, MessageDialogOptions.YesNo, question, question_args) == DialogResult.Yes;
        }

        public static bool YesNo(Form owner, MessageDialogOptions options, string question, string question_args)
        {
            // Options forcing
            options.Icon = MessageDialogIcons.Question;
            options.Buttons = MessageDialogButtons.YesNo;
            // 
            return Show(owner, options, question, question_args) == DialogResult.Yes;
        }

        public static bool YesNo(Control control, string question)
        {
            return YesNo(control, question, null);
        }

        public static bool YesNo(Control control, string question, string question_args)
        {
            return YesNo(control, MessageDialogOptions.YesNo, question, question_args);
        }

        public static bool YesNo(Control control, MessageDialogOptions options, string question, string question_args)
        {
            return Show(control, options, question, question_args) == DialogResult.Yes;
        }

        #endregion YES-NO

        #region YES-NO-CANCEL

        public static DialogResult YesNoCancel(string question)
        {
            return YesNoCancel(null, question);
        }

        public static DialogResult YesNoCancel(Form owner, string question, params object[] question_args)
        {
            return Show(owner, MessageDialogOptions.YesNoCancel, question, question_args);
        }

        public static DialogResult YesNoCancel(Form owner, MessageDialogOptions options, string question, string question_args)
        {
            // Options forcing
            options.Icon = MessageDialogIcons.Question;
            options.Buttons = MessageDialogButtons.YesNo;
            // 
            return Show(owner, options, question, question_args);
        }

        #endregion YES-NO-CANCEL

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region EVENT HANDLERS

        private void MessageDialog_Load(object sender, EventArgs e)
        {
            UpdateUI();
            UIUtils.Center(this);
        }

        private void MessageDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_canClose;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            _canClose = true;
            Close();
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

    }

    public class MessageDialogOptions
    {

        // -----------------------------------------------------------------------------------------------------------
        #region CONSTANTS

        private const bool  DEFAULT_AUTOSIZE        = true;
        private const int   DEFAULT_WIDTH           = 240;
        private const int   DEFAULT_HEIGHT          = 120;

        // Dafault options configuration for common messages
        public static readonly MessageDialogOptions Info        = new MessageDialogOptions(MessageDialogIcons.Information, MessageDialogButtons.Ok);
        public static readonly MessageDialogOptions Alert       = new MessageDialogOptions(MessageDialogIcons.Alert, MessageDialogButtons.Ok);
        public static readonly MessageDialogOptions Error       = new MessageDialogOptions(MessageDialogIcons.Error, MessageDialogButtons.Ok);
        public static readonly MessageDialogOptions Question    = new MessageDialogOptions(MessageDialogIcons.Question, MessageDialogButtons.OkCancel);
        public static readonly MessageDialogOptions YesNo       = new MessageDialogOptions(MessageDialogIcons.Question, MessageDialogButtons.YesNo);
        public static readonly MessageDialogOptions YesNoCancel = new MessageDialogOptions(MessageDialogIcons.Question, MessageDialogButtons.YesNoCancel);

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region FIELDS

        private int _width;

        private int _height;

        private Font _font;

        private MessageDialogLayout _layout;

        private MessageDialogIcons _icon;

        private MessageDialogButtons _buttons;

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region CONSTRUCTORS

        public MessageDialogOptions()
        {
            _width = DEFAULT_WIDTH;
            _height = DEFAULT_HEIGHT;
            _font = SystemFonts.MessageBoxFont;
            _icon = MessageDialogIcons.None;
            _buttons = MessageDialogButtons.Ok;
            _layout = MessageDialogLayout.Automatic;
        }

        public MessageDialogOptions(MessageDialogIcons icon, MessageDialogButtons buttons) : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, SystemFonts.MessageBoxFont, MessageDialogLayout.Automatic, icon, buttons)
        {
            
        }

        public MessageDialogOptions(int width, int height, Font font, MessageDialogLayout layout, MessageDialogIcons icon, MessageDialogButtons buttons)
        {
            _width = width;
            _height = height;
            _layout = layout;
            _font = font;
            _icon = icon;
            _buttons = buttons;
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PROPERTIES

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public MessageDialogLayout Layout
        {
            get { return _layout; }
            set { _layout = value; }
        }

        public MessageDialogIcons Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public MessageDialogButtons Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------
        #region PUBLIC METHODS

        public void Copy(MessageDialogOptions options)
        {
            _width = options.Width;
            _height = options.Height;
            _layout = options.Layout;
            _icon = options.Icon;
            _buttons = options.Buttons;
        }

        public MessageDialogOptions Clone()
        {
            MessageDialogOptions options = new MessageDialogOptions();
            options.Copy(this);
            return options;
        }

        #endregion
        // -----------------------------------------------------------------------------------------------------------

    }

    public enum MessageDialogIcons
    {
        None,
        Information,
        Alert,
        Question,
        Error
    }

    public enum MessageDialogButtons
    {
        Ok,
        OkCancel,
        Yes,
        YesNo,
        YesNoCancel
    }

    public enum MessageDialogLayout
    {
        /// <summary>
        /// Message dialog will use the given width and height values and mantains the dimension
        /// </summary>
        Fixed,
        /// <summary>
        /// Message dialog will force the width of the window but recalcualtes the height basing 
        /// on the text and font given
        /// </summary>
        FixedWidth,
        /// <summary>
        /// Message dialog will calculate his size, width and height using the given text
        /// </summary>
        Automatic
    }

}
