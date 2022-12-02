using System;
using System.Runtime.InteropServices;
using System.Text;

namespace StressHeadset_TEST
{
    static class MediaControl
    {
        private static string Pcommand;
        private static StringBuilder ReturnData;
        private static bool isOpen;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        static public void Close()
        {
            Pcommand = "close MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
            isOpen = false;
        }

        /// <param name="sFileName" />This is the audio file's path and filename</param />
        static public void Open(string sFileName)
        {
            Pcommand = "open \"" + sFileName + "\" type mpegvideo alias MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
            isOpen = true;
        }

        /// <param name="loop" />If True,audio file will repeat</param />
        static public void Play(bool loop)
        {
            if (isOpen)
            {
                Pcommand = "play MediaFile";
                if (loop)
                    Pcommand += " REPEAT";

                mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        static public void Play(bool loop, int seekTime)
        {
            if (isOpen)
            {
                Pcommand = "play MediaFile";
                if (loop)
                    Pcommand += " REPEAT";

                if (seekTime > 0)
                    Pcommand += " from " + seekTime.ToString();

                mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        static public void Seek(int seekTime)
        {
            if (isOpen)
            {
                Pcommand = "seek MediaFile to " + seekTime.ToString();
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        static public void Stop()
        {
            Pcommand = "stop MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
            isOpen = false;
        }

        static public void Pause()
        {
            Pcommand = "pause MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
        }

        static public void Resume()
        {
            Pcommand = "resume MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
        }

        static public void MasterVolume(int value)
        {
            int vol = value;
            vol = (vol < 0) ? 0 : vol;
            vol = (vol > 1000) ? 1000 : vol;

            Pcommand = "setaudio MediaFile volume to " + vol.ToString();
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
        }

        /// Returns the current status player: playing, paused, stopped, etc.
        static public string Status()
        {
            int i = 128;

            Pcommand = "status MediaFile mode";
            ReturnData = new StringBuilder(i);
            mciSendString(Pcommand, ReturnData, i, IntPtr.Zero);

            return ReturnData.ToString();
        }

        static public int Length()
        {
            if (isOpen)
            {
                Pcommand = "status MediaFile length";
                mciSendString(Pcommand, ReturnData, ReturnData.Capacity, IntPtr.Zero);

                return int.Parse(ReturnData.ToString());
            }
            else
                return 0;
        }

        static public int Position()
        {
            if (isOpen)
            {
                Pcommand = "status MediaFile position";
                mciSendString(Pcommand, ReturnData, ReturnData.Capacity, IntPtr.Zero);

                return int.Parse(ReturnData.ToString());
            }
            else
                return 0;
        }
    }
}
