using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BinObjToRamLinkerConsole
{
	class Program
	{
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
		static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, uint dwFlags);
		static string solutionPath = @"D:\Programming\MeterShop\BCHydro\";
		static string ramDiskPath = @"R:";
		const uint SYMBLOC_LINK_FLAG_FILE = 0x0;
		const uint SYMBLOC_LINK_FLAG_DIRECTORY = 0x1;
		static void Main(string[] args)
		{
			DirectoryInfo solutionDir = new DirectoryInfo(solutionPath);
			if (solutionDir.Exists) {				
				var projectFiles = solutionDir.GetFiles("*.*proj", SearchOption.AllDirectories);
				foreach (var projectFile in projectFiles)
				{
					string sourceObjDirPath = Path.Combine(projectFile.DirectoryName + "\\obj");
					string sourceBinDirPath = Path.Combine(projectFile.DirectoryName + "\\bin");
					string targetObjDirPath = Path.Combine(ramDiskPath + projectFile.DirectoryName.Substring(2) + "\\obj");
					string targetBinDirPath = Path.Combine(ramDiskPath + projectFile.DirectoryName.Substring(2) + "\\bin");

					if (Directory.Exists(sourceBinDirPath))
						Directory.Delete(sourceBinDirPath, true);										
					Directory.CreateDirectory(targetBinDirPath);

					if (Directory.Exists(sourceObjDirPath))
						Directory.Delete(sourceObjDirPath, true);
					
					Directory.CreateDirectory(targetObjDirPath);
					
					CreateSymbolicLink(sourceObjDirPath, targetObjDirPath, SYMBLOC_LINK_FLAG_DIRECTORY);
					CreateSymbolicLink(sourceBinDirPath, targetBinDirPath, SYMBLOC_LINK_FLAG_DIRECTORY);
				}
			}
		}
	}
}
