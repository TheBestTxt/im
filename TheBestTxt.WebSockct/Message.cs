﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestTxt.SocketAbout.Common
{
    public class Message
    {
        public string SendClientId { get; set; }
        public string Msg { get; set; }
        public string Action { get; set; }
        public string Nick { get; set; }
    }
}
