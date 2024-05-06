 using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Guinea.Core
{
public static class Logger
{
    #region LogWarning
    //-----------------------------------
    //--------------------- Log , warning, 

    [Conditional("GUINEA_ENABLE_LOG")]
    public static void Log(object message, 
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        UnityEngine.Debug.Log($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
    }
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogIf(bool condition, object message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (condition)
        {
            UnityEngine.Debug.Log($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
        }
    }
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogIfFormat(bool condition, string format, params object[] args)
    {
        if (condition)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }
    }
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogWarning(object message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        UnityEngine.Debug.LogWarning($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
    }

    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogWarning(object message, UnityEngine.Object context,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        UnityEngine.Debug.LogWarning($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}", context);
    }

    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }
    #endregion


    [Conditional("GUINEA_ENABLE_LOG")]
    public static void WarningUnless(bool condition, object message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!condition) UnityEngine.Debug.LogWarning($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
    }

    [Conditional("GUINEA_ENABLE_LOG")]
    public static void WarningUnless(bool condition, object message, UnityEngine.Object context, 
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!condition) UnityEngine.Debug.LogWarning($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}", context);
    }

    [Conditional("GUINEA_ENABLE_LOG")]
    public static void WarningUnlessFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
    {
        if (!condition) UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

    #region Assert
    //---------------------------------------------
    //------------- Assert ------------------------

    /// Throw an exception if condition = false
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new UnityEngine.UnityException();
    }

    /// Throw an exception if condition = false, show message on console's log
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void Assert(bool condition, string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        UnityEngine.Debug.Assert(condition, $"Assert {sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
    }

    /// Throw an exception if condition = false, show message on console's log
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, format, args);
    }
    #endregion

    #region Error
    //---------------------------------------------
    //------------- Error ------------------------
    [Conditional("GUINEA_ENABLE_LOG")]
    public static void LogError(object message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        UnityEngine.Debug.LogError($"{sourceFilePath} {memberName}(Line {sourceLineNumber}): {message}");
    }
    #endregion
}
}