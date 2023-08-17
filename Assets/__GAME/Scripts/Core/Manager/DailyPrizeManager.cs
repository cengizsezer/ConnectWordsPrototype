using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using MyProject.Core.Manager;


namespace MyProject.DailyPrize
{
    public class DailyPrizeManager : PersistentSingleton<DailyPrizeManager>
    {
        public static bool Initialized = false;
        public static bool isRewardAvailable = false;
        public static DateTime dateToday = DateTime.Now;

        public delegate void OnInternetConnected();
        public static OnInternetConnected onInternetConnected;


        private void Start()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {

                LoadDateFromWeb();
            }
            else
            {

                StartChekingInternetConnection();
            }
        }

        void StartChekingInternetConnection()
        {
            StartCoroutine(WaitForConnection());
        }

        IEnumerator WaitForConnection()
        {
            yield return new WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);

            onInternetConnected?.Invoke();
            onInternetConnected = null;

            if (!Initialized)
            {
                LoadDateFromWeb();
            }
        }


        void LoadDateFromWeb()
        {
            if (Initialized) return;

            GetUtcTimeAsync(OnTimeReceived);
            HasConnection(connection =>
                Debug.Log("Connection: " + connection));


        }
        void HasConnection(Action<bool> callback)
        {
            StartCoroutine(Download(TimeServer, www => { callback(www.error == null); }));
        }

        private const string TimeServer = "https://script.google.com/macros/s/AKfycbxODNhDLs1f1BXsZZz57OzBFQWuujILkVdofJpAM0JsBerxJcVNd0gCE0IiX4eP1E1Y/exec";

        public void GetUtcTimeAsync(Action<bool, string, DateTime> callback)
        {
            string url = TimeServer;

            StartCoroutine(Download(url, www =>
            {
                if (www.error == null)
                {
                    try
                    {
                        callback(true, null, DateTime.Parse(www.downloadHandler.text, CultureInfo.InvariantCulture).ToUniversalTime());
                    }
                    catch (Exception e)
                    {
                        callback(false, e.Message, DateTime.MinValue);
                    }
                }
                else
                {
                    callback(false, www.error, DateTime.MinValue);
                }
            }));
        }

        private IEnumerator Download(string url, Action<UnityWebRequest> callback)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            yield return webRequest.SendWebRequest();

            callback(webRequest);
        }


        private void OnTimeReceived(bool success, string error, DateTime time)
        {
            if (success)
            {

                dateToday = time.ToLocalTime().Date;
                DateTime saved = SaveLoadManager.GetLastRewardTime().Date;

                Debug.Log("************** DATE TODAY :" + dateToday);
                Debug.Log("************** DATE SAVED :" + saved);

                int dif = DateTime.Compare(dateToday, saved);

                if (dif == 1)
                {
                    isRewardAvailable = true;
                }

                Initialized = true;
            }
            else
            {
                Debug.LogError("************ TIME FAIL ! ==> " + error);
            }
        }

    }
}

