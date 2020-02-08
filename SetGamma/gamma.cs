using System;
using System.Runtime.InteropServices;

namespace SetGamma
{
    public class Gamma
    {
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        RAMP ramp = new RAMP();
        [DllImport("gdi32.dll")]
        public static extern int SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]

        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        public void SetDefaultGamma()
        {
            ramp.Red = new ushort[256];
            ramp.Green = new ushort[256];
            ramp.Blue = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] =
                (ushort)(i * 256);
            }
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
        }

        private double CalColorGammaVal(ushort[] line)
        {
            var index = 1;
            var min = line[index];
            double gamma = Math.Log((min - 0.5) / 65535, (index + 1) / 256.0) * 10;
            return gamma;
        }
        private double CalAllGammaVal(RAMP ramp)
        {
            return Math.Round(((CalColorGammaVal(ramp.Blue) + CalColorGammaVal(ramp.Red) + CalColorGammaVal(ramp.Green)) / 3), 2);
        }

        public void SetGamma(int gammaValue)
        {
            double gamma = 10 - 0.09 * gammaValue;
            ramp.Red = new ushort[256];
            ramp.Green = new ushort[256];
            ramp.Blue = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] =
                (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, gamma * 0.1) * 65535 + 0.5)));
            }
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
        }

        public int GetGamma()
        {
            var ramp = default(RAMP);
            GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            double gamma = CalAllGammaVal(ramp);
            int gammaValue = Convert.ToInt32(Math.Round((10 - gamma) / 0.09));
            return gammaValue;
        }


    }
}
