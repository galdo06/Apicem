using System;
using System.ComponentModel.DataAnnotations;

namespace WS.Models
{
    public class Device
    {
        [Required]
        public string DeviceID { get; set; }

        public Device(string deviceID)
        {
            DeviceID = deviceID;
        }
    }
}