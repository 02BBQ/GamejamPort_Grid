using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void LineDraw(Vector2 basePos, Vector2 targetPos, float firT, float secT)
    {
        _lineRenderer.SetPosition(0, basePos);
        StartCoroutine(IELineDraw(basePos,targetPos, firT, secT));
    }

    IEnumerator IELineDraw(Vector2 basePos, Vector2 targetPos, float firT, float secT)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * firT;
            _lineRenderer.SetPosition(1, Vector3.Lerp(basePos, targetPos, percent));
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * secT;
            _lineRenderer.SetPosition(0, Vector3.Lerp(basePos, targetPos, percent));
            yield return null;
        }
        PoolManager.Release(gameObject);
    }
}
