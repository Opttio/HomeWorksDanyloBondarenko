using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace _Project.Scripts.Runtime.Bootstraps
{
    public class FirebaseBootstrap : MonoBehaviour
    {
        [SerializeField] private string _databaseURL = "https://mygame-c3b88-default-rtdb.europe-west1.firebasedatabase.app/";
        
        public static string Uid {get; private set;}
        public static DatabaseReference Db {get; private set;}
        public static bool IsReady {get; private set;}

        private void Awake()
        {
            InitAsync().Forget();
        }

        public async UniTask InitAsync(bool force = false)
        {
            if (IsReady && !force) return;
            IsReady = false;

            try
            {
                var dep = await FirebaseApp.CheckAndFixDependenciesAsync();
                if (dep != DependencyStatus.Available)
                {
                    Debug.LogError($"[FB] Deps: {dep}");
                    return;
                }

                var auth = FirebaseAuth.DefaultInstance;
                if (auth.CurrentUser == null)
                {
                    await auth.SignInAnonymouslyAsync().AsUniTask().AttachExternalCancellation(destroyCancellationToken);
                }
                
                Uid = auth.CurrentUser.UserId;

                var app = FirebaseApp.DefaultInstance;
                var db = FirebaseDatabase.GetInstance(app, _databaseURL);
                db.SetPersistenceEnabled(false);
                Db = db.RootReference;

                IsReady = true;
                Debug.Log($"[FB] Ready. Uid = {Uid}");
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"[FB] Initialization cancelled");
            }

            catch (Exception e)
            {
                Debug.LogError($"[FB] Initialization error: {e}");
            }
        }
        public static async UniTask DeleteUserSaveData()
        {
            if (!IsReady || string.IsNullOrEmpty(Uid))
            {
                Debug.LogWarning("[FB] Cannot delete user save data: not ready or UID is empty");
                return;
            }

            try
            {
                await Db.Child("users").Child(Uid).Child("saveData").RemoveValueAsync();
                Debug.Log("[FB] saveData for user deleted successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FB] Failed to delete saveData: {e}");
            }
        }
    }
}
