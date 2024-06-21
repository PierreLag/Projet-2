using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class MenuScript : MonoBehaviour
{
    VisualElement root;
    TextField emailField;
    TextField passwordField;
    Button loginButton;
    Label errorLabel;
    VisualElement rotatingGear;

    // Start is called before the first frame update
    void Start()
    {
        root = this.GetComponent<UIDocument>().rootVisualElement;
        emailField = root.Q<TextField>("Email");
        passwordField = root.Q<TextField>("Password");
        loginButton = root.Q<Button>("Login");
        errorLabel = root.Q<Label>("ErrorLabel");
        rotatingGear = root.Q<VisualElement>("RotatingGear");

        loginButton.clicked += LoginConfirmation;
    }

    // Update is called once per frame
    void Update()
    {
        rotatingGear.transform.rotation *= Quaternion.Euler(0, 0, 100f * Time.deltaTime);
    }

    void LoginConfirmation()
    {
        if (emailField.text == "" || passwordField.text == "")
        {
            errorLabel.text = "One or more field is empty";
        }
        else
        {
            string emailTest = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";   // Vérifie que l'email aie la forme "quelquechose@domaine.code"
            if (!Regex.Match(emailField.text, emailTest).Success)
            {
                errorLabel.text = "Please input a valid email address";
            } else
            {
                errorLabel.text = "";
                this.gameObject.SetActive(false);
            }
        }
    }
}
