public static class TipsText
{   
    //  ------------------ Public ------------------
    /// <summary>
    /// Array of tip texts to be displayed in the game.
    /// </summary>
    /// <remarks>
    /// This array contains various tips that can be shown to the player.
    /// Each tip provides helpful information or strategies for gameplay.
    /// </remarks>
    public static string[] TipText = {
        "Tip: You can build walls to block projectiles.",
        "Tip: Always start with Bit generators.",
        "Tip: Worms are really weak against long range units.",
        "Tip: You can build towers to sell later.",
        "Tip: Rerolling costs change based on the number of rerolls.",
        "Tip: You can read the patch notes on the itch.io page.",
        "Tip: There is a max number of enemies on the screen at once.",
        "Tip: Single use cards are a good way to delay the game.",
        "Tip: Sometimes, you just need to take a break.",
        "Tip: You should put your units in the middle of the board.",
        "Tip: Always leave a space for Bit collectors.",
        "Tip: Bit burners are invincible.",
        "Tip: Always leave space for single use cards.",
        "Tip: Landmines are a good way to deal with fast enemies.",
        "Tip: Grenades deal higher damage than landmines.",
        "Tip: Each cycle has a mechanic learn them well.",
        "Tip: Ghosts go fast, and phase through towers!",
        "Tip: Franken Enemys then gain speed the lower their health is!",
        "Tip: When the Spawn flips, its best to not have towers dedicated to one side.",
        "Tip: Bit generators should have a small space in the middle of groups for collectors.",
        "Tip: When on rail levels, its best to use turrets or high powered towers in the rail car.",
        "Tip: Fake cards are annoying, But there is a max amount of fakes you can have.",
        "Tip: Use Landmines as a way to delay enemies.",
        "Tip: Worms counter walls, walls counter melee enemies",
        "Tip: In the wack-a-enemy levels, its best to target the furthest along!",
        "Tip: Some gunner enemies rotate 360",
        "Tip: Use Bit stealers to collect bits"
    };
    public static string GetTip() => TipText[UnityEngine.Random.Range(0, TipText.Length)];
}
