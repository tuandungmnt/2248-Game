using UnityEngine;
using UnityEngine.Android;

public class Permissions : MonoBehaviour
{
    GameObject dialog = null;
    
    void Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
            Permission.RequestUserPermission(Permission.Microphone);
            dialog = new GameObject();
        }
        #endif
    }
    
    void OnGUI () {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
            //dialog.AddComponent<PermissionsRationalDialog>();
            return;
        }
        else if (dialog != null) {
            Destroy(dialog);
        }
        #endif
    }
}
