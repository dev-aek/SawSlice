using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextButton;
    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        LevelManager.Instance.RestartLevel();
        //LevelManager.Instance.ui.UpdateLevelText();
        nextButton.gameObject.active = false;

    }

    public void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
        restartButton.gameObject.active = false;

    }
}
