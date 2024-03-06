using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDead : MonoSingleton<PlayerDead>
{
    public bool isDead = false;
    public GameObject playerDeadParticle;
    public void OnDead()
    {
        isDead = true;
        GridManager.Instance.isChanging = true;
        foreach (Enemy e in EnemySpawner.Instance.enemyList)
        {
            e.Dead();
        }
        EnemySpawner.Instance.StopSpawn();
        StartCoroutine(IEDead());
    }

    IEnumerator IEDead()
    {
        Transform playerTrm = Player.Instance.transform;
        SoundManager.Instance.BGMVolumeDown(0, true, 2f);
        CameraManager.Instance.MoveCamera(playerTrm);
        CameraManager.Instance.ZoomCamera(3, 1f);
        yield return new WaitForSeconds(2f);
        playerTrm.DOShakePosition(0.2f, 0.15f, 100, 90, false, true);
        yield return new WaitForSeconds(1.2f);
        playerTrm.DOShakePosition(0.3f, 0.15f, 100, 90, false, true);
        yield return new WaitForSeconds(1f);
        playerTrm.DOShakePosition(0.4f, 0.2f, 120, 90, false, false).OnComplete(() => 
        {
            Instantiate(playerDeadParticle, playerTrm.position, Quaternion.identity);
            Destroy(playerTrm.gameObject);
        });
        yield return new WaitForSeconds(1.5f);
        CameraManager.Instance.ZoomCamera(400, 2f, Ease.InCubic);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ResultScene");
    }
}
