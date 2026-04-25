using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    [SerializeField] private int keyid = 1;
    [SerializeField] private GameObject keyVFX;


    void Update()
    {
        transform.Rotate(0f, 45f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!c.CompareTag("Player")) return;

        Inventory inventory = c.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyid);
            Debug.Log("Picked up key " + keyid);
            gameObject.SetActive(false);

            if(keyVFX != null){
                keyVFX.SetActive(false);
            }
        }
    }
}