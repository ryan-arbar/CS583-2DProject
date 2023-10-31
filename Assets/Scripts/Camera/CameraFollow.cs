using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public bool followBoth = false;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = .25f;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (followBoth && enemy != null)
        {
            Vector3 middlePoint = (player.position + enemy.position) / 2f;
            float distance = Vector3.Distance(player.position, enemy.position);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, distance, smoothTime);

            Vector3 targetPosition = middlePoint + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1.7f, smoothTime); // 5f is the default size, change as needed

            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
