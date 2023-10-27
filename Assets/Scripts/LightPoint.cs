using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D light2D;
    public float smoothTime = 0.1f;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = light2D.transform.rotation; // Initialize target rotation
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");



        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90;

        if (horizontal != 0 || vertical != 0)
        {
            targetRotation = Quaternion.Euler(0, 0, angle);

            light2D.transform.rotation = Quaternion.Lerp(light2D.transform.rotation, targetRotation, smoothTime);
        }
    }
}
