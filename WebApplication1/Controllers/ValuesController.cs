using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/values/validate")]
        public bool Post([FromBody]string code)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(code);

            var systemRefLocation = typeof(object).GetTypeInfo().Assembly.Location;
            var systemReference = MetadataReference.CreateFromFile(systemRefLocation);
            var nunitReference = MetadataReference.CreateFromFile(typeof(TestAttribute).GetTypeInfo().Assembly.Location);

            string fileName = "OnlineExam.dll";
            string pathTodll = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["PathTo_dll"], fileName);

            var compilation = CSharpCompilation
                .Create(fileName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, checkOverflow: true))
                .AddReferences(systemReference, nunitReference)
                .AddSyntaxTrees(tree);

            EmitResult compilationResult = compilation.Emit(pathTodll);
            return compilationResult.Success;
        }
    }
}
