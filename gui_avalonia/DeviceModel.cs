using System.Collections.Generic;

namespace gui_avalonia
{
    public class DeviceModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> SupportedOps { get; set; } = new List<string>();

        public DeviceModel(string name, string code, List<string> supportedOps = null)
        {
            Name = name;
            Code = code;
            if (supportedOps != null)
            {
                SupportedOps = supportedOps;
            }
            else
            {
                // Default operations if none specified
                SupportedOps = new List<string>
                {
                    "Factory Reset + FRP", "Factory Reset Only",
                    "Flash Firmware", "Reset FRP Only",
                    "Read Info", "Read Codes",
                    "Repair IMEI", "Network Unlock",
                    "Reboot EDL", "Reboot Recovery",
                    "Read Pattern/PIN", "Remove MDM"
                };
            }
        }
    }
}
