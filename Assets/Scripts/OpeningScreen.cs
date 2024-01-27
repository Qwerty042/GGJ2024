using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScreen : MonoBehaviour
{
    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Load the main scene
            SceneManager.LoadScene("SampleScene");
        }
    }
}
