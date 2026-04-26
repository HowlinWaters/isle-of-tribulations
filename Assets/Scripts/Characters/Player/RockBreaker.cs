using UnityEngine;

public class RockBreaker : MonoBehaviour
{
    [SerializeField] private AudioSource hammerSound;
    [SerializeField] private float hitRange = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HitClosestRock();
        }
    }

    void HitClosestRock()
    {
        Rock[] allRocks = FindObjectsOfType<Rock>();
        Rock closest = null;
        float closestDist = hitRange;

        foreach (Rock rock in allRocks)
        {
            float dist = Vector3.Distance(transform.position, rock.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = rock;
            }
        }

        if (closest != null)
        {
            if (hammerSound != null)
            {
                hammerSound.Play();
            }

            Vector3 dir = transform.forward;
            dir.y = 0f;
            dir = new Vector3(Mathf.Round(dir.x), 0f, Mathf.Round(dir.z)).normalized;

            closest.Hit(dir);
        }
        else
        {
            Debug.Log("No rock found within range");
        }
    }
}