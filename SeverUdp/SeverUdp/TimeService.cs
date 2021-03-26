using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeverUdp_5._0
{

    class TimeService
    {
        static public int[] numbers = new int[10];
        static public void Timer ()
        {        
            TimerCallback timeCB = new TimerCallback(Plus);
            Timer timer = new Timer(timeCB, null, 0, 100);
        }

        static public void Plus(object state)
        {
            
            for(int i = 0; i<10;i++)
            {
                numbers[i] += 1; 
            }
        }
    }

    
}
