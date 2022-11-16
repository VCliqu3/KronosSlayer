using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject _camera;
    private CinemachineVirtualCamera _CMVCam;
   
    // Start is called before the first frame update
    void Start()
    {
        _CMVCam = _camera.GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator CameraTranslationPosition(Vector2 pointToTranslate, float sizeToTranslate, float transitionTime)
    {

        float time = 0f;
        float distance = Vector2.Distance(_camera.transform.position, pointToTranslate);
        Vector2 direction = new Vector2(pointToTranslate.x - _camera.transform.position.x, pointToTranslate.y - _camera.transform.position.y).normalized;

        Vector2 originalPosCamera = _camera.transform.position;
        float originalSizeCamera = _CMVCam.m_Lens.OrthographicSize;

        _CMVCam.Follow = null;
        
        while (time < transitionTime)
        {
            Vector2 newPos = originalPosCamera + direction * distance / 2 * (1 - Mathf.Cos(Mathf.PI * time / transitionTime));
            _camera.transform.position = new Vector3 (newPos.x, newPos.y, _camera.transform.position.z);
            _CMVCam.m_Lens.OrthographicSize = originalSizeCamera + (sizeToTranslate-originalSizeCamera)/2 * (1 - Mathf.Cos(Mathf.PI * time / transitionTime));

            time += Time.deltaTime;

            yield return null;
        }

        _camera.transform.position = new Vector3 (pointToTranslate.x,pointToTranslate.y,_camera.transform.position.z);
        _CMVCam.m_Lens.OrthographicSize = sizeToTranslate;

    }

    public IEnumerator CameraTranslationTarget(Transform objectToTranslate, float sizeToTranslate, float transitionTime)
    {

        float time = 0f;
        float distance = Vector2.Distance(_camera.transform.position, objectToTranslate.position);
        Vector2 direction = new Vector2(objectToTranslate.position.x - _camera.transform.position.x, objectToTranslate.position.y - _camera.transform.position.y).normalized;

        Vector2 originalPosCamera = _camera.transform.position;
        float originalSizeCamera = _CMVCam.m_Lens.OrthographicSize;

        _CMVCam.Follow = null;

        while (time < transitionTime)
        {
            Vector2 newPos = originalPosCamera + direction * distance / 2 * (1 - Mathf.Cos(Mathf.PI * time / transitionTime));
            _camera.transform.position = new Vector3(newPos.x, newPos.y, _camera.transform.position.z);
            _CMVCam.m_Lens.OrthographicSize = originalSizeCamera + (sizeToTranslate - originalSizeCamera) / 2 * (1 - Mathf.Cos(Mathf.PI * time / transitionTime));

            time += Time.deltaTime;

            yield return null;
        }

        _CMVCam.Follow = objectToTranslate;
    }

    public void CallCameraTranslationPosition(Vector2 pointToTranslate, float sizeToTranslate, float transitionTime)
    {
        StartCoroutine(CameraTranslationPosition(pointToTranslate, sizeToTranslate, transitionTime));
    }

    public void CallCameraTranslationTarget(Transform objectToTranslate, float sizeToTranslate, float transitionTime)
    {
        StartCoroutine(CameraTranslationTarget(objectToTranslate, sizeToTranslate, transitionTime));
    }

    public void FollowTarget(Transform target)
    {
        _CMVCam.Follow = target;
    }
}
