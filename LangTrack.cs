namespace App.LangTracker;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

public sealed class LangTrack
{
    [DllImport("user32.dll")]
    static extern IntPtr GetKeyboardLayout(uint idThread);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

    private CultureInfo? _currentLanaguge = null;

    private static CultureInfo GetCurrentCulture()
    {
        var l = GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));
        return new CultureInfo((short)l.ToInt64());
    }

    public void HandleCurrentLanguage()
    {
        var currentCulture = GetCurrentCulture();
        if (_currentLanaguge == null || _currentLanaguge.LCID != currentCulture.LCID)
        {
            _currentLanaguge = currentCulture;
            SetWslLang(currentCulture);
        }
    }

    private void SetWslLang(CultureInfo culture)
    {
        string lang = culture.Name.Split('-')[1].ToLower();

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = String.Format("/C wsl win_set_lang {0}; tmux refresh-client -S", lang);
        process.StartInfo = startInfo;
        process.Start();
    }

}