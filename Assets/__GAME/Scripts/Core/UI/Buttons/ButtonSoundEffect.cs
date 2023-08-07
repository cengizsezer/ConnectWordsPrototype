using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
