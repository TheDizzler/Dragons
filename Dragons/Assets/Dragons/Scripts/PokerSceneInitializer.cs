using AtomosZ.Gambal.Poker;
using Mirror;
using UnityEngine;

public class PokerSceneInitializer : NetworkBehaviour
{
	[SerializeField] private GameObject dealerPrefab = null;
	private int numPlayers = -1;
	private int registeredPlayers = 0;
	private Dealer dealer;


	void Start()
	{
		GameObject dealerGo = Instantiate(dealerPrefab);
		NetworkServer.Spawn(dealerGo);
		dealer = dealerGo.GetComponent<Dealer>();
	}


	public void ReportNumPlayersToExpect(int num)
	{
		if (num <= 0)
			throw new System.Exception("Must have positive player num");
		numPlayers = num;
	}

	public void RegisterPlayer(PokerPlayer pokerPlayer)
	{
		++registeredPlayers;
		dealer.RegisterPlayer(pokerPlayer);
	}

	void Update()
	{
		// check if all players spawned
		if (registeredPlayers == numPlayers)
		{
			// start dealer
			dealer.DealNewHand();
			// destroy self
			Destroy(gameObject);
		}
	}
}
