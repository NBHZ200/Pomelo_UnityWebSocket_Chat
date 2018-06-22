using System;
//using System.Timers;
using System.Threading;

namespace Pomelo.WebglPomelo
{
    public class HeartBeatService
    {
        int interval;
        public int timeout;
        //Timer timer;
        DateTime lastTime;
        Protocol protocol;

        public HeartBeatService(int interval, Protocol protocol)
        {
            this.interval = interval * 1000;
            this.protocol = protocol;
        }

        internal void resetTimeout()
        {
            this.timeout = 0;
            lastTime = DateTime.Now;
        }


       
        
        //public void sendHeartBeat(object source, ElapsedEventArgs e)
        //{

        //    TimeSpan span = DateTime.Now - lastTime;
        //    timeout = (int)span.TotalMilliseconds;

        //    //check timeout
        //    if (timeout > interval * 2)
        //    {
        //        protocol.getPomeloClient().disconnect();
        //        //stop();
        //        return;
        //    }

        //    //Send heart beat
        //    protocol.send(PackageType.PKG_HEARTBEAT);
            
        //}


        public void sendHeartBeat()
        {
            TimeSpan span = DateTime.Now - lastTime;
            timeout = (int)span.TotalMilliseconds;

            //check timeout
            if (timeout > interval * 2)
            {
                protocol.getPomeloClient().disconnect();
                //stop();
                return;
            }

            //Send heart beat
            protocol.send(PackageType.PKG_HEARTBEAT);

        }
        public void start()
        {
            if (interval < 1000) return;
            //start hearbeat
            // 调用:
            Delayer.Instance.WaitAndInvoke((float)interval / (float)1000, sendHeartBeat);
            //Set timeout
            timeout = 0;
            lastTime = DateTime.Now;

        }

        //public void start()
        //{
        //    WebglPomeloLogin.Instance.Alert("Start timer!");
        //    if (interval < 1000) return;

        //    //start hearbeat
        //    // 调用:
        //    Delayer.Instance.WaitAndInvoke((float)interval / (float)1000, sendHeartBeat);
        //    //timer = new Timer();
        //    //timer.Interval = interval;
        //    //timer.Elapsed += new ElapsedEventHandler(sendHeartBeat);
        //    //timer.Enabled = true;
            
        //    //catch (Exception e)
        //    //{
        //    //    if (e != null)
        //    //    {
        //    //        WebglPomeloLogin.Instance.Alert("timer enabled Error is:" + e.ToString());
        //    //    }
        //    //}
        //    //Set timeout
        //    timeout = 0;
        //    lastTime = DateTime.Now;
            
        //}

        public void stop()
        {
            Delayer.Instance.Stop();
        }
    }
}