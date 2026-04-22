using UnityEngine;

public class ColorPlateManager : MonoBehaviour
{
    public ColorPlateTrigger[] plates;
    public DoorUpward2 Door2;

    public void CheckAll()
    {
        foreach (var p in plates)
        {
            if (!p.iscorrect) return;
        }

        Door2.OpenDoor();
    }
}