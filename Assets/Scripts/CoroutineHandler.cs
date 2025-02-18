using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    private static CoroutineHandler instance = null;
    public static CoroutineHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject inst = new("Reload Handler");
                DontDestroyOnLoad(inst);
                instance = inst.AddComponent<CoroutineHandler>();
            }
            
            return instance;
        }
    }
}