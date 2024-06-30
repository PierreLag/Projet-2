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
        // Obtention des objets de l'UIDocument.
        root = GetComponent<UIDocument>().rootVisualElement;
        disconnectButton = root.Q<Button>("disconnect-button");
        playersListView = root.Q<ListView>("list-view");

        // Création de la liste des joueurs et lien entre cette liste et l'affichage ListView des joueurs.
        playersList = new List<string>(maxPlayersList);
        playersListView.itemsSource = playersList;

        // Gestion de l'affichage des objets dans ListView.
        playersListView.fixedItemHeight = 90f;
        playersListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        // Ajout de l'action de déconnexion au bouton Disconnect.
        disconnectButton.clicked += OnDisconnectClicked;

        // Connexion au serveur Photon.
        PhotonNetwork.ConnectUsingSettings();
    }

    /**
     <summary>
     Cette méthode est envoyée en RPC à tous les récipients affectés afin d'ajouter un joueur à leur liste de joueurs dans le salon.
     </summary>
     <param name="nickname"> Le pseudo du joueur qui rejoint. </param>
    **/
    [PunRPC]
    void AddPlayer(string nickname)
    {
        playersListView.Clear();
        playersList.Add(nickname);

        playersListView.RefreshItems();
    }

    /**
     <summary>
     Cette méthode permet de se connecter à un salon en utilisant un pseudonyme.
     </summary>
     <param name="pseudo"> Le pseudo du joueur. </param>
    **/
    public void Login(string pseudo)
    {
        nickname = pseudo;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Une fois que le joueur rejoint le salon, on indique aux autres joueurs et ceux qui viendront qu'on est arrivé.
        photonView.RPC("AddPlayer", RpcTarget.AllBuffered, nickname);
    }

    /**
     * <summary>Cette méthode fait appel à la coroutine de déconnexion pour que le joueur quitte le salon dans lequel il se trouve.</summary>
    **/
    void OnDisconnectClicked()
    {
        StartCoroutine("Disconnect");
    }

    /**
     * <summary>Cette coroutine envoie un message de déconnexion à tous les autres joueurs dans le salon, puis se déconnecte du salon.</summary>
    **/
    public IEnumerator Disconnect()
    {
        photonView.RPC("RemovePlayer", RpcTarget.AllBuffered, nickname);

        yield return new WaitForSeconds(1);
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        // Debug.Log("Disconnecting");

        playersList.RemoveRange(0, playersList.Count -1);
        loginPage.enabled = true;
    }

    /**
     * <summary>Cette méthode est envoyée en RPC à tous les récipients affectés afin de retirer un joueur de leur liste de joueurs dans le salon.</summary>
     * <param name="nickname"> Le pseudo du joueur qui quitte. </param>
    **/
    [PunRPC]
    void RemovePlayer(string nickname)
    {
        playersListView.Clear();
        playersList.Remove(nickname);
        // Debug.Log("Removing " + nickname);
        playersListView.RefreshItems();
    }
}
