using Mirror;

public class DespawnAfterVoting : NetworkBehaviour
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        VotingManager.OnVotingEnded += RemoveDeadPlayerRemains;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingEnded -= RemoveDeadPlayerRemains;
    }

    void RemoveDeadPlayerRemains()
    {
        NetworkServer.Destroy(gameObject);
    }
}
