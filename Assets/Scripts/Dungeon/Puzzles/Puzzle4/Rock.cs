using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private LayerMask RockLayer;
    [SerializeField] private float checkDistance = 1.5f;
    [SerializeField] private float rayHeight = 1f;
    [SerializeField] private GameObject breakVFX;

    private Renderer rend;
    private Color originalColor;
    private RockDestroyManager puzzleManager;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            originalColor = rend.material.color;
        }

        puzzleManager = FindObjectOfType<RockDestroyManager>();
    }

    public void SetStage(int stage)
    {
        if (rend == null) return;

        if (stage == 0)
        {
            rend.material.color = originalColor;
        }
        else if (stage == 1)
        {
            rend.material.color = Color.cyan;
        }
        else if (stage == 2)
        {
            rend.material.color = Color.blue;
        }
    }

    public void Break(Vector3 direction)
    {
        direction.y = 0f;
        direction = new Vector3(Mathf.Round(direction.x), 0f, Mathf.Round(direction.z)).normalized;

        Vector3 origin = transform.position + Vector3.up * rayHeight;

        RaycastHit hit;
        Rock nextBlock = null;

        if (Physics.Raycast(origin, direction, out hit, checkDistance, RockLayer))
        {
            nextBlock = hit.collider.GetComponent<Rock>();
        }

        if (breakVFX != null)
        {
            Debug.Log("Break() called on: " + gameObject.name); 
            Instantiate(breakVFX, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (puzzleManager != null)
        {
            puzzleManager.BlockDestroyed();
        }

        Destroy(gameObject);

        if (nextBlock != null)
        {
            nextBlock.Break(direction);
        }
    }
}