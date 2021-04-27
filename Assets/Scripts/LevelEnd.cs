using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelEnd : MonoBehaviour
{
    [SerializeField] private bool loadNext = true;
    [SerializeField] private string levelName = "no level";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (loadNext) LevelManager.Instance().LoadNextScene();
            else LevelManager.Instance().LoadScene(levelName);
        }
    }
}
