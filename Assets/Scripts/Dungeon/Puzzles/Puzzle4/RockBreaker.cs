using UnityEngine;

public class RockBreaker : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleEnter();
        }
    }
    private Rock currentTarget;
    private int enterStage = 0;

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
         Rock[] allRocks = FindObjectsOfType<Rock>();
        Rock closest = null;
        float closestDist = 5f;

        foreach (Rock rock in allRocks)
        {
            float dist = Vector3.Distance(transform.position, rock.transform.position);
            Debug.Log("Rock: " + rock.name + " distance: " + dist);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = rock;
            }
        }

        if (closest != null)
        {
            currentTarget = closest;
            enterStage = 1;
            currentTarget.SetStage(1);
            Debug.Log("Stage 1: Rock selected - " + closest.name);
        }
        else
        {
            Debug.Log("No rock found within 5 units");
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
            Debug.Log("currentTarget is NULL!");
            ResetInteraction();
            return;
        }

        Debug.Log("Calling Break on: " + currentTarget.name);
        Vector3 dir = transform.forward;
        dir.y = 0f;
        dir = new Vector3(Mathf.Round(dir.x), 0f, Mathf.Round(dir.z)).normalized;

        currentTarget.SetStage(0);
        currentTarget.Break(dir);

        Debug.Log("Stage 3: Rock broken!");
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