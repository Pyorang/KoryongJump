using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneUIScript : MonoBehaviour
{
    public float minLoadTime = 1.0f;
    public string TargetScene;

    float startTime;
    float endTime;

    AsyncOperation nextScene;

    [SerializeField]
    Slider LoadBar;

    void Start()
    {
        startTime = Time.time;nextScene = SceneManager.LoadSceneAsync(TargetScene);
        nextScene.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(nextScene != null)
        {
            LoadBar.value = Mathf.Clamp01(nextScene.progress);
        }

        if(Time.time - startTime >=minLoadTime && nextScene.progress >= 0.9f)
        {
            nextScene.allowSceneActivation = true;
        }
    }
}
