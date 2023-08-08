# PROJE ADI:
ConnectWordsPrototype
## OYUN HAKKINDA:
Alt boardda ki sayıların grid üzerinde hareket ederek çizilen Line'ler ile harflere bağlanması ve üst boardda ki bulunduğu levele ait DOGRU kelime veya kelimelerin bulunmaya calısıldıgı bir 2D kelime-puzzle oyunudur.<br/>

Developer için Açıklama : Oyun 3 scene'den olusmaktadır. İlk scene Loading Scenedir.
Bu Scene GameManager,SaveLoadManager,PrizeManager,VibrationManager ve DailiyManager classları PersistentSingleton olarak oluşturulmuştur. Sceneler arası geçişlerde Objelerin Destroy olmasını engellemek için bu şekilde tasarlanmıştır.
GameManager: Oyunun Levelları JSon dosyasından okunmuştur. GameManager classında Level verilerini yüklediğim classtır. Oyunun Arkaplan Spriteleri ve İngilizce-Türkce dil secimi bu classtan yüklenmistir.
SaveLoadManager: Oyunda save sistemi olarak PlayerPref kullanılmıştır. Level,Coin,Ses ve titreşim ayarları ve analytic Eventler için gerekli oyuncu verileri bu classta tutulmuştur.
DailiyManager: 24 saatte 1 kez hediye kutusu kazanması için internetten veri bu classta tutularak kontrol edilmiştir.
                                      2.Scene-Main Scene
1-Oyun 1 Chaper 12 Level olacak sekilde tasarlanmıstır. Oyuncu Levelleri gectikce kilitli olanlar acılır. Oyuncu GameIn scne ile Main scene arasında geçiş yapabilir ve geçtiği chapter ve levelleri tekrar oynayabilmektedir.
2-Ayrıca Oyunun Shop,Settings ve DailiyEvent bölümlerini içerdiği kısımdır.
3- Sistem olarak Constructor Dependency Injection tasarım modeli baz alınmıştır. 
                                                          -MainMenuManager-
                                                         MainMenuController
                   ChapterPanelController -GiftPanelController-SettingsPanelController-ToggleController- ShopPanelController ==> seklinde MainMenuController'ın CTOR'unda new() lenmişler MainManuControllerda MainMenuManagerin
CTOR'unda oluşturulmuştur. References ve Settingsler scriptableObject olarak  MainMenuManager de tutulmuş ve oluşturulan classlara dağıtılmıştır.

                                    3.Scene-GameIn Scene
1-Oyunun Oynandığı Scenedir
2-HintButtonu,Settings, Üst Board,Alt Boarddan oluşur.
3-Alt boardda ki Sayıları harflerle eşleştirdiğimizde birleştirilen harf curve sekilde ucarak üst boardda ki eşleştiği sayının üzerine yerleşir.Doğru kelime bulunmaya calısılır.
4-Bu bölümde Controllerin ilişkisinde yine Constructor Dependency Injection tasarım modeli kullanılmış, ilgili classlar GameInControllerin CTOR'unda oluşturulmuş GameInController ise GameInManagerin CTOR'unda Oluşturulmuş
  references ve settingsler buradan classlara dağıtılmıştır.
5- Board-BoardCell ve WordTable-WordTableCell arasında ki ilişki de Generic Repository Pattern Tasarım Modelinden yararlanılmıştır. 

      public interface IGenericBase<T> where T: Cell ---> Bir Generic İnterface olusturulmus Boardlar arası ortak özellikler ve Methodlar burada tutulmustur.
      public abstract class GenericBoardBase<T> : IGenericBase<T> where T : Cell ----> Ortak abstract bir Base class yapılmış ve IGenericBase iplemente edilmiştir. Bu Classta fonksiyonların bodyleri doldurulmuştur.
      public interface IBoard : IGenericBase<BoardCell> ----> Board classına özel özellik ve Methodlar ortak interface'den türetilmiş yeni bir classta tutulmuştur.
      public class Board : GenericBoardBase<BoardCell>, IBoard ----> Hem OrtakBase hem de o classa ait interface ile kodun düzeni ve okunurlaklığı arttırılmaya calısılmıs ve Kod tekrarından kacınma, Kodun genisletilebilir olması
  prensibi gereği SOLID prensiblerine uyulmaya calısılmıstır.    
