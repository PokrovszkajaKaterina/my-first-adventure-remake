using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPersistence : MonoBehaviour
{
    public static PlayerPersistence Instance;

    Rigidbody rb;
    CameraController cameraController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        rb = GetComponent<Rigidbody>();
        cameraController = GetComponentInChildren<CameraController>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPoint target = null;

        foreach (SpawnPoint point in FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None))
        {
            if (point.id == MapTransition.PendingSpawnId)
            {
                target = point;
                break;
            }
        }

        if (target == null)
            return;

        rb.linearVelocity = Vector3.zero;
        transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);

        if (cameraController != null)
            cameraController.SetYaw(target.transform.eulerAngles.y);
    }
}
