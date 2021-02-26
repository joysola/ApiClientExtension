using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace HttpClientExtension.Helper
{
    internal class OSHelper
    {
        /// <summary>
        /// 获取OS的名字
        /// </summary>
        /// <returns></returns>
        internal static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
        /// <summary>
        /// 是否操作系统版本低于win10
        /// </summary>
        /// <returns></returns>
        internal static bool IsOSLowThanWin10()
        {
            return !GetOSFriendlyName().StartsWith("Microsoft Windows 10");
        }
        /// <summary>
        /// 获取操作系统名称
        /// </summary>
        /// <param name="os_info"></param>
        /// <returns></returns>
        internal static string GetOsName(OperatingSystem os_info)
        {
            string version =
                os_info.Version.Major.ToString() + "." +
                os_info.Version.Minor.ToString();
            switch (version)
            {
                case "10.0": return "10/Server 2016";
                case "6.3": return "8.1/Server 2012 R2";
                case "6.2": return "8/Server 2012";
                case "6.1": return "7/Server 2008 R2";
                case "6.0": return "Server 2008/Vista";
                case "5.2": return "Server 2003 R2/Server 2003/XP 64-Bit Edition";
                case "5.1": return "XP";
                case "5.0": return "2000";
            }
            return "Unknown";
        }
    }
}
