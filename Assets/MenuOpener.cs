using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuOpener : MonoBehaviour
{
   [SerializeField] GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
