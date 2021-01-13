using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOPath = System.IO.Path;
using static BOToRamLinker.BinObjToRamLinkerService;
using static BOToRamLinker.RamPathHelper;
using BOToRamLinker.Properties;
using System.Threading;

namespace BOToRamLinker
{
	internal class CustomerProjectDirectoryWatcher : FileSystemWatcher
	{
		private Timer _selfDestroy;
		public CustomerProjectDirectoryWatcher(string projecDirectoryPath) : base(projecDirectoryPath, "*.*")
		{			
			this.IncludeSubdirectories = true;
			this.NotifyFilter = NotifyFilters.FileName;
			this.Created += OnProjFileCreated;
			this.Deleted += OnProjFileDeleted;
			this.InternalBufferSize = 65536;
			_selfDestroy = new Timer(OnTimerElapsed);
			_selfDestroy.Change(10000, 10000);
		}

		private void OnTimerElapsed(object state)
		{
			this.EnableRaisingEvents = false;
			_selfDestroy.Dispose();
			EventLogger.WriteEntry("Watcher stopped for " + this.Path);
		}

		private void OnProjFileDeleted(object sender, FileSystemEventArgs e)
		{
			/*if (Settings.Default.MustSkeep(e.Name)) return;
			var ramDirectory = new DirectoryInfo(RamPathToProjectFolder(e.FullPath));			
			if (ramDirectory.Exists) {
				ramDirectory.Delete(true);
				EventLogger.WriteEntry(ramDirectory + "deleted");
			}*/			
		}

		private void OnProjFileCreated(object sender, FileSystemEventArgs e)
		{
			if (Settings.Default.MustSkeep(e.Name)) return;
			if (IOPath.GetExtension(e.Name).IndexOf("proj") == -1)
			{				
				_selfDestroy.Change(10000, 10000);
				return;
			}

			var pathToObjLink = MakePathToLinkOnObjFolder(e.FullPath);
			var pathToObjFolder = MakeRamPathToObjFolder(e.FullPath);
			if (!Directory.Exists(pathToObjFolder))
			{
				var success = LinkMaker.CreateSoftLinkToFolder(pathToObjLink, pathToObjFolder) != false;
				if (!success)
					EventLogger.WriteEntry("Cannot create obj link for file " + e.FullPath);
				else
					EventLogger.WriteEntry(pathToObjLink + " <===> " + pathToObjFolder);
			}

			var pathToBinLink = MakePathToLinkOnBinFolder(e.FullPath);
			var pathToBinFolder = MakeRamPathToBinFolder(e.FullPath);
			if (!Directory.Exists(pathToBinFolder))
			{				
				var success = LinkMaker.CreateSoftLinkToFolder(pathToBinLink, pathToBinFolder) != false;
				if (!success)
					EventLogger.WriteEntry("Cannot create bin link for file " + e.FullPath);
				else
					EventLogger.WriteEntry(pathToBinLink + " <===> " + pathToBinFolder);
			}
		}

		
	}
}
