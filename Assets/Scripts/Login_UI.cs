using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Login_UI : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;

    [SerializeField] private GameObject loginUI;

    [SerializeField] private GameObject gameRoot;

    private Button loginButton;
    private Button registerButton;
    private Button googleButton;
    private TextField userNameField;
    private TextField passwordField;
    private void OnEnable()
    {
        // Escuta quando o login der certo
        AuthenticationManager.OnLoginSuccess += GoToGame;
    }

    private void OnDisable()
    {
        AuthenticationManager.OnLoginSuccess -= GoToGame;
    }
    private void Awake()
    {
        var root = uIDocument.rootVisualElement;
        loginButton = root.Q<Button>("loginButton");
        registerButton = root.Q<Button>("registerButton");
        userNameField = root.Q<TextField>("prontuarioTextField");
        passwordField = root.Q<TextField>("passwordTextField");
        googleButton = root.Q<Button>("googleButton");

        loginButton.clicked += LoginButtonClick;
        registerButton.clicked += RegisterButtonClick;
        if (googleButton != null)
            googleButton.clicked += GoogleButtonClick;
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
    private async void GoogleButtonClick()
    {
        if (AuthenticationManager.Instance == null)
        {
            Debug.LogError("AuthenticationManager não encontrado na cena!");
            return;
        }

        await AuthenticationManager.Instance.LoginWithGoogleAsync();
    }
    private void GoToGame()
    {
        Debug.Log("Login bem-sucedido! Entrando no jogo...");

        // Esconde a UI de login
        if (loginUI != null)
            loginUI.SetActive(false);

        // Mostra o jogo
        if (gameRoot != null)
            gameRoot.SetActive(true);

        // === RESET DO PERSONAGEM ===
        PlayerControll player = FindObjectOfType<PlayerControll>();
        if (player != null)
        {
            player.ResetPlayer();
        }

        // Garante que o GameController está no estado correto
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController != null)
        {
            gameController.SetGameState(GameState.FreeRoam);
        }
    }
}
