using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIv2.Services
{
    public interface IFeatureFlag
    {
        bool IsFeatureEnabled(string feature);
    }
}
