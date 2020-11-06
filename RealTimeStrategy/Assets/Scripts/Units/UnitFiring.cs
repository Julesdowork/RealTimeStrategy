using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePf = null;
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();
        
        if (target == null) { return; }

        if (!CanFireAtTarget()) { return; }

        Quaternion targetRot = Quaternion.LookRotation(
            target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        if (Time.time > (1f / fireRate) + lastFireTime)
        {
            Quaternion projectileRot = Quaternion.LookRotation(
                target.GetAimAtPoint().position - projectileSpawnPoint.position);
            GameObject projectile = Instantiate(
                projectilePf, projectileSpawnPoint.position, projectileRot);

            NetworkServer.Spawn(projectile, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude
            <= fireRange * fireRange;
    }
}
