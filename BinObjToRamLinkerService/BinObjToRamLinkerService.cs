using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BOToRamLinker.Properties;

namespace BOToRamLinker
{
	public partial class BinObjToRamLinkerService : ServiceBase
	{
		private List<CustomerProjectDirectoryWatcher> _customerDirectoriesWatchers;
		public static EventLog EventLogger;
		public BinObjToRamLinkerService()
		{
			InitializeComponent();
			EventLogger = Logger = new System.Diagnostics.EventLog();
			if (!System.Diagnostics.EventLog.SourceExists("BinObjToRamSource")) {
				System.Diagnostics.EventLog.CreateEventSource(
					"BinObjToRamSource", "BinObjToRamLog");
			}
			EventLogger.Source = "BinObjToRamSource";
			EventLogger.Log = "BinObjToRamLog";
			_customerDirectoriesWatchers = new List<CustomerProjectDirectoryWatcher>();
		}

		/*private void OnBranchDirectoryDeleted(object sender, FileSystemEventArgs e)
		{
			if (Settings.Default.MustSkeep(Path.GetFileName(e.Name))) return;
			try
			{
				Directory.Delete(e.FullPath,true);
			}
			catch (System.Exception ex)
			{
				EventLogger.WriteEntry(ex.ToString());
			}				
		}*/

		protected override void OnStart(string[] args)
		{
			try {
				DirectoryInfo solutionDir = new DirectoryInfo(Settings.Default.SolutionsDirectory);
				if (solutionDir.Exists) {
					EventLogger.WriteEntry("Start find top direcory of "+ solutionDir.FullName);
					SolutionsDirectoryWatcher = new SolutionsDirectoryWatcher(solutionDir.FullName);
					var concreteCustomerFoldersProjects = solutionDir.GetDirectories("*", SearchOption.TopDirectoryOnly);
					SolutionsDirectoryWatcher.Created += OnBranchDirectoryCreated;

					if (!Settings.Default.WatchOnlyNewDirectories)
					{
						foreach (DirectoryInfo dir in concreteCustomerFoldersProjects)
							if (Settings.Default.MustSkeep(dir.Name)) continue;
							else
								AddNewCustormerBranchDirectory(dir.FullName);
					}

					SolutionsDirectoryWatcher.EnableRaisingEvents = true;
				}
				else
					EventLogger.WriteEntry("Canot find " + Settings.Default.SolutionsDirectory + " direcory");
			}
			catch (System.Exception ex) {
				EventLogger.WriteEntry(ex.ToString());
			}
		}

		private void OnBranchDirectoryCreated(object sender, FileSystemEventArgs e)
		{
			AddNewCustormerBranchDirectory(e.FullPath);
		}

		private void AddNewCustormerBranchDirectory(string directoryFullName)
		{
			_customerDirectoriesWatchers.Add(new CustomerProjectDirectoryWatcher(directoryFullName) { EnableRaisingEvents = true });
			EventLogger.WriteEntry("AddNewCustormerBranchDirectory watcher for dir " + directoryFullName);
		}

		protected override void OnPause()
		{
			try
			{
				base.OnPause();
				SolutionsDirectoryWatcher.EnableRaisingEvents = false;
				_customerDirectoriesWatchers.ForEach(w => w.EnableRaisingEvents = false);
			}
			catch (System.Exception ex)
			{
				EventLogger.WriteEntry(ex.ToString());
			}			
		}

		protected override void OnContinue()
		{
			base.OnContinue();
			try
			{
				SolutionsDirectoryWatcher.EnableRaisingEvents = true;
				_customerDirectoriesWatchers.ForEach(w => w.EnableRaisingEvents = true);
			}
			catch (System.Exception ex)
			{
				EventLogger.WriteEntry(ex.ToString());
			}			
		}

		protected override void OnStop()
		{
			try
			{
				SolutionsDirectoryWatcher.EnableRaisingEvents = false;
				SolutionsDirectoryWatcher.Dispose();
				_customerDirectoriesWatchers.ForEach(w => w.EnableRaisingEvents = false);
				_customerDirectoriesWatchers.Clear();
			}
			catch (System.Exception ex)
			{
				EventLogger.WriteEntry(ex.ToString());
			}			
		}
	}

	internal static class SettingsExtensions
	{
		private static List<string> itemsForExclude = Settings.Default.Exclude.Split(';').ToList();
		public static bool MustSkeep(this Settings appSettings, string fileName)
		{
			if (fileName.IndexOf("_tmp") >= 0)
				return true;
			return itemsForExclude.IndexOf(fileName) >= 0;
		}
	}
}
