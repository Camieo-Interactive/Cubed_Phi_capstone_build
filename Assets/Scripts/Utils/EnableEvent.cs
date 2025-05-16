using UnityEngine;

public class EnableEvent : MonoBehaviour
{
    public GameObject ObjectToEnable;
    public void EnableObject()
    {
        if (ObjectToEnable != null)
        {
            ObjectToEnable.SetActive(true);
        }
    }
    public void DisableObject()
    {
        if (ObjectToEnable != null)
        {
            ObjectToEnable.SetActive(false);
        }
    }
}
