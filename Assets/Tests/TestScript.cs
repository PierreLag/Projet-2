using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TestScript
{
    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator TestUIDocumentExists()
    {
        // Use the Assert class to test conditions
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.Find("UIDocumentLogin");
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        Assert.IsNotNull(uIDocument);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestButtonWorks()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.Find("UIDocumentLogin");
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        Label errorLabel = uIDocument.rootVisualElement.Q<Label>("ErrorLabel");
        Button button = uIDocument.rootVisualElement.Q<Button>("Login");

        using (var clicked = new NavigationSubmitEvent() { target = button })
            button.SendEvent(clicked);

        Assert.IsTrue(errorLabel.text == "One or more field is empty");
    }

    [UnityTest]
    public IEnumerator TestEmailCheckFalse()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.Find("UIDocumentLogin");
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        Label errorLabel = uIDocument.rootVisualElement.Q<Label>("ErrorLabel");
        Button button = uIDocument.rootVisualElement.Q<Button>("Login");
        TextField pseudoField = uIDocument.rootVisualElement.Q<TextField>("Pseudo");
        TextField password = uIDocument.rootVisualElement.Q<TextField>("Password");
        TextField email = uIDocument.rootVisualElement.Q<TextField>("Email");

        password.value = "Test";
        pseudoField.value = "Test";
        email.value = "test";
        using (var clicked = new NavigationSubmitEvent() { target = button })
            button.SendEvent(clicked);

        Assert.IsTrue(errorLabel.text == "Please input a valid email address");
    }

    [UnityTest]
    public IEnumerator TestEmailCheckTrue()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.Find("UIDocumentLogin");
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        Label errorLabel = uIDocument.rootVisualElement.Q<Label>("ErrorLabel");
        Button button = uIDocument.rootVisualElement.Q<Button>("Login");
        TextField pseudoField = uIDocument.rootVisualElement.Q<TextField>("Pseudo");
        TextField password = uIDocument.rootVisualElement.Q<TextField>("Password");
        TextField email = uIDocument.rootVisualElement.Q<TextField>("Email");

        password.value = "Test";
        pseudoField.value = "Test";
        email.value = "test@email.com";
        using (var clicked = new NavigationSubmitEvent() { target = button })
            button.SendEvent(clicked);

        Assert.IsTrue(errorLabel.text == "");
    }
}
