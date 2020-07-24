using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

namespace Domain
{
    public class FirebaseManager : MonoBehaviour
    {
        private DatabaseReference _reference;
        private string _name;
        private string _email;

        private async void Start()
        {
            // Initialization Firebase;
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hwaiting-df83d.firebaseio.com/");
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async void SignIn(string userName)
        {
            _name = userName;
            if (_name == "") _name = "noname";
            _email = _name + "@yahoo.com";
            Debug.Log("Account Name: " + _email);

            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    if (!child.Key.Equals(_name)) continue;
                    Debug.Log("Yet");
                    ScoreData.bestScore = int.Parse(child.Value.ToString());
                    return;
                }

                var childUpdates = new Dictionary<string, object> {[_name] = ScoreData.bestScore};
                _reference.Child("users").UpdateChildrenAsync(childUpdates);
                Debug.Log("Not yet");
            });
            Debug.Log("End signin " + ScoreData.bestScore);
        }

        public async void SaveScore()
        {
            var childUpdates = new Dictionary<string, object> {[_name] = ScoreData.bestScore};
            await _reference.Child("users").UpdateChildrenAsync(childUpdates);
        }

        public async Task UpdateScoreBoard()
        {
            await FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
            {
                if (!task.IsCompleted) return;
                var snapshot = task.Result;
                var childNum = (int) snapshot.ChildrenCount;
                var counter = childNum;

                foreach (var child in snapshot.Children.OrderBy(ch => ch.Value))
                {
                    Debug.Log("scoreboard: " + child.Key + " " + child.Value);
                    if (child.Key.Equals(_name))
                    {
                        ScoreData.yourPosition = counter;
                        Debug.Log("write counter: " + counter);
                    }
                    
                    counter--;
                    if (counter >= 5) continue;
                    ScoreData.scoreBoardName[counter] = child.Key;
                    ScoreData.scoreBoardScore[counter] = child.Value.ToString();
                }
            });
            
            
        }
    }
}
