using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Login_UI : MonoBehaviour
{
    [SerializeField]
    private UIDocument uIDocument;

    private Button loginButton;
    private Button registerButton;
    private TextField userNameField;
    private TextField passwordField;

    private void Awake()
    {
        var root = uIDocument.rootVisualElement;
        loginButton = root.Q<Button>("loginButton");
        registerButton = root.Q<Button>("registerButton");
        userNameField = root.Q<TextField>("prontuarioTextField");
        passwordField = root.Q<TextField>("passwordTextField");

        loginButton.clicked += LoginButtonClick;
        registerButton.clicked += RegisterButtonClick;
    }

    private async void RegisterButtonClick()
    {
        var errorText = await AuthenticationManager.Instance.RegisterWithUsernamePasswordAsync(
            userNameField.value,
            passwordField.value
        );
        Debug.Log(errorText);
    }

    private async void LoginButtonClick()
    {
        var errorText = await AuthenticationManager.Instance.LoginWithUsernamePasswordAsync(
            userNameField.value,
            passwordField.value
        );
        Debug.Log(errorText);
    }
}
