using UnityEngine;

public class NPCController : MonoBehaviour, Interacao
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private Animator animator;

    private bool playerInRange;
    private Transform playerTransform;

    private void Awake()
    {
        if (dialog == null)
            Debug.LogError("Dialog não foi atribuído para o NPC: " + gameObject.name);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (dialog != null)
        {
            TurnToPlayer();
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    }

    private void TurnToPlayer()
    {
        if (playerTransform == null) return;

        var diff = playerTransform.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            animator.SetFloat("moveX", diff.x > 0 ? 1f : -1f);
            animator.SetFloat("moveY", 0f);
        }
        else
        {
            animator.SetFloat("moveX", 0f);
            animator.SetFloat("moveY", diff.y > 0 ? 1f : -1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
        }
    }
}