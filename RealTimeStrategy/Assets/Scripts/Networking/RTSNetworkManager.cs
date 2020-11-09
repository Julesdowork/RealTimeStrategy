using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPf = null;
    [SerializeField] private GameOverHandler gameOverHandlerPf = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();
        player.SetTeamColor(new Color(
            UnityEngine.Random.Range(0, 1f),
            UnityEngine.Random.Range(0, 1f),
            UnityEngine.Random.Range(0, 1f)
        ));
        
        GameObject unitSpawner = Instantiate(unitSpawnerPf, conn.identity.transform.position,
            conn.identity.transform.rotation);

        NetworkServer.Spawn(unitSpawner, conn);
    }

	public override void OnServerSceneChanged(string sceneName)
	{
		if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            GameOverHandler gameOverHandler = Instantiate(gameOverHandlerPf);

            NetworkServer.Spawn(gameOverHandler.gameObject);
        }
	}
}
