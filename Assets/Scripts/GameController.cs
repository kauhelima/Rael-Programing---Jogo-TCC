using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Challenge}

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerControll playerControll;

    private GameState state;
    private void Awake()
    {
        // Se playerControll não foi atribuído no Inspector, tentamos buscar o PlayerControll automaticamente
        if (playerControll == null)
        {
            playerControll = FindObjectOfType<PlayerControll>();
            if (playerControll == null)
            {
                Debug.LogError("PlayerControll não foi encontrado na cena!");
            }
        }
    }
    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }
    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerControll.HandleUpdate();

        }else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();

        }else if(state == GameState.Challenge)
        {

        }
    }
    public void SetGameState(GameState newState)
    {
        state = newState;
    }
}
