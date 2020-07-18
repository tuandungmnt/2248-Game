using UnityEngine;
using System.IO;
using System.Collections;

public class ShareManager : MonoBehaviour
{
    public void ShareButton()
    {
        Debug.Log("click share button");
        StartCoroutine(TakeScreenshotAndShare());
    }
    
    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
        ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
        ss.Apply();
        
        Debug.Log("take screenshot");

        string filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
        File.WriteAllBytes( filePath, ss.EncodeToPNG() );

        // To avoid memory leaks
        Destroy( ss );

        Debug.Log("delele");
        
        new NativeShare().AddFile( filePath )
            .SetSubject( "2248Game" ).SetText( "Hahaha" )
            .SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
    }
}
