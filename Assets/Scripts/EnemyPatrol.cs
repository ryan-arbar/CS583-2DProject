using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float rotationSpeed;

    private Vector3 currentTarget;
    private float lerpTime = 0f;

    void Start()
    {
        targetPoint = 0;
        SetNewTarget(patrolPoints[targetPoint].position);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            increaseTargetInt();
            SetNewTarget(patrolPoints[targetPoint].position);
        }

        // Handle rotation
        Vector2 direction = currentTarget - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Handle movement
        lerpTime += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(transform.position, currentTarget, lerpTime);
    }

    void SetNewTarget(Vector3 newTarget)
    {
        currentTarget = newTarget;
        lerpTime = 0f;
    }

    void increaseTargetInt()
    {
        targetPoint++;

        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
