using UnityEngine;

public class ColorPlateManager : MonoBehaviour
{
    public ColorPlateTrigger[] plates;
    public DoorUpward2 Door2;
    private bool opened = false;

    public void CheckAll()
    {
        if(opened) return;

        foreach (var p in plates)
        {
            if (!p.iscorrect) return;
        }
        opened = true;
        Debug.Log("Color plate puzzle is completed.");
        Door2.OpenDoor();
    }
}