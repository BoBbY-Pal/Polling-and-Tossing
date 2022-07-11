using UnityEngine;

public static class GameLogManager
{
    private static bool showLogs = false;

    public static void CustomLog(object message)
    {
        if (!showLogs )
        {
            return;
        }

        Debug.Log(message);
    }
}
