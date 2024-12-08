using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ServiCuentas.Shared
{

    public static class DebugInfo
    {
        public static string GetCurrentMethod([CallerMemberName] string methodName = "")
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            var className = method.DeclaringType.Name;
            return $"{className}.{methodName}";
        }
    }


}
