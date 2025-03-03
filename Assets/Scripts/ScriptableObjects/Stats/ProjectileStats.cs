using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile stats", menuName = "Weapon/Projectile Stats")]
public class ProjectileStats : ScriptableObject
{
    //  ------------------ Public ------------------

    [Tooltip("How fast projectile is moving")]
    public float projectileSpeed;
    
    [Tooltip("How long is the projectile going to be live")]
    public int bulletLifeTime;

    [Tooltip("Does it spawn something on impact?")]
    public bool spawnOnHit;

    [Tooltip("How many things does it spawn on impact")]
    public int numberOfSpawnables;

    [Tooltip("What spawnable object?")]
    public GameObject spawnable;
}