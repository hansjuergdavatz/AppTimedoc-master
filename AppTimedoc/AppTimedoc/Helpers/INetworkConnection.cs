﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimedoc.Helpers
{
  public interface INetworkConnection
  {
    bool IsConnected { get; }
    void CheckNetworkConnection();
  }
}
