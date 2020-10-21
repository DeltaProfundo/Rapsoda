using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public float buttonDelay;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            StartGamePressed();
        }
    }

    public void StartGamePressed()
    {
        audioSource.clip = Data.instance.OkButtonPressed();
        audioSource.Play();
        LoadMainScene();
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(Data.instance.MainBuildIndex());
    }
}
