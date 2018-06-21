using Model.Entity;

namespace BAL.Interfaces
{
    public interface ISandboxManager
    {
        /// <summary>
        /// Returns object ExecutionResult contains success of compilation and string result
        /// </summary>
        /// <param name="code">String code including unit tests and user code</param>
        /// <returns></returns>
        ExecutionResult Execute(string code);
    }
}
