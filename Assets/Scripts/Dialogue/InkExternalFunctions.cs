using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{

    public void bindIncreaseSIL(Story story, string choice){
        story.BindExternalFunction("increaseSIL", (string choice) => {
            PlayerStats playerStats = PlayerStats.GetOrFindInstance();
            if (playerStats != null)
            {
                playerStats.increaseSIL(choice);
                Debug.Log("Called increaseSIL with choice: " + choice);
            }
            else
            {
                Debug.LogError("Unable to call increaseSIL: PlayerStats instance is missing.");
            }
        });
    }

    public void unbindIncreaseSIL(Story story){
        story.UnbindExternalFunction("increaseSIL");
        Debug.Log("Unbound increaseSIL function from story");
    }

    public void bindGetSIL(Story story){
        story.BindExternalFunction("getSIL", (string choice) =>
    {
        PlayerStats playerStats = PlayerStats.GetOrFindInstance();
        if (playerStats != null)
        {
            int silValue = playerStats.getSIL(choice);
            Debug.Log("Called getSIL with choice: " + choice + ", returning: " + silValue);
            return silValue;
        }
        else
        {
            Debug.LogError("Unable to call getSIL: PlayerStats instance is missing.");
            return 0; // Default value if PlayerStats is not available
        }
    });
}

    public void unbindGetSIL(Story story){
        story.UnbindExternalFunction("getSIL");
        Debug.Log("Unbound getSIL function from story");
    }


}
