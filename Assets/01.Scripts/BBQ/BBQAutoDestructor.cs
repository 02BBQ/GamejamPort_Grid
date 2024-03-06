using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBQAutoDestructor : MonoBehaviour
{
    public float Lifetime = 5f;
    private void OnEnable()
    {
        if (gameObject.GetComponent<ParticleSystem>())
            gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine("Destruct");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator Destruct()
    {
        yield return new WaitForSeconds(Lifetime);
        PoolManager.Release(gameObject);
    }
}
