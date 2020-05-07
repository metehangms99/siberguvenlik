using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    class Program
    {
        class NMAP : IDisposable
        {
            private ProcessStartInfo proccesStart = new ProcessStartInfo();
            private Process process = new Process();
            private string sonuc;
            private string script;
            public NMAP(string script){
                this.script = script;
                proccesStart.Arguments = "-p 80 --script " + this.script + " testphp.vulnweb.com -oX -";
                proccesStart.RedirectStandardOutput = true;
                proccesStart.FileName = "nmap";
                process.StartInfo = proccesStart;
            }

            public void Dispose() {
                process.Dispose();
            }

            private void TakeOutput(){
                if (string.IsNullOrEmpty(sonuc)) {
                    process.Start();
                    sonuc = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
            }
            public string StdOut {
                get{
                    TakeOutput();
                    return sonuc;
                }
            }
        }
        static void Main(String[] args){
            List<NMAP> nmaps = new List<NMAP>();
            nmaps.Add(new NMAP("http-sql-injection"));
            nmaps.Add(new NMAP("ssl-ccs-injection"));
            nmaps.Add(new NMAP("http-csrf"));
            //nmaps.ForEach(x => Console.WriteLine(x.StdOut));
            StreamWriter sw = new StreamWriter("result.xml");
            nmaps.ForEach(x => sw.WriteLine(x.StdOut));
            sw.Flush();
            sw.Close();
        }
    }
}
