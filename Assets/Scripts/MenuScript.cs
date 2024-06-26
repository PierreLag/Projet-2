using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class MenuScript : MonoBehaviour
{
    VisualElement root;
    TextField emailField;
    TextField pseudoField;
    TextField passwordField;
    Button loginButton;
    Label errorLabel;
    VisualElement rotatingGear;

    [SerializeField]
    PlayersScript playersScript;

    // Start is called before the first frame update
    void Start()
    {
        root = this.GetComponent<UIDocument>().rootVisualElement;
        emailField = root.Q<TextField>("Email");
        pseudoField = root.Q<TextField>("Pseudo");
        passwordField = root.Q<TextField>("Password");
        loginButton = root.Q<Button>("Login");
        errorLabel = root.Q<Label>("ErrorLabel");
        rotatingGear = root.Q<VisualElement>("RotatingGear");

        loginButton.clicked += LoginConfirmation;
    }

    // Update is called once per frame
    void Update()
    {
        // Fait tourner l'engrenage � chaque frame.
        rotatingGear.transform.rotation *= Quaternion.Euler(0, 0, 100f * Time.deltaTime);
    }

    /**
     * <summary>Cette m�thode v�rifie que les champs soient remplis, et v�rifie que l'email ins�r� poss�de un bon format.</summary>
    **/
    void LoginConfirmation()
    {
        if (emailField.text == "" || passwordField.text == "")
        {
            errorLabel.text = "One or more field is empty";
        }
        else
        {
            string emailTest = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";   // V�rifie que l'email aie la forme "quelquechose@domaine.code"
            if (!Regex.Match(emailField.text, emailTest).Success)
            {
                errorLabel.text = "Please input a valid email address";
            } else
            {
                if (pseudoField.text == "")
                {
                    errorLabel.text = "Please insert a username";
                } else
                {
                    errorLabel.text = "";
                    playersScript.Login(pseudoField.text);
                    GetComponent<UIDocument>().enabled = false;
                }
            }
        }
    }
}
