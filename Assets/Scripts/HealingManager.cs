using UnityEngine;

public class HealingManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag.Equals("Player"))
        {
            Debug.Log("heart");
            if (player.GetComponent<PlayerController>().GetCurrentHealth() < 100)
            {
                Debug.Log("cheers");

                player.GetComponent<PlayerController>().Heal();
                Destroy(gameObject);
            }
        }
    }
}
