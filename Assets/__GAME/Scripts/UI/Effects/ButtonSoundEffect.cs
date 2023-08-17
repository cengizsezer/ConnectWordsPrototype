using UnityEngine;
using UnityEngine.UI;
using MyProject.Core.Manager;


[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour
{
    Button btn;
    [SerializeField] Sounds soundEffect;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SoundManager.I.PlayOneShot(soundEffect));
    }
}
