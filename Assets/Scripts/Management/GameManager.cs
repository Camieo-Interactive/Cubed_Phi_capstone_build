using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public int BitsCollected = 0; 
    public override void PostAwake() {}
}