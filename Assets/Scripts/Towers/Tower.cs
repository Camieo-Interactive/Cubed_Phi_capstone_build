using UnityEngine;

public abstract class Tower : MonoBehaviour, IDamageable
{
    public void ChangeHealth(int damageDelta)
    {
        throw new System.NotImplementedException();
    }
}