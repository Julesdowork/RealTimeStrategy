using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    private Targetable target;

    public Targetable GetTarget()
    {
        return target;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGo)
    {
        if (!targetGo.TryGetComponent<Targetable>(out Targetable target)) { return; }

        this.target = target;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }
}
