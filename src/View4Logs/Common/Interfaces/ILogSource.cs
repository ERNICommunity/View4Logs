﻿using System;
using System.Collections.Generic;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSource : IObservable<IList<LogMessage>>
    {
        string Name { get; }

        void Start();
    }
}