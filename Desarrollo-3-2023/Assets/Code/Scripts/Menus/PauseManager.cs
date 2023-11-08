using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kuznickiEventChannel;

public class PauseManager : MonoBehaviourSingleton<PauseManager>
{
    [SerializeField] private EventChannelSO channel;
    [SerializeField] private GameObject pauseText;
    private bool isPaused = false;

    private void Start()
    {
        channel.Subscribe(OnChannelHandler);
    }

    private void OnChannelHandler()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseText.SetActive(true);
            Debug.Log("Pause");
            Time.timeScale = 0;
        }
        else
        {
            pauseText.SetActive(false);
            Debug.Log("Unpaused");
            Time.timeScale = 1;
        }
    }
}
