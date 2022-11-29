using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    public Transform bossFightPoint;
    public Transform playerCamFollowPoint;
    private CameraController _cameraController;

    public GameObject Boss;

    public float timeToStartBattle;
    public float timeToActivateBoss;

    // Start is called before the first frame update
    void Start()
    {
        _cameraController = GetComponent<CameraController>();

        StartCoroutine(StartBossBattle());
    }

    IEnumerator StartBossBattle()
    {
        yield return new WaitForSeconds(timeToStartBattle);

        _cameraController.CallCameraTranslationPosition(bossFightPoint.position, 6, 2);

        yield return new WaitForSeconds(timeToActivateBoss);

        Boss.SetActive(true);
    }

    public void EndBossBattle()
    {
        _cameraController.CallCameraTranslationTarget(playerCamFollowPoint, 5, 2);
    }
}
