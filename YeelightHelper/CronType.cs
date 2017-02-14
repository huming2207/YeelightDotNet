using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeelightHelper
{
    /// <summary>
    /// Currently Xiaomi only added support for powering off their lightbulb...
    /// 
    /// That's stupid, they've already have a "set_scene" method in their API which accepts auto-poweroff. 
    /// But it seems that they've designed an exactly same interface here.
    /// </summary>
    public enum CronType
    {
        PowerOff = 0
    }
}
