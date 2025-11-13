using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : MonoBehaviour
{
    [Header("TowerLaser")]
    public Transform laserOrigin;
    public Transform laserTarget;
    public LineRenderer lineRenderer;
    public float laserWidth;
    public LayerMask playerLayer;

    [Header("Effect")]
    public float detectionDelay;
    public ParticleSystem smokeParticleSystem;
    public GameObject bridgeObject;
    public float bridgeAppearTime;

    [Header("ScreenVibration")]
    public Camera camera;
    public float shakeDuration;
    public float shakeMagnitude;

    public PlayerController controller;

    private bool trapActivated = false;
    private bool playerDetected = false;

    private Vector3 offsetOrigin;
    private Vector3 offsetTarget;
    public float yOffset = 1f;

    private void Start()
    {
        offsetOrigin = laserOrigin.position;
        offsetTarget = laserTarget.position;

        offsetOrigin.y += yOffset;
        offsetTarget.y += yOffset;

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, offsetOrigin);
        lineRenderer.SetPosition(1, offsetTarget);

        if (bridgeObject != null)
        {
            bridgeObject.SetActive(false);
        }

        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.gameObject.SetActive(false);
        }

        if (camera == null)
        {
            camera = Camera.main;
        }

        if (controller != null)
        {
            camera = controller.GetCurCamera();
            controller.OnActiveCameraChanged += UpdateCamera;
        }
        else
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }
    }

    private void OnDisable()
    {
        if (controller != null)
        {
            controller.OnActiveCameraChanged -= UpdateCamera;
        }
    }
    private void Update()
    {
        if (trapActivated) return;

        RaycastHit hit;

        if (Physics.Linecast(offsetOrigin, offsetTarget, out hit, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if(!playerDetected)
                {
                    playerDetected = true;
                    StartCoroutine(ActivateTrap());
                }
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
            }
        }

        Debug.DrawLine(offsetOrigin, offsetTarget, Color.red);
    }

    IEnumerator ActivateTrap()
    {
        trapActivated = true;

        yield return new WaitForSeconds(detectionDelay);

        if (camera != null)
        {
            StartCoroutine(ShakeCamera());
        }

        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.gameObject.SetActive(true); // È°¼ºÈ­
            smokeParticleSystem.Play();

            StartCoroutine(SmokeDelay(smokeParticleSystem.main.duration + smokeParticleSystem.main.startLifetime.constantMax));
        }

        if(bridgeObject != null)
        {
            bridgeObject.SetActive(true);
        }

        lineRenderer.enabled = false;
    }

    IEnumerator SmokeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (smokeParticleSystem != null)
        {
            smokeParticleSystem.gameObject.SetActive(false);
            smokeParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }


    IEnumerator ShakeCamera()
    {
        if (camera == null) yield break;

        Vector3 originalPosition = camera.transform.localPosition;
        float elapsed = 0f;

        while(elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            camera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.transform.localPosition = originalPosition;
    }

    private void UpdateCamera(Camera camera)
    {
        this.camera = camera;
    }
}
