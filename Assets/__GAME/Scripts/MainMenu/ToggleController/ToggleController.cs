using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController:IDisposable
{

    private Vector2 SoundAncPos, MusicAncPos, HapticAncPos;
    private Toggle tglSound;
    private Toggle tglMusic;
    private Toggle tglHaptic;
    private RectTransform tglSoundHandle;
    private RectTransform tglMusicHandle;
    private RectTransform tglHapticHandle;

    public ToggleController() :this(null,null)
    {

    }


    public ToggleController(ToggleSceneSettings settings, ToggleSceneReferences references)
    {
        tglSound = references.ToggleSound;
        tglHaptic = references.ToggleHaptic;
        tglMusic = references.ToggleMusic;

        tglSoundHandle = references.ToggleSoundHandle;
        tglMusicHandle = references.ToggleMusicHandle;
        tglHapticHandle = references.ToggleHapticHandle;

        SoundAncPos = references.ToggleSoundHandle.anchoredPosition;
        MusicAncPos = references.ToggleMusicHandle.anchoredPosition;
        HapticAncPos = references.ToggleHapticHandle.anchoredPosition;

        SetSettingToggles();
        ResetEvent();
        AddValueChancedEvent();
    }



    void ResetEvent()
    {
        tglHaptic.onValueChanged.RemoveListener(SetHaptic);
        tglMusic.onValueChanged.RemoveListener(SetMusic);
        tglSound.onValueChanged.RemoveListener(SetSound);

    }
    void AddValueChancedEvent()
    {
        tglHaptic.onValueChanged.AddListener(SetHaptic);
        tglMusic.onValueChanged.AddListener(SetMusic);
        tglSound.onValueChanged.AddListener(SetSound);
    }

    void SetSettingToggles()
    {
        tglHaptic.SetIsOnWithoutNotify(SaveLoadManager.IsHapticOn());
        SetHaptic(SaveLoadManager.IsHapticOn());
        tglMusic.SetIsOnWithoutNotify(SaveLoadManager.IsMusicAvailable());
        SetMusic(SaveLoadManager.IsMusicAvailable());
        tglSound.SetIsOnWithoutNotify(SaveLoadManager.IsSoundAvailable());
        SetSound(SaveLoadManager.IsSoundAvailable());
    }


    public void SetSound(bool isOn)
    {
        tglSoundHandle.anchoredPosition = isOn ? SoundAncPos : SoundAncPos * -1f;
        SaveLoadManager.SetSound(isOn);
    }

    public void SetMusic(bool isOn)
    {
        tglMusicHandle.anchoredPosition = isOn ? MusicAncPos : MusicAncPos * -1f;
        SaveLoadManager.SetMusic(isOn);
    }

    public void SetHaptic(bool isOn)
    {
        tglHapticHandle.anchoredPosition = isOn ? HapticAncPos : HapticAncPos * -1f;
        SaveLoadManager.SetHaptic(isOn);
    }

    public void Dispose()
    {
        ResetEvent();
    }
}
