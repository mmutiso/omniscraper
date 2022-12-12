using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Infrastructure
{
    public interface ILoadApplicationKeys
    {
        TwitterKeys LoadTwitterKeys();

        string LoadByKeyName(string key);
    }
}
