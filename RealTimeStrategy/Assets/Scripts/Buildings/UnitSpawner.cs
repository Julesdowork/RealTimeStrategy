﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject unitPf = null;
    [SerializeField] private Transform unitSpawnPoint = null;

	#region Server

	public override void OnStartServer()
	{
		health.ServerOnDie += ServerHandleDie;
	}

	public override void OnStopServer()
	{
		health.ServerOnDie -= ServerHandleDie;
	}

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unit = Instantiate(unitPf, unitSpawnPoint.position, unitSpawnPoint.rotation);

        NetworkServer.Spawn(unit, connectionToClient);
    }

	#endregion

	#region Client

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left) { return; }

        if (!hasAuthority) { return; }

        CmdSpawnUnit();
	}

    #endregion
}
