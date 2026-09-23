using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance { get; private set; }

    public static event Action OnLoginSuccess;

    private bool isInitialized = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await InitializeServices();
            SetupEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Debug.Log($"Unity Services State: {UnityServices.State}");
    }
    public async Task InitializeServices()
    {
        if (isInitialized) return;

        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
            isInitialized = true;
            Debug.Log("Unity Services inicializado com sucesso!");
        }
        catch (Exception e)
        {
            Debug.LogError("Falha ao inicializar Unity Services: " + e.Message);
            Debug.LogException(e);
        }
    }
    public async Task<string> RegisterWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        }
        catch (AuthenticationException e)
        {
            if(e.ErrorCode == 51)
            {
                return "Acesso Negado, Token Invalido, Tenta o login De Novo";
            }
            Debug.LogException(e);
            return e.Message;
        }
        catch (RequestFailedException e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        return "";
    }
    public async Task<string> LoginWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);

            OnLoginSuccess?.Invoke();
        }
        catch (AuthenticationException e)
        {
            if (e.ErrorCode == 51)
            {
                return "Acesso Negado, Token Invalido, Tenta o login De Novo";
            }
            Debug.LogException(e);
            return e.Message;
        }
        catch (RequestFailedException e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return e.Message;
        }
        return "";
    }
    public async Task LoginWithGoogleAsync()
    {
        // Garante que está inicializado antes de continuar
        if (!isInitialized)
        {
            await InitializeServices();
        }

        if (!isInitialized)
        {
            Debug.LogError("Unity Services não foi inicializado. Não é possível fazer login com Google.");
            return;
        }

        try
        {
            Debug.Log("Iniciando login com Google...");

            // Abre a tela de login do Player Accounts
            await PlayerAccountService.Instance.StartSignInAsync();

            // Pega o token e autentica no Authentication Service
            string accessToken = PlayerAccountService.Instance.AccessToken;

            if (string.IsNullOrEmpty(accessToken))
            {
                Debug.LogError("Access Token vazio após o login.");
                return;
            }

            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);

            Debug.Log("Login com Google realizado com sucesso!");
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");

            OnLoginSuccess?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("Erro no login com Google:");
            Debug.LogException(e);
        }
    }
    private static void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            Debug.Log($"Player Name: {AuthenticationService.Instance.PlayerName}");
        };
        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log($"Player signed out");

        };
    }
}
