using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapStory : MonoBehaviour {

    public GameObject StoryPanel;

    void Awake()
    {
        hideStory();
    }

    public void hideStory()
    {
        StoryPanel.SetActive(false);
    }

    public void toggleStory()
    {
        if (!StoryPanel.active)
        {
            StoryPanel.SetActive(true);
        }
        else
        {
            StoryPanel.SetActive(false);
        }
    }

}
