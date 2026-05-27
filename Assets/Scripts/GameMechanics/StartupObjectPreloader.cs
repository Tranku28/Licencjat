using UnityEngine;

public class StartupObjectPreloader : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToPreload;
    void Start()
    {
        PreloadObjects();
        DisablePreloadedObjects();
    }

    private void PreloadObjects()
    {
        foreach (GameObject obj in objectsToPreload)
            obj.SetActive(true);
    }

    private void DisablePreloadedObjects()
    {
        foreach (GameObject obj in objectsToPreload)
            obj.SetActive(false);
    }
}
