namespace SmartPrint.Service
{
    partial class ProjectInstaller
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
            this.printServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.printServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // printServiceProcessInstaller
            // 
            this.printServiceProcessInstaller.Password = null;
            this.printServiceProcessInstaller.Username = null;
            // 
            // printServiceInstaller
            // 
            this.printServiceInstaller.ServiceName = "SmartPrintService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.printServiceProcessInstaller,
            this.printServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller printServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller printServiceInstaller;
    }
}