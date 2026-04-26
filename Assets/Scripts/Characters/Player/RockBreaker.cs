using UnityEngine;

public class RockBreaker : MonoBehaviour
{
    [SerializeField] private float range = 2f;
    [SerializeField] private LayerMask RockLayer;
    [SerializeField] private float rayHeight = 1f;

    private Rock currentTarget;
    private int enterStage = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleEnter();
        }
    }

    void HandleEnter()
    {
        if (enterStage == 0)
        {
            SelectTarget();
        }
        else if (enterStage == 1)
        {
            ChargeTarget();
        }
        else if (enterStage == 2)
        {
            ExecuteBreak();
        }
    }

    void SelectTarget()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * rayHeight;
        Vector3 dir = transform.forward;
        dir.y = 0f;
        dir.Normalize();

        if (Physics.Raycast(origin, dir, out hit, range, RockLayer))
        {
            Rock ice = hit.collider.GetComponent<Rock>();

            if (ice != null)
            {
                currentTarget = ice;
                enterStage = 1;
                currentTarget.SetStage(1);
                Debug.Log("Stage 1: Block selected");
            }
        }
    }

    void ChargeTarget()
    {
        if (currentTarget == null)
        {
            ResetInteraction();
            return;
        }

        enterStage = 2;
        currentTarget.SetStage(2);
        Debug.Log("Stage 2: Block charged");
    }

    void ExecuteBreak()
    {
        if (currentTarget == null)
        {
            ResetInteraction();
            return;
        }

        Vector3 dir = transform.forward;
        dir.y = 0f;
        dir = new Vector3(Mathf.Round(dir.x), 0f, Mathf.Round(dir.z)).normalized;

        currentTarget.SetStage(0);
        currentTarget.Break(dir);

        Debug.Log("Stage 3: Chain break executed");
        ResetInteraction();
    }

    void ResetInteraction()
    {
        if (currentTarget != null)
        {
            currentTarget.SetStage(0);
        }

        currentTarget = null;
        enterStage = 0;
    }
}