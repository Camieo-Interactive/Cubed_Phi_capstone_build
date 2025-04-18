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

    #if UNITY_STANDALONE
    private Discord.Discord _discord;
    #endif
    private long _time;

    // Unity lifecycle method
    public override void PostAwake() { }

#if UNITY_STANDALONE

    /// <summary>
    /// Cleans up and disposes the Discord instance safely.
    /// </summary>
    private void DisposeDiscord()
    {
        try
        {
            _discord?.Dispose();
        }
        catch
        {
            // Ignore any exceptions during dispose
        }

        _discord = null;
    }
    private void Start()
    {
        try
        {
            _discord = new Discord.Discord(applicationID, (System.UInt64)CreateFlags.NoRequireDiscord);
            _time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            state = SplashText.GetCurrentSplash();
        }
        catch
        {
            DisposeDiscord();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        try
        {
            _discord.RunCallbacks();
        }
        catch
        {
            DisposeDiscord();
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
        if (_discord == null)
            return;

        try
        {
            ActivityManager activityManager = _discord.GetActivityManager();
            Activity activity = new Activity
            {
                Details = details,
                State = state,

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

            activityManager.UpdateActivity(activity, result =>
            {
                if (result != Result.Ok)
                    Debug.LogWarning("Failed to update Discord status.");
            });
        }
        catch
        {
            DisposeDiscord();
            Destroy(gameObject);
        }
    }
#endif
}
