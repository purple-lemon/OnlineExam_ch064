using System.Collections.Generic;

namespace Model.Entity
{
    public class CompileResult
    {
        public bool Success { get; set; }
        public List<string> CompileErrors { get; set; }

        public CompileResult()
        {
            CompileErrors = new List<string>();
        }
    }
}
