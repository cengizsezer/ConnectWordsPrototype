# PROJE ADI:
- ConnectWordsPrototype
## OYUN HAKKINDA:
- Alt boardda ki sayıların grid üzerinde hareket ederek çizilen Line'ler ile harflere bağlanması ve üst boardda ki bulunduğu levele ait DOGRU kelime veya kelimelerin bulunmaya calısıldıgı bir 2D kelime-puzzle oyunudur.<br/>

## KOD ACIKLAMASI:
- Oyun 3 scene'den olusmaktadır.<br/>
### 1. LOADING SCENE
- Bu Scene **GameManager,SaveLoadManager,PrizeManager,VibrationManager ve DailiyManager** classları **PersistentSingleton** olarak oluşturulmuştur. Sceneler arası geçişlerde Objelerin Destroy olmasını engellemek için bu şekilde tasarlanmıştır.<br/>

- **GameManager**: Oyunun Levelları JSon dosyasından okunmuştur. GameManager classında Level verilerini yüklediğim classtır. Oyunun Arkaplan Spriteleri ve İngilizce-Türkce dil secimi bu classtan yüklenmistir.<br/>

- **SaveLoadManager**: Oyunda save sistemi olarak PlayerPref kullanılmıştır. Level,Coin,Ses ve titreşim ayarları ve analytic Eventler için gerekli oyuncu verileri bu classta tutulmuştur.<br/>

- **DailiyManager**: 24 saatte 1 kez hediye kutusu kazanması için internetten veri bu classta tutularak kontrol edilmiştir.<br/>
```
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
```
### 2. MAINMENU SCENE
- Oyun 1 Chaper 12 Level olacak sekilde tasarlanmıstır. Oyuncu Levelleri gectikce kilitli olanlar acılır. Oyuncu GameIn scne ile Main scene arasında geçiş yapabilir ve geçtiği chapter ve levelleri tekrar oynayabilmektedir.<br/>

- Ayrıca Oyunun Shop,Settings ve DailiyEvent bölümlerini içerdiği kısımdır.<br/>

- Sistem olarak Constructor Dependency Injection tasarım modeli baz alınmıştır. <br/>
```
                                                          - MainMenuManager -
                                                           MainMenuController
                    ChapterPanelController -GiftPanelController-SettingsPanelController-ToggleController- ShopPanelController ==> seklinde MainMenuController'ın CTOR'unda new() lenmişler MainManuControllerda MainMenuManagerin CTOR'unda oluşturulmuştur. References ve Settingsler scriptableObject olarak  MainMenuManager de tutulmuş ve oluşturulan classlara dağıtılmıştır.

```

### 2. GAMEIN SCENE
- Oyunun Oynandığı Scenedir<br/>

- HintButtonu,Settings, Üst Board,Alt Boarddan oluşur.<br/>

- Alt boardda ki Sayıları harflerle eşleştirdiğimizde birleştirilen harf curve sekilde ucarak üst boardda ki eşleştiği sayının üzerine yerleşir.Doğru kelime bulunmaya calısılır.<br/>

- Bu bölümde Controllerin ilişkisinde yine ***Constructor Dependency Injection*** tasarım modeli kullanılmış, ilgili classlar GameInControllerin CTOR'unda oluşturulmuş GameInController ise GameInManagerin CTOR'unda Oluşturulmuş  references ve settingsler buradan classlara dağıtılmıştır.<br/>

 
- Board-BoardCell ve WordTable-WordTableCell arasında ki ilişki de ***Generic Repository Pattern*** Tasarım Modelinden yararlanılmıştır. 
```
     - public interface IGenericBase<T> where T: Cell ---> Bir Generic İnterface olusturulmus Boardlar arası ortak özellikler ve Methodlar burada tutulmustur.
     - public abstract class GenericBoardBase<T> : IGenericBase<T> where T : Cell ----> Ortak abstract bir Base class yapılmış ve IGenericBase iplemente edilmiştir. Bu Classta fonksiyonların bodyleri doldurulmuştur.
     - public interface IBoard : IGenericBase<BoardCell> ----> Board classına özel özellik ve Methodlar ortak interface'den türetilmiş yeni bir classta tutulmuştur.
     - public class Board : GenericBoardBase<BoardCell>, IBoard ----> Hem OrtakBase hem de o classa ait interface ile kodun düzeni ve okunurlaklığı arttırılmaya calısılmıs ve Kod tekrarından kacınma, Kodun 
      genisletilebilir olması prensibi gereği SOLID prensiblerine uyulmaya calısılmıstır.
  
```    
