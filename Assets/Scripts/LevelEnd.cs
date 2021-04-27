using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelEnd : MonoBehaviour
{
    [SerializeField] private bool loadNext = true;
    [SerializeField] private string levelName = "do not use this yet";
    [SerializeField] private int levelIndex = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (loadNext) LevelManager.Instance().LoadNextScene();
            else LevelManager.Instance().LoadScene(levelIndex);
        }
    }
}
