using Assignment_2_a;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2_a
{
    class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
        {
            return true;
        }

        public bool Save(string filename)
        {
            return true;
        }
    }
}