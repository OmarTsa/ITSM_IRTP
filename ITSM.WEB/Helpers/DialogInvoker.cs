using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ITSM.WEB.Helpers
{
    public static class DialogInvoker
    {
        // Safely invoke Close on a dialog instance provided by MudBlazor without a compile-time dependency.
        public static bool SafeClose(object? dialogInstance, object? data = null, ILogger? logger = null)
        {
            if (dialogInstance == null) return false;
            try
            {
                var type = dialogInstance.GetType();
                var closeMethod = type.GetMethod("Close", BindingFlags.Public | BindingFlags.Instance);
                if (closeMethod == null)
                {
                    // Try canonical name with different casing or signature
                    closeMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(m => string.Equals(m.Name, "Close", StringComparison.OrdinalIgnoreCase));
                }

                if (closeMethod == null)
                {
                    logger?.LogWarning("Dialog instance does not expose a Close method: {Type}", type.FullName);
                    return false;
                }

                var parameters = closeMethod.GetParameters();
                if (parameters.Length == 0)
                {
                    closeMethod.Invoke(dialogInstance, null);
                    return true;
                }

                // If Close expects a MudBlazor.DialogResult or similar, try to create an instance reflectively
                var paramType = parameters[0].ParameterType;

                object? arg = null;

                // Look for MudBlazor.DialogResult type in loaded assemblies
                var dialogResultType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => SafeGetTypes(a))
                    .FirstOrDefault(t => t != null && t.FullName == "MudBlazor.DialogResult");

                if (dialogResultType != null && paramType.IsAssignableFrom(dialogResultType))
                {
                    // Try to call static Ok method if available
                    var okMethod = dialogResultType.GetMethod("Ok", BindingFlags.Public | BindingFlags.Static);
                    if (okMethod != null)
                    {
                        arg = okMethod.Invoke(null, new object?[] { data });
                    }
                    else
                    {
                        // Try to create instance and set Data property if present
                        arg = Activator.CreateInstance(dialogResultType);
                        var dataProp = dialogResultType.GetProperty("Data", BindingFlags.Public | BindingFlags.Instance);
                        dataProp?.SetValue(arg, data);
                    }
                }
                else if (paramType == typeof(object))
                {
                    arg = data;
                }
                else
                {
                    // Try to construct the expected parameter type with a default or data property
                    try
                    {
                        arg = Activator.CreateInstance(paramType);
                        if (arg != null)
                        {
                            var dataProp = paramType.GetProperty("Data", BindingFlags.Public | BindingFlags.Instance);
                            dataProp?.SetValue(arg, data);
                        }
                    }
                    catch
                    {
                        // Can't create parameter instance; pass null
                        arg = null;
                    }
                }

                closeMethod.Invoke(dialogInstance, arg != null ? new[] { arg } : new object?[] { null });
                return true;
            }
            catch (TargetInvocationException tie)
            {
                logger?.LogError(tie, "Error invoking Close on dialog instance");
                return false;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Unexpected error invoking Close on dialog instance");
                return false;
            }
        }

        public static bool SafeCancel(object? dialogInstance, ILogger? logger = null)
        {
            if (dialogInstance == null) return false;
            try
            {
                var type = dialogInstance.GetType();
                var cancelMethod = type.GetMethod("Cancel", BindingFlags.Public | BindingFlags.Instance);
                if (cancelMethod == null)
                {
                    cancelMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(m => string.Equals(m.Name, "Cancel", StringComparison.OrdinalIgnoreCase));
                }

                if (cancelMethod == null)
                {
                    logger?.LogWarning("Dialog instance does not expose a Cancel method: {Type}", type.FullName);
                    return false;
                }

                cancelMethod.Invoke(dialogInstance, null);
                return true;
            }
            catch (TargetInvocationException tie)
            {
                logger?.LogError(tie, "Error invoking Cancel on dialog instance");
                return false;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Unexpected error invoking Cancel on dialog instance");
                return false;
            }
        }

        private static Type[] SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).ToArray()!;
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}
