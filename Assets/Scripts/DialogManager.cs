using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;

    [SerializeField] private int lettersPerSecond = 30;

    private Queue<string> lines;

    public event Action OnShowDialog;
    public event Action OnHideDialog;
    public static DialogManager Instance { get; private set; }

    public bool IsShowing { get; private set; }
    private void Awake()
    {
        Instance = this;
        lines = new Queue<string>();

        Debug.Log(dialogText);
    }

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;
    public IEnumerator ShowDialog(Dialog dialog)
    {
        this.dialog = dialog;
        currentLine = 0;
        IsShowing = true;

        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
    }
    public void HandleUpdate()
    {
        if (dialog == null) return;

        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                IsShowing = false;
                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && isTyping)
        {
            StopAllCoroutines();
            dialogText.text = dialog.Lines[currentLine];
            isTyping = false;
        }
    }
    IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";

        yield return null;

        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}