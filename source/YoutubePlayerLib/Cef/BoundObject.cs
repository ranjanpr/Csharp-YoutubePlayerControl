﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubePlayerLib.Cef
{
    class BoundObject
    {
        public event EventHandler PlayerLoadingDone;
        public event EventHandler PlayerQualityChanged;
        public event EventHandler<YoutubePlayerState> PlayerPlayingChanged;

        public void PlayerLoaded()
        {
            if(PlayerLoadingDone != null)
            {
                PlayerLoadingDone(this, new EventArgs());
            }
        }

        public void qualityChanged()
        {
            PlayerQualityChanged?.Invoke(this, new EventArgs());
        }

        public void PlayingChanged(int state)
        {
            PlayerPlayingChanged?.Invoke(this, state.ParseToYoutubeState());
        }

        public void rateChanged(int data)
        {
            Console.WriteLine("BoundObject rate change recvd:"+data);
        }

        public void onApiChange(int data)
        {
            Console.WriteLine("BoundObject api change recvd:"+data);
        }

        public void onError(int data)
        {
            Console.WriteLine("BoundObject error recvd:"+data);
        }

        public void showMessage(string msg)
        {
            Console.WriteLine("BoundObject message rcvd: " + msg);
        }

        public void sendVideoId(string id)
        {
            Console.WriteLine("BoundObject videoid recvd: " + id);
        }
        
    }
}
