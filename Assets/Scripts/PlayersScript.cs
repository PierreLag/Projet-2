using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayersScript : MonoBehaviour
{
    VisualElement root;
    Button ajouterButton;
    ListView playersListView;

    List<string> playersList;
    [SerializeField]
    int maxPlayersList = 10;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ajouterButton = root.Q<Button>("add-button");
        playersListView = root.Q<ListView>("list-view");

        playersList = new List<string>(maxPlayersList);
        playersListView.itemsSource = playersList;
    }

    void AddPlayer()
    {
        playersListView.Clear();

    }
}
