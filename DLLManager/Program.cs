using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

namespace DLLManager
{
  class Program
  {
    static void Main(string[] args)
    {
      bool install = true;
      string dllFile = "";
      try
      {
        install = args[0] == "instalar";
        dllFile = args[1];
      }
      catch (Exception)
      {
        Console.WriteLine("Argument Error. Usage (instalar|desinstalar) dllfile");
      }

      try
      {
        string dllStore = getDllStore();
        if (install)
        {
          copyDLL(dllFile, dllStore);
        }
        registerDLL(install, dllFile, dllStore);
      }
      catch (Exception)
      {
        Console.WriteLine("Error, operation not completed.");
      }
    }

    private static string getDllStore()
    {
      string destination = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "System32");
      if (Environment.Is64BitOperatingSystem)
      {
        destination = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "SysWOW64");
      }
      return destination;
    }

    private static void copyDLL(string file, string dllStore)
    {      
      File.Copy(file, Path.Combine(dllStore, file), true);
    }

    private static void registerDLL(bool install, string file, string dllStore)
    {
      string regsvrPath = Path.Combine(dllStore, "regsvr32.exe");
      Process installDLL = new Process();
      installDLL.StartInfo.FileName = regsvrPath;
      installDLL.StartInfo.Arguments = file;      
      if (!install)
      {
        installDLL.StartInfo.Arguments = "-u " + file;
      }
      installDLL.Start();
      installDLL.WaitForExit();
    }
  }
}
