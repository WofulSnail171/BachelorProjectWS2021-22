using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover _instance;

    public Camera cam;
    public Vector3 mousePosition;
    public Transform test;

    Vector3 targetPosition;
    Vector3 startPosition;
    Vector3 currentPosition;
    float marginDistance;

    float currentZoom;
    public float targetZoom;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    float lerpTime = 1f;
    float currentLerpTime;

    float moveSpeed = 5.0f;
    float zoomSpeed = 10.0f;

    float amplitude = .15f;
    float period = 5f;

    void Start()
    {
        currentPosition = transform.position;
        SetTargetPos(transform.position);
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
            return;
        }

        if (cam == null)
            cam = gameObject.GetComponent<Camera>();
        currentZoom = cam.orthographicSize;
        targetZoom = currentZoom;
    }

    void Update()
    {
        mousePosition = CameraMover._instance.cam.ScreenToWorldPoint(Input.mousePosition);
        DoLerp();
        //Breath();
        if(currentZoom != targetZoom)
        {
            DoZoom();
        }
    }

    void Breath()
    {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        transform.position = startPosition + Vector3.up * distance;
    }

    void DoZoom()
    {
        if(targetZoom - currentZoom > 0)
        {
            currentZoom += zoomSpeed * Time.deltaTime;
            if (targetZoom - currentZoom < 0)
                currentZoom = targetZoom;
        }
        else
        {
            currentZoom -= zoomSpeed * Time.deltaTime;
            if (targetZoom - currentZoom > 0)
                currentZoom = targetZoom;
        }
        cam.orthographicSize = currentZoom;
    }

    void SetZoom(float _targetZoom)
    {
        if (_targetZoom >= maxZoom)
            targetZoom = maxZoom;
        else if (_targetZoom <= minZoom)
            targetZoom = minZoom;
        else
            targetZoom = _targetZoom;
    }

    void DoLerp()
    {
        if (lerpTime == 0)
            return;
        currentLerpTime += Time.deltaTime;
        SetZoomPercent(.5f);
        if (currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;
            SetZoomPercent(0.4f);
        }
        float t = currentLerpTime / lerpTime;
        //t = t * t * t * (t * (6f * t - 15f) + 10f);
        t = t * t * (3f - 2f * t);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    public void SetTargetPos(Vector3 _targetPos)
    {
        //lerpTime abhängig von distance:
        _targetPos.z = startPosition.z;
        if(moveSpeed != 0)
            lerpTime = (_targetPos - transform.position).magnitude / moveSpeed;
        currentLerpTime = 0.0f;
        Debug.Log("t: " + lerpTime.ToString());

        targetPosition = new Vector3(_targetPos.x, _targetPos.y, transform.position.z);
        startPosition = transform.position;
    }

    public void SetZoomPercent(float zoomStep)
    {
        if (zoomStep > 1)
            zoomStep = 1;
        else if (zoomStep < 0)
            zoomStep = 0;
        float step = (maxZoom - minZoom);
        targetZoom = minZoom + (step * zoomStep);
    }
}
