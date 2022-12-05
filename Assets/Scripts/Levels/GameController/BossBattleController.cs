using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    public Transform bossFightPoint;
    public Transform playerCamFollowPoint;
    private CameraController _cameraController;
    public Animator BossDoorAnimator;

    public GameObject Boss;

    public float timeToStartBattle;
    public float timeToActivateBoss;
    public float timeToOpenDoorAfterBossDeath = 1f;
    public float timeToReturnCamToPlayer = 4f;

    //SFX

    public string nameSFXdoorOpen;

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

    public IEnumerator EndBossBattle()
    {
        yield return new WaitForSeconds(timeToOpenDoorAfterBossDeath);

        AudioManager.instance.PlaySFX(nameSFXdoorOpen);
        BossDoorAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(timeToReturnCamToPlayer);

        _cameraController.CallCameraTranslationTarget(playerCamFollowPoint, 5, 2);
        //_cameraController.FollowTarget(playerCamFollowPoint);
    }

}
