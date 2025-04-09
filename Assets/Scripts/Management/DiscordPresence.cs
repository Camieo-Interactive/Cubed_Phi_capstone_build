using UnityEngine;

// This code only works on desktop builds.
#if UNITY_STANDALONE
using Discord;
#endif

/// <summary>
/// Singleton that manages Discord presence for the game, updating the player's status on Discord.
/// </summary>
/// <remarks>
/// Backported from Cubed Arcade, this script manages Discord integration, setting up rich presence for the game.
/// </remarks>
public class DiscordPresence : SingletonBase<DiscordPresence>
{
    [Header("Discord")]
    [Space(10)]
    
    /// <summary>
    /// The application ID for the Discord application.
    /// </summary>
    [Tooltip("The Application ID for Discord.")]
    public long applicationID;
    
    /// <summary>
    /// The details text for Discord presence.
    /// </summary>
    [Tooltip("Details text shown in Discord.")]
    public string details = "Main Menu";

    /// <summary>
    /// The state text for Discord presence.
    /// </summary>
    [Tooltip("State text shown in Discord.")]
    public string state = "Snarky funny flavor text!";

    /// <summary>
    /// The large image key for Discord presence.
    /// </summary>
    [Tooltip("The large image displayed in Discord.")]
    public string largeImage = "game_logo";

    /// <summary>
    /// The large image text for Discord presence.
    /// </summary>
    [Tooltip("Text associated with the large image in Discord.")]
    public string largeText = "Cubed";
    
    private Discord.Discord _discord;
    private long _time;

    // Unity lifecycle method
    public override void PostAwake() { }

    #if UNITY_STANDALONE
    private void Start()
    {
        // Initialize Discord with the Application ID
        _discord = new Discord.Discord(applicationID, (System.UInt64)CreateFlags.NoRequireDiscord);
        _time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        details = SplashText.GetCurrentSplash();
    }

    private void Update()
    {
        // Ensure the Discord callbacks are processed each frame
        try
        {
            _discord.RunCallbacks();
        }
        catch
        {
            // If Discord isn't running, destroy the object
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        // Dispose of the Discord instance when the application quits
        _discord?.Dispose();
    }

    private void LateUpdate()
    {
        // Update the Discord status each frame
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        try
        {
            // Create and update the activity status on Discord
            ActivityManager activityManager = _discord.GetActivityManager();
            Activity activity = new Activity
            {
                Details = details,
                
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText,
                    SmallText = state,
                },
                Timestamps =
                {
                    Start = _time
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Result.Ok)
                    Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            // If updating the status fails, destroy the object
            Destroy(gameObject);
        }
    }
    #endif
}
