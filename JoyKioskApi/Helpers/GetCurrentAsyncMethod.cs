using System.Reflection;
using System.Runtime.CompilerServices;

namespace JoyKioskApi.Helpers
{
    public static class GetCurrentAsyncMethod
    {
        public static string GetName(MethodBase? asyncMethod)
        {
            if (asyncMethod == null)
                return string.Empty;
            var generatedType = asyncMethod.DeclaringType;
            var originalType = generatedType!.DeclaringType;
            var matchingMethods =
                from methodInfo in originalType.GetMethods()
                let attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>()
                where attr != null && attr.StateMachineType == generatedType
                select methodInfo;

            // If this throws, the async method scanning failed.
            var foundMethod = matchingMethods.Single();
            return foundMethod.Name;
        }
    }
}
