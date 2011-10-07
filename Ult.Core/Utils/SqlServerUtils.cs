using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

using Ult.Commons;


namespace Ult.Util
{
    /// <summary>
    /// Microsoft SQL Server handling utilities
    /// </summary>
    public class SqlServerUtils
    {
        

        private const string DEFAULT_SERVICE_NAME           = "MSSQLSERVER";
        private const int DEFAULT_START_TIMEOUT             = 30000;
        private const int DEFAULT_STOP_TIMEOUT              = 30000;

        private const string DEFAULT_START_MESSAGE          = "Database server is starting ...";
        private const string DEFAULT_STOP_MESSAGE           = "Database server is stopping ...";

        /// <summary>
        /// Check if the given SQL Server instance service exists
        /// </summary>
        /// <param name="service">SQL Server instance Windows Service name</param>
        /// <returns>True if the service exists</returns>
        public static bool Exists(string service)
        {
            return ServiceManager.Exists(service);
        }

        /// <summary>
        /// Check if the default SQL Server instance service exists
        /// </summary>
        /// <returns>True if the service exists</returns>
        public static bool Exists()
        {
            return Exists(DEFAULT_SERVICE_NAME);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool StartServer()
        {
            return StartServer(DEFAULT_SERVICE_NAME, DEFAULT_START_TIMEOUT, DEFAULT_START_MESSAGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool StartServer(int timeout)
        {
            return StartServer(DEFAULT_SERVICE_NAME, timeout, DEFAULT_START_MESSAGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StartServer(int timeout, string message)
        {
            return StartServer(DEFAULT_SERVICE_NAME, timeout, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StartServer(int timeout, string message, Form owner)
        {
            return StartServer(DEFAULT_SERVICE_NAME, timeout, message, owner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StartServer(string service, int timeout, string message)
        {
            return StartServer(service, timeout, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StartServer(string service, int timeout, string message, Form owner)
        {
            bool success = false;
            try
            {
                // Check if the service is running
                if (!ServiceManager.IsRunning(DEFAULT_SERVICE_NAME))
                {
                    // Waiting form
                    FormSqlServerWait wait_form = new FormSqlServerWait(owner);
                    wait_form.Message = message;
                    // Start action
                    SqlServerAction action = new SqlServerAction();
                    SqlServerAction.SqlServerActionDelegate action_delegate = new SqlServerAction.SqlServerActionDelegate(action.Start);
                    // Start action async execution, passing the form as state object
                    IAsyncResult ar = action_delegate.BeginInvoke(service,
                                                                  timeout,
                                                                  new AsyncCallback(action.StartCallback),
                                                                  wait_form);
                    // Show form
                    wait_form.Start();
                    // returns operation result
                    success = wait_form.Success;
                    /*

                    // Waiting form
                    FormSqlServerWait wait_form = new FormSqlServerWait();
                    wait_form.Message = "Database server is starting...";
                    wait_form.Start();

                    SqlServerAction action = new SqlServerAction();
                    SqlServerAction.SqlServerActionDelegate action_delegate = new SqlServerAction.SqlServerActionDelegate(action.Start);

                    // Async execution
                    IAsyncResult ar = action_delegate.BeginInvoke(DEFAULT_SERVICE_NAME,
                                                                  SQLSERVER_START_TIMEOUT,
                                                                  null,
                                                                  null);

                    // Poll while simulating work.
                    while(ar.IsCompleted == false)
                    {
                        Application.DoEvents();
                        Thread.Sleep(25);
                    }

                    // Call EndInvoke to retrieve the results.
                    success = action_delegate.EndInvoke(ar);

                    // Hide waiting form
                    wait_form.Stop();
                    */
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool StopServer()
        {
            return StopServer(DEFAULT_SERVICE_NAME, DEFAULT_STOP_TIMEOUT, DEFAULT_STOP_MESSAGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool StopServer(int timeout)
        {
            return StopServer(DEFAULT_SERVICE_NAME, timeout, DEFAULT_STOP_MESSAGE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StopServer(int timeout, string message)
        {
            return StopServer(DEFAULT_SERVICE_NAME, timeout, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StopServer(int timeout, string message, Form owner)
        {
            return StopServer(DEFAULT_SERVICE_NAME, timeout, message, owner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StopServer(string service, int timeout, string message)
        {
            return StopServer(service, timeout, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeout"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool StopServer(string service, int timeout, string message, Form owner)
        {
            bool success = false;
            try
            {
                // Check if the service is running
                if (ServiceManager.IsRunning(DEFAULT_SERVICE_NAME))
                {
                    // Waiting form
                    FormSqlServerWait wait_form = new FormSqlServerWait(owner);
                    wait_form.Message = message;
                    // Start action
                    SqlServerAction action = new SqlServerAction();
                    SqlServerAction.SqlServerActionDelegate action_delegate = new SqlServerAction.SqlServerActionDelegate(action.Stop);
                    // Start action async execution, passing the form as state object
                    IAsyncResult ar = action_delegate.BeginInvoke(service,
                                                                  timeout,
                                                                  new AsyncCallback(action.StopCallback),
                                                                  wait_form);
                    // Show form
                    wait_form.Start();
                    // returns operation result
                    success = wait_form.Success;
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database_name"></param>
        /// <param name="database_file"></param>
        /// <returns></returns>
        public static bool AttachDatabase(string database_name, string database_file)
        {
            bool success = false;
            MessageBox.Show("TODO!");
            return success;
        }


        /// <summary>
        /// 
        /// </summary>
        private class SqlServerAction
        {

            public bool Start(string service, int timeout)
            {
                bool success = false;
                try
                {
                    success = ServiceManager.Start(service, timeout);
                }
                catch (Exception ex)
                {
                    success = false;
                }
                return success;
            }

            public bool Stop(string service, int timeout)
            {
                bool success = false;
                try
                {
                    success = ServiceManager.Stop(service, timeout);
                }
                catch (Exception ex)
                {
                    success = false;
                }
                return success;
            }

            public void StartCallback(IAsyncResult result)
            {
                // Retrieving async result
                AsyncResult ar = (AsyncResult) result;
                // Retrieving the delegate
                SqlServerActionDelegate del = (SqlServerActionDelegate) ar.AsyncDelegate;
                // retrieveing the form (state object)
                FormSqlServerWait wait_form = (FormSqlServerWait) ar.AsyncState;
                // retrieve the execution result
                bool success = del.EndInvoke(result);
                // Avoid that the execution ends before the wait form is shown
                while (!wait_form.Visible)
                {
                    Thread.Sleep(25);
                }
                // Closes the form
                wait_form.Stop(success);
            }

            public void StopCallback(IAsyncResult result)
            {
                // Retrieving async result
                AsyncResult ar = (AsyncResult) result;
                // Retrieving the delegate
                SqlServerActionDelegate del = (SqlServerActionDelegate) ar.AsyncDelegate;
                // retrieveing the form (state object)
                FormSqlServerWait wait_form = (FormSqlServerWait) ar.AsyncState;
                // retrieve the execution result
                bool success = del.EndInvoke(result);
                // Avoid that the execution ends before the wait form is shown
                while (!wait_form.Visible)
                {
                    Thread.Sleep(25);
                }
                // Closes the form
                wait_form.Stop(success);
            }

            public delegate bool SqlServerActionDelegate(string service, int timeout);
            
        }

        /// <summary>
        /// Internal form used to notify the user to wait server operations
        /// </summary>
        private class FormSqlServerWait : Form
        {

            // -----------------------------------------------------------------------------------------------------------
            #region CONSTANTS

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region FIELDS

            // Closing flag to avoid user cancelling
            private bool _canClose = false;
            // Operation success
            private bool _success = false;

            // Components
            private System.Windows.Forms.Label lblMessage;
            private System.Windows.Forms.ProgressBar prgProgress;

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region CONSTRUCTORS

            public FormSqlServerWait() : this(null)
            {   
            }

            public FormSqlServerWait(Form owner)
            {
                if (owner != null) this.Owner = owner;
                InitializeComponent();
            }

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region PROPERTIES

            /// <summary>
            /// 
            /// </summary>
            public bool Success
            {
                get { return _success; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Message
            {
                get { return Text; }
                set
                {
                    Text = value;
                    lblMessage.Text = value;
                }
            }

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region PRIVATE METHODS

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.lblMessage = new System.Windows.Forms.Label();
                this.prgProgress = new System.Windows.Forms.ProgressBar();
                this.SuspendLayout();
                // 
                // label1
                // 
                this.lblMessage.AutoSize = true;
                this.lblMessage.Location = new System.Drawing.Point(12, 9);
                this.lblMessage.Name = "lblMessage";
                this.lblMessage.Size = new System.Drawing.Size(139, 13);
                this.lblMessage.TabIndex = 0;
                this.lblMessage.Text = "Starting service, please wait";
                // 
                // progressBar1
                // 
                this.prgProgress.Location = new System.Drawing.Point(15, 30);
                this.prgProgress.Name = "prgProgress";
                this.prgProgress.Size = new System.Drawing.Size(280, 16);
                this.prgProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                this.prgProgress.TabIndex = 1;
                // 
                // FormWait
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(307, 58);
                this.ControlBox = false;
                this.Controls.Add(this.prgProgress);
                this.Controls.Add(this.lblMessage);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.Name = "FormWait";
                this.ShowIcon = false;
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.Text = "Starting service, please wait";
                this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWait_FormClosing);
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region PUBLIC METHODS

            /// <summary>
            /// 
            /// </summary>
            public void Start()
            {
                ShowDialog();
                _canClose = false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="success"></param>
            public void Stop(bool success)
            {
                _canClose = true;
                _success = success;
                // result
                DialogResult = DialogResult.OK;
            }

            #endregion
            // -----------------------------------------------------------------------------------------------------------

            // -----------------------------------------------------------------------------------------------------------
            #region EVENT HANDLERS

            private void FormWait_FormClosing(object sender, FormClosingEventArgs e)
            {
                e.Cancel = !_canClose;
            }

            // ---

            #endregion
            // -----------------------------------------------------------------------------------------------------------
            
        }

    }
}
