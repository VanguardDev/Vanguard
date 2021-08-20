using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
public class DiscordController : MonoBehaviour
{
    // Start is called before the first frame update
    Discord.Discord dc;
    public string Detail, LargeImage, LargeText;
    void Start()
    {
        if (Application.isBatchMode) enabled = false;
      dc = new Discord.Discord(877848055880253501, (System.UInt64)Discord.CreateFlags.Default);
        Discord.ActivityManager activityManager = dc.GetActivityManager();
        Discord.Activity activity = new Discord.Activity
        {
            Details = Detail,
            Assets =
            {
                LargeImage=LargeImage,
                LargeText=LargeText
            }
            
        };
        activityManager.UpdateActivity(activity,(res)=> {
            if (res == Discord.Result.Ok) Debug.Log("yey");
            else Debug.Log("no");
        
        });
        dc.SetLogHook(Discord.LogLevel.Debug, LogProblemsFunction);
    }
    public void LogProblemsFunction(Discord.LogLevel level, string message)
    {
        Debug.Log("Discord:{0} - {1}"+ level+ message);
    }

    void Update()
    {
        dc.RunCallbacks();
    }
    public void OnDestroy()
    {
        dc.Dispose();
    }
}
