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

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                return;
            }
        }

        if (animator == null)
        {
            return;
        }

        TurnToPlayer();

        if (dialog != null)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    }

    private void TurnToPlayer()
    {

        Vector2 diff = playerTransform.position - transform.position;

        float moveX = 0f;
        float moveY = 0f;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            moveX = diff.x > 0 ? 1f : -1f;
        }
        else
        {
            moveY = diff.y > 0 ? 1f : -1f;
        }

        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);

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