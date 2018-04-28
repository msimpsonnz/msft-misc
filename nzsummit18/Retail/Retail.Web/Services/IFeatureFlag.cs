using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail.Web.Services
{
    public interface IFeatureFlag
    {
        bool IsFeatureEnabled(string feature);
    }
}
