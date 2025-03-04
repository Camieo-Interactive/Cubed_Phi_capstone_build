using UnityEngine;
public class SlimeEnemy : EnemyBase {
    private void Update() => Move();
    protected override void OnDeath()
    {
        base.OnDeath();
        int rand = Random.Range(1, 4);
        for (int i = 0; i < rand; i++) PoolManager.Instance.GetObject(stats.spawnable);
    }
}