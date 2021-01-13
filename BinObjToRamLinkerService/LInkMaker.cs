using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BOToRamLinker
{
	static class LinkMaker
	{
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
		static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, uint dwFlags);		
		const uint SYMBLOC_LINK_FLAG_FILE = 0x0;
		const uint SYMBLOC_LINK_FLAG_DIRECTORY = 0x1;
		static uint SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x2;

		internal static bool CreateSoftLinkToFolder(string linkPath, string targetFolder)
		{
			try 
			{
				if (Directory.Exists(linkPath))
					Directory.Delete(linkPath, true);
				if (!Directory.Exists(targetFolder))
					Directory.CreateDirectory(targetFolder);
			}
			catch { return false; }

			return CreateSymbolicLink(linkPath, targetFolder, (SYMBLOC_LINK_FLAG_DIRECTORY | SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE));
		}

		/*internal static string RemoveFolderCreatedForProjFile(string projectFilePath)
		{
			try 
			{ 
				var dirpath = ramDisk + Path.GetDirectoryName(projectFilePath).Substring(2);
				if (Directory.Exists(dirpath)) {
					Directory.Delete(dirpath, true);
					return dirpath + "deleted";
				}
			}catch(Exception e)
			{
				return e.Message;
			}
			return null;
		}*/
	}
}
