using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float delayTime;
    private void OnEnable()
    {
        StartCoroutine(IEDestroy());   
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator IEDestroy()
    {
        yield return new WaitForSeconds(delayTime);
        PoolManager.Release(gameObject);
    }
}
