using BOToRamLinker.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOToRamLinker
{
	internal class RamPathHelper
	{
		internal static RamPathHelper PathHelper = new RamPathHelper();
		internal static string MakePathToLinkOnObjFolder(string pathToProjFile)
		{
			return Path.Combine(GetPathWithOutFileName(pathToProjFile), "obj");
		}

		internal static string MakePathToLinkOnBinFolder(string pathToProjFile)
		{
			return Path.Combine(GetPathWithOutFileName(pathToProjFile), "bin");
		}

		internal static string MakeRamPathToObjFolder(string pathToProjFile)
		{
			return Path.Combine(MakeRamPathEquivalent(GetPathWithOutFileName(pathToProjFile)), "obj");
		}

		internal static string MakeRamPathToBinFolder(string pathToProjFile)
		{
			return Path.Combine(MakeRamPathEquivalent(GetPathWithOutFileName(pathToProjFile)), "bin");
		}

		internal static string RamPathToProjectFolder(string pathToProjFile)
		{
			return MakeRamPathEquivalent(GetPathWithOutFileName(pathToProjFile));
		}

		internal static string MakeRamPathEquivalent(string pathToProjFile)
		{
			return Settings.Default.RamDisk + pathToProjFile.Substring(2);
		}

		internal static string GetPathWithOutFileName(string pathToFile)
		{			
			return Path.GetDirectoryName(pathToFile);
		}
	}
}
