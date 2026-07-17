using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Player Stats")]
    private int bucketShadowIntegrationLevel = 0;
    private int lanternShadowIntegrationLevel = 0;
    private int sprayShadowIntegrationLevel = 0;

    public static PlayerStats GetOrFindInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<PlayerStats>();
            if (Instance == null)
            {
                Debug.LogError("PlayerStats instance not found in scene. Add a GameObject with PlayerStats attached.");
            }
        }
        return Instance;
    }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void increaseSIL(string choice)
    {
        if (choice == "Bucket")
        {
            bucketShadowIntegrationLevel +=1;
            Debug.Log("Bucket SIL increased to: " + bucketShadowIntegrationLevel);
        }

        else if (choice == "Lantern")
        {
            lanternShadowIntegrationLevel +=1;
            Debug.Log("Lantern SIL increased to: " + lanternShadowIntegrationLevel);
        }

        else if (choice == "Spray")
        {
            sprayShadowIntegrationLevel +=1;
            Debug.Log("Spray SIL increased to: " + sprayShadowIntegrationLevel);
        }
    }

    public int getSIL(string choice)
    {
        if (choice == "Bucket")
        {
            return bucketShadowIntegrationLevel;
        }

        else if (choice == "Lantern")
        {
            return lanternShadowIntegrationLevel;
        }

        else if (choice == "Spray")
        {
            return sprayShadowIntegrationLevel;
        }
        return 0;
    }
}