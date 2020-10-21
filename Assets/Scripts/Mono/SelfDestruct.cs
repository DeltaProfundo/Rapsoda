using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float delay;

    private void Start()
    {
        Invoke("SelfDestroy", delay);
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
