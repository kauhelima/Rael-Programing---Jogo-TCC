using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Debug.Log($"Unity Services State: {UnityServices.State}");
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
    // Update is called once per frame
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
