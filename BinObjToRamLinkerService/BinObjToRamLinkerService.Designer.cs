namespace BOToRamLinker
{
	partial class BinObjToRamLinkerService
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
			if (disposing && (components != null)) {
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
			this.Logger = new System.Diagnostics.EventLog();
			this.SolutionsDirectoryWatcher = new System.IO.FileSystemWatcher();
			((System.ComponentModel.ISupportInitialize)(this.Logger)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SolutionsDirectoryWatcher)).BeginInit();
			// 
			// SolutionsDirectoryWatcher
			// 
			this.SolutionsDirectoryWatcher.EnableRaisingEvents = true;
			this.SolutionsDirectoryWatcher.NotifyFilter = System.IO.NotifyFilters.DirectoryName;
			// 
			// BinObjToRamLinkerService
			// 
			this.ServiceName = "BinObjToRamLinkerService";
			((System.ComponentModel.ISupportInitialize)(this.Logger)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SolutionsDirectoryWatcher)).EndInit();

		}

		#endregion
		private System.IO.FileSystemWatcher SolutionsDirectoryWatcher;
		internal System.Diagnostics.EventLog Logger;
	}
}
