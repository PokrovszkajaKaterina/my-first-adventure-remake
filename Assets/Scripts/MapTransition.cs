using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public string targetSceneName;
    public string spawnPointId = "Default";
    public KeyCode interactKey = KeyCode.E;

    public static string PendingSpawnId = "Default";

    bool playerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
            playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            PendingSpawnId = spawnPointId;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
