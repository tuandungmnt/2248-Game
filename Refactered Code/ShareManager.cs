using System.Collections;
using System.IO;
using UnityEngine;

namespace Domain
{
    public class ShareManager : MonoBehaviour
    {
        public void Share()
        {
            StartCoroutine(TakeScreenshotAndShare());
        }
    
        private IEnumerator TakeScreenshotAndShare()
        {
            yield return new WaitForEndOfFrame();

            var ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
            ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
            ss.Apply();
        
            Debug.Log("take screenshot");

            var filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
            File.WriteAllBytes( filePath, ss.EncodeToPNG() );

            Destroy( ss );

            Debug.Log("Delete");
        
            new NativeShare().AddFile( filePath )
                .SetSubject( "2248Game" ).SetText("Ha ha ha")
                .SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
                .Share();
        }
    }
}
