namespace OfflineDataSyncDemo.Module.Controllers
{
    partial class SyncController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Sync = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Sync
            // 
            this.Sync.Caption = "Sync";
            this.Sync.ConfirmationMessage = null;
            this.Sync.Id = "ac45f018-9a4d-4d7a-9e44-226934c43774";
            this.Sync.ToolTip = null;
            this.Sync.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Sync_Execute);
            // 
            // SyncController
            // 
            this.Actions.Add(this.Sync);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Sync;
    }
}
