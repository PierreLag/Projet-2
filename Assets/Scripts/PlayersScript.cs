using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class PlayersScript : MonoBehaviourPunCallbacks
{
    VisualElement root;
    Button ajouterButton;
    ListView playersListView;

    List<string> playersList;
    [SerializeField]
    int maxPlayersList = 10;

    string pseudo;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ajouterButton = root.Q<Button>("add-button");
        playersListView = root.Q<ListView>("list-view");

        playersList = new List<string>(maxPlayersList);
        playersListView.itemsSource = playersList;
        ajouterButton.clicked += AddPlayer;

        playersListView.fixedItemHeight = 90f;
        playersListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
    }

    [PunRPC]
    void AddPlayer()
    {
        playersListView.Clear();
        Debug.Log("Ajout de joueur " + pseudo);
        playersList.Add(pseudo);
        playersListView.RefreshItems();
    }

    public void Login(string pseudo)
    {
        this.pseudo = pseudo;
        PhotonNetwork.JoinRandomOrCreateRoom();
        photonView.RPC("AddPlayer", RpcTarget.AllBuffered);
    }
}
