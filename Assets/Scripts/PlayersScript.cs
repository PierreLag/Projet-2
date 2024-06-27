using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class PlayersScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    UIDocument loginPage;

    VisualElement root;
    Button disconnectButton;
    ListView playersListView;

    List<string> playersList;
    [SerializeField]
    int maxPlayersList = 10;

    string nickname;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        disconnectButton = root.Q<Button>("disconnect-button");
        playersListView = root.Q<ListView>("list-view");

        playersList = new List<string>(maxPlayersList);
        playersListView.itemsSource = playersList;

        playersListView.fixedItemHeight = 90f;
        playersListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        disconnectButton.clicked += OnDisconnectClicked;

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

    void OnDisconnectClicked()
    {
        StartCoroutine("Disconnect");
    }

    public IEnumerator Disconnect()
    {
        photonView.RPC("RemovePlayer", RpcTarget.AllBuffered, nickname);

        yield return new WaitForSeconds(1);
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("Disconnecting");

        playersList.RemoveRange(0, playersList.Count -1);
        loginPage.enabled = true;
    }

    [PunRPC]
    void RemovePlayer(string nickname)
    {
        playersListView.Clear();
        playersList.Remove(nickname);
        Debug.Log("Removing " + nickname);
        playersListView.RefreshItems();
    }
}
