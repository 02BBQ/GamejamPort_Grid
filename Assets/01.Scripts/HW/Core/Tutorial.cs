using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    protected TextMeshProUGUI _tutorialText;

    private void Awake()
    {
        _tutorialText = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();
    }
    public abstract void OnStart();
    public abstract bool Excute();
    public abstract void OnEnd();
}
