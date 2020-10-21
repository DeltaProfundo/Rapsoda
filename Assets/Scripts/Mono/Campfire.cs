using UnityEngine;

public class Campfire : MonoBehaviour
{
    public Light lighting;

    public float minLightIntensity;
    public float maxLightIntensity;
    public float lightIntensityChangeFreq;
    private float lightIntensityChangeTimer;

    private void Start()
    {
        lightIntensityChangeTimer = lightIntensityChangeFreq;
    }

    private void Update()
    {
        lightIntensityChangeTimer -= Time.deltaTime;
        if (lightIntensityChangeTimer <= 0)
        {
            lighting.intensity = Random.Range(minLightIntensity, maxLightIntensity);
            lightIntensityChangeTimer = lightIntensityChangeFreq;
        }
    }
}
