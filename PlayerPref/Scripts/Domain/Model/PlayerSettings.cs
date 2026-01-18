namespace Xenocode.Features.PlayerPref.Scripts.Domain.Model
{
    public class PlayerSettings
    {
        //audio
        public virtual float MasterVolume { get; set; }
        public virtual float SfxVolume { get; set; }
        public virtual float MusicVolume { get; set; }
        public virtual bool IsMuted { get; set; }
        
        //video
        public virtual int QualityIndex { get; set; }
        public virtual int ResolutionIndex { get; set; }
        public virtual int ScreenMode { get; set; }
        public virtual int VSync { get; set; }
        public virtual int Framerate { get; set; }
    }
}