﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7Wonders.Infrastructure
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }


}
