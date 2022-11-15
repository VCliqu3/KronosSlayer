using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraTest : MonoBehaviour
{
    public Transform KKBossFight;
    public Transform playerCamFollowPoint;
    private CameraController _cameraController;
    // Start is called before the first frame update
    void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            _cameraController.CallCameraTranslationPosition(KKBossFight.position, 6, 2);
        }

        if (Input.GetKeyDown("j"))
        {
            _cameraController.CallCameraTranslationTarget(playerCamFollowPoint, 5, 2);
        }

        if (Input.GetKeyDown("n"))
        {
            CameraShaker.Instance.ShakeOnce(1f, 3f, 0.1f, 1f);
        }
    }
}
