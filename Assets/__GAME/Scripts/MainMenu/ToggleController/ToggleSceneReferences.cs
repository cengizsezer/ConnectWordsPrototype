using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ToggleSceneReferences
{
    [BHeader("Toggle Controller References")]
    [SerializeField] CanvasGroup panelShop;
    [SerializeField] Toggle tglSound;
    [SerializeField] Toggle tglMusic;
    [SerializeField] Toggle tglHaptic;
    [SerializeField] RectTransform tglSoundHandle;
    [SerializeField] RectTransform tglMusicHandle;
    [SerializeField] RectTransform tglHapticHandle;

    public CanvasGroup PanelShop => panelShop;

    public Toggle ToggleSound => tglSound;
    public Toggle ToggleMusic => tglMusic;
    public Toggle ToggleHaptic => tglHaptic;

    public RectTransform ToggleSoundHandle => tglSoundHandle;
    public RectTransform ToggleMusicHandle => tglMusicHandle;
    public RectTransform ToggleHapticHandle => tglHapticHandle;
}
