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

    string nickname;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ajouterButton = root.Q<Button>("add-button");
        playersListView = root.Q<ListView>("list-view");

        playersList = new List<string>(maxPlayersList);
        //playersListView.itemsSource = PhotonNetwork.PlayerList;
        playersListView.itemsSource = playersList;

        playersListView.fixedItemHeight = 90f;
        playersListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        PhotonNetwork.ConnectUsingSettings();
    }

    [PunRPC]
    void AddPlayer(string nickname)
    {
        playersListView.Clear();
        playersList.Add(nickname);

        playersListView.RefreshItems();
    }

    public void Login(string pseudo)
    {
        nickname = pseudo;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        photonView.RPC("AddPlayer", RpcTarget.AllBuffered, nickname);
    }
}
