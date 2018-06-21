using Model.Entity;
using BAL.Interfaces;
using RestSharp;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;

namespace BAL.Managers
{
    public class SandboxManager : ISandboxManager
    {
        private readonly string sandboxAPI;

        public SandboxManager(string sandboxAPI)
        {
            this.sandboxAPI = sandboxAPI;
        }

        public ExecutionResult Execute(string code)
        {
            var client = new RestClient(sandboxAPI);
            var request = new RestRequest("api/values/validate", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(code);
            IRestResponse response = client.Execute(request);
            bool result = false;
            if (response.Content == "true")
            {
                result = true;
            }
            else if (response.Content == "false")
            {
                result = false;
            }

            string fileName = "OnlineExam.dll";
            string resultFilename = "Result.xml";
            string pathTodll = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string pathToxml = Path.Combine(Directory.GetCurrentDirectory(), resultFilename);
            string NUnit = "NUnit.Console-3.8.0";

            var executeResult = new ExecutionResult();
            if (result)
            {
                string strCmdText = $"NUNIT3-CONSOLE {pathTodll} --result={pathToxml}";
                var p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = true;
                p.Start();
                p.StandardInput.WriteLine($"cd {Path.Combine(Directory.GetCurrentDirectory(), NUnit)}");
                p.StandardInput.WriteLine(strCmdText);
                Thread.Sleep(2500);
                var xmlResult = XDocument.Load(pathToxml);
                var tests = xmlResult.Document.Element("test-run");
                var total = string.Concat("Total:", (string)tests.Attribute("total"));
                var passed = string.Concat("Passed:", (string)tests.Attribute("passed"));
                var failed = string.Concat("Failed:", (string)tests.Attribute("failed"));
                var duration = string.Concat("Duration:", (string)tests.Attribute("duration"));
                executeResult.Success = true;
                executeResult.Result = string.Join("; ", total, passed, failed, duration);
            }
            else
            {
                executeResult.Success = false;
                executeResult.Result = "Compile errors";
            }
            return executeResult;
        }
    }
}
