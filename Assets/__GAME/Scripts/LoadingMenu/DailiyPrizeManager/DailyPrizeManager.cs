using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DailyPrizeManager : MonoSingleton<DailyPrizeManager>
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


[Serializable]
public class WorldTimeApi
{
    public string utc_datetime;
    //public string day_of_week;
    //public long week_number;
    //public string timezone;
    //public string abbreviation;
    //public DateTime client_ip;

    //public string dateTime;
    //public int[] arrDayWeekYear;
    //public bool isTakeMoney = false;

    //async void Start()
    //{
    //    var time = await GetUtcTimeAsync();
    //    dateTime = time.ToString();
    //    arrDayWeekYear = GetDayWeekYear();

    //    var arrCurrent = SaveLoadManager.GetTime().Reverse().ToArray();
    //    var arrPrev = arrDayWeekYear.Reverse().ToArray();

    //    for (int i = 0; i < arrCurrent.Length; i++)
    //    {
    //        int idx = i;

    //        if (arrCurrent[idx] == arrPrev[idx])
    //        {
    //            idx += 1;

    //            if (arrCurrent[idx] < arrPrev[idx])
    //            {
    //                isTakeMoney = true;
    //                Debug.Log("ertesi ay");
    //                break;

    //            }
    //            else if (arrCurrent[idx] == arrPrev[idx])
    //            {
    //                idx += 1;

    //                if (arrCurrent[idx] == arrPrev[idx])
    //                {
    //                    Debug.Log("aynı gün");

    //                    break;
    //                }
    //                else if (arrCurrent[idx] <= arrPrev[idx])
    //                {
    //                    isTakeMoney = true;
    //                    Debug.Log("ertesi gün");
    //                    break;
    //                }
    //            }
    //        }
    //        else if (arrCurrent[idx] <= arrPrev[idx])
    //        {
    //            isTakeMoney = true;
    //            Debug.Log("ertesi yıl");
    //            break;
    //        }
    //    }


    //    for (int i = 0; i < arrDayWeekYear.Length; i++)
    //    {
    //        SaveLoadManager.SetTime(arrDayWeekYear[i]);
    //    }


    //    SaveLoadManager.SetGiveGold(isTakeMoney);


    //}


    //public int[] GetDayWeekYear()
    //{

    //    string[] spearator = { "/", " ", ":" };
    //    int count = 6;

    //    string[] s_elements = dateTime.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);


    //    int[] resault = new int[s_elements.Length];


    //    for (int i = 0; i < resault.Length; i++)
    //    {
    //        int _i = -1;

    //        if (int.TryParse(s_elements[i], out _i))
    //        {
    //            resault[i] = _i;
    //        }
    //    }

    //    var arr = resault[0..3];

    //    return arr;
    //}

    //private async Task<DateTime> GetUtcTimeAsync()
    //{

    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            var response = await client.GetAsync("https://worldtimeapi.org/api/timezone/Europe/Istanbul%22");

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var content = await response.Content.ReadAsStringAsync();
    //                var data = JsonUtility.FromJson<WorldTimeApi>(content);

    //                return DateTime.Parse(data.utc_datetime);
    //            }
    //            else
    //            {
    //                Debug.LogWarning("Failed to get UTC time from web API.");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogWarning("Error getting UTC time from web API: " + ex.Message);
    //    }

    //    // Return local time if the web API call fails
    //    return DateTime.UtcNow.ToLocalTime();
    //}
}