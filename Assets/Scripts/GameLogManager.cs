using UnityEngine;

public static class GameLogManager
{
    private static bool showLogs = true;

    public static void CustomLog(object message)
    {
        if (!showLogs )
        {
            return;
        }

        Debug.Log(message);
    }
}
