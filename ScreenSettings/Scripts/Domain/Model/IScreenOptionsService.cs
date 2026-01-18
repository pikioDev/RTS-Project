using System.Collections.Generic;

namespace Xenocode.Features.ScreenSettings.Scripts.Domain.Model
{
    public interface IScreenOptionsService
    {
        void InitializeGraphicsQuality();
        void InitializeResolution();
        void InitializeFramerate();
        void InitializeVsyncState();
        void InitializeScreenMode();
        int GetFrameRateIndex();
        int GetVsyncState();
        int GetQualityIndex();
        int GetScreenMode();
        int GetResolutionIndex();
        List<string> GetResolutionsList();
        void SetResolution(int resolutionIndex);
        void SetQuality(int qualityIndex);
        void SetVSync(int index);
        void SetFrameRate(float frameRate);
        void SetScreenMode(int mode);
    }
}