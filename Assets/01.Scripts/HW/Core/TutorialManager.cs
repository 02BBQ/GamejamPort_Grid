using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    public List<Tutorial> tutorials = new();
    bool _isStart = false;
    int _currnetIndex = 0;
    public CinemachineVirtualCamera cam;
    private void Awake()
    {
        tutorials = GetComponents<Tutorial>().ToList();
    }

    private void Update()
    {
        if (tutorials[_currnetIndex] == null) return;
        if(!_isStart)
        {
            tutorials[_currnetIndex].OnStart();
            _isStart = true;
        }
        if (tutorials[_currnetIndex].Excute())
        {
            tutorials[_currnetIndex].OnEnd();
            _currnetIndex++;
            _isStart = false;
        }
    }
}
