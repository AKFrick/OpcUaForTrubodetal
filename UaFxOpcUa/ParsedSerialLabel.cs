﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OpcUaForTrubodetal
{
    public class ParsedSerialLabel
    {
        public ParsedSerialLabel(string StartSerialNumber)
        {
            Regex regex = new Regex(@"[^0-9]{5}/[^0-9]{2}$");
            if (StartSerialNumber != null)
            {


                if (false && regex.IsMatch(StartSerialNumber))
                {
                    string[] splitted = StartSerialNumber.Split('/');
                    StartSerial = int.Parse(splitted[0]);
                    EndLabel = $"/{splitted[1]}";
                }
                else
                {
                    StartSerial = 1;
                    EndLabel = $"/{DateTime.Now:yy}";
                    //Log.logThis($"Неверный формат серийного номер: {StartSerialNumber}");
                }
            }
            else
            {
                StartSerial = 0;
                EndLabel = "";
            }
        }
        public int StartSerial { get; private set; }
        public string EndLabel { get; private set; }
    }
}
