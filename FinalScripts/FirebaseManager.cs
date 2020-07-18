using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Database;
using UnityEngine.UI;
using Firebase.Unity.Editor;

public class FirebaseManager : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private DatabaseReference _reference;
    public Text text;
    private string name;
    private string email;

    async void Start()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
        
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hwaiting-df83d.firebaseio.com/");
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void SignIn()
    {
        name = text.text;
        if (name == "") name = "noname";
        email = name + "@yahoo.com";
        Debug.Log("accountname: " + email);

        //await auth.CreateUserWithEmailAndPasswordAsync(email, "000000");
        //await auth.SignInWithEmailAndPasswordAsync(email, "000000");

        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    if (child.Key.Equals(name))
                    {
                        Debug.Log("Yet");
                        MenuController.bestScore = Int32.Parse(child.Value.ToString());
                        return;
                    }
                }
                Dictionary<string, object> childUpdates = new Dictionary<string, object>();
                childUpdates[name] = MenuController.bestScore;
 
                _reference.Child("users").UpdateChildrenAsync(childUpdates);
                Debug.Log("Not yet");
            }
        });
    }

    public void SaveScore()
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[name] = MenuController.bestScore;
 
        _reference.Child("users").UpdateChildrenAsync(childUpdates);
    }

    public async Task ScoreBoard()
    {
        for (int i = 0; i < 5; ++i)
        {
            EndgameController.username[i] = "...";
            EndgameController.userscore[i] = "...";
        }
        
        await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int childNum = (int) snapshot.ChildrenCount;
                int cnt = childNum;

                foreach (var child in snapshot.Children.OrderBy(ch => ch.Value))
                {
                    Debug.Log("scoreboard: " + child.Key + " " + child.Value.ToString());
                    if (child.Key.Equals(name)) EndgameController.yourPos = cnt;
                    
                    cnt--;
                    if (cnt < 5)
                    {
                        EndgameController.username[cnt] = child.Key;
                        EndgameController.userscore[cnt] = child.Value.ToString();
                    }
                }
            }
        });
        
        FindObjectOfType<EndgameController>().WriteScoreBoard();
        
        
    }
}
