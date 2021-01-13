using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BOToRamLinker.BinObjToRamLinkerService;
using static BOToRamLinker.RamPathHelper;

namespace BOToRamLinker
{
	internal class SolutionsDirectoryWatcher : FileSystemWatcher
	{
		public SolutionsDirectoryWatcher(string pathToSolutionsDirectory) : base(pathToSolutionsDirectory, "*.*")
		{			
			this.NotifyFilter = NotifyFilters.DirectoryName;
			this.Created += OnNewSolutionsDirectoryCreated;
			this.Deleted += OnSolutionDirectoryDeleted;		
		}

		private void OnSolutionDirectoryDeleted(object sender, FileSystemEventArgs e)
		{
			EventLogger.WriteEntry("directory deleted " + e.FullPath);
			var ramPath = MakeRamPathEquivalent(e.FullPath);
			if (Directory.Exists(ramPath))
				try
				{
					Directory.Delete(ramPath, true);
				}
				catch (System.Exception ex)
				{
					EventLogger.WriteEntry("Delete folder " + ramPath + "exception " + ex.ToString());
				}
		}

		private void OnNewSolutionsDirectoryCreated(object sender, FileSystemEventArgs e)
		{
			EventLogger.WriteEntry("New directory created " + e.FullPath);
			var ramPath = MakeRamPathEquivalent(e.FullPath);
			if (Directory.Exists(ramPath) == false)
			{
				try
				{
					Directory.CreateDirectory(ramPath);
				}
				catch (System.Exception ex)
				{
					EventLogger.WriteEntry("Create folder " + ramPath + "exception " + ex.ToString());	
				}				
			}
			
		}
	}
}
