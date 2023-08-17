using MoreMountains.NiceVibrations;


namespace MyProject.Core.Manager
{
    public class VibrationManeger : PersistentSingleton<VibrationManeger>
    {
        private void Start()
        {
            MMVibrationManager.SetHapticsActive(SaveLoadManager.IsHapticOn());
        }


        public static void ChangeVibrationStatus(bool on)
        {
            MMVibrationManager.SetHapticsActive(!on);
        }

        //public static void EnableVibration(bool isEnabled)
        //{
        //    SaveLoadManager.SetVibrationStatus(isEnabled);
        //    MMVibrationManager.SetHapticsActive(SaveLoadManager.IsHapticOn());
        //}

        public static void Haptic(HapticTypes type = HapticTypes.MediumImpact)
        {
            if (MMVibrationManager.HapticsSupported())
                MMVibrationManager.Haptic(type);
        }
    }
}

