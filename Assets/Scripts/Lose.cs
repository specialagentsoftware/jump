using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        SceneManager.LoadScene("Lose");
    }
}
