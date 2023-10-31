using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolTeaser : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float rotationSpeed;

    private Vector3 currentTarget;
    private float lerpTime = 0f;
    private bool isActive = true;

    void Start()
    {
        targetPoint = 0;
        SetNewTarget(patrolPoints[targetPoint].position);
    }

    void Update()
    {
        if (!isActive) return; //exit the script once the teaser destroys itself

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            increaseTargetInt();
            if (!isActive) return; //exit if not active after increasing target
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
            Destroy(gameObject);
            isActive = false;
        }
    }
}
