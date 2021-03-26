/* 
 * MoxLog Copyright (c) 2020 Paul Ping Kohler
 * 
 */

using System.Collections.Generic;
using UnityEngine;
using Category = LogCat;

public class Log
{
    static Log Instance { get; } = new Log();

    static HashSet<Category> active;
    static HashSet<Category> hidden;

    static Log() { }
    private Log() 
    {
        active = new HashSet<Category> { 
            Category.Debug 
        };
        hidden = new HashSet<Category>();
    }


    public static bool Hide(Category category)
    {
        if (active.Contains(category))
        {
            active.Remove(category);
        }

        if (hidden.Contains(category))
        {
            return true;
        }

        hidden.Add(category);
        return hidden.Contains(category);
    }

    public static bool Show(Category category)
    {
        if (hidden.Contains(category))
        {
            hidden.Remove(category);
        }

        if (active.Contains(category))
        {
            return true;
        }

        active.Add(category);
        
        return active.Contains(category);
    }

    public static void Error(object message, params object[] args)
    {
        if (args.Length <= 0)
        {
            Debug.LogError(message);
            return;
        }

        if (message is string)
        {
            Debug.LogErrorFormat(message as string, args);
            return;
        }
    }

    public static bool Info(Category category, object message, params object[] args)
    {
#if !RELEASE_BUILD
        // Info is opt-in
        if (!active.Contains(category))
        {
            return false;
        }

        if (args.Length <= 0)
        {
            Debug.Log(message);
            return true;
        }

        if (message is string)
        {
            Debug.LogFormat(message as string, args);
            return true;
        }

#endif
        return false;
    }

    public static void Warning(Category category, object message, params object[] args)
    {
#if RELEASE_BUILD
        return;
#else
        // Warnings are opt-out
        if (hidden.Contains(category))
        {
            return;
        }

        if (args.Length <= 0)
        {
            Debug.LogWarning(message);
            return;
        }

        if (message is string)
        {
            Debug.LogWarningFormat(message as string, args);
            return;
        }
#endif
    }

    /// <summary>
    /// For temporary output.
    /// Please find all references and remove before moving on.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Temp(object message, params object[] args)
    {
#if RELEASE_BUILD
        return false;
#else
        if (args.Length <= 0)
        {
            Debug.Log(message);
            return;
        }

        if (message is string)
        {
            Debug.LogFormat(message as string, args);
            return;
        }
#endif
    }
}
