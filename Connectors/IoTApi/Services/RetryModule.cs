﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daenet.Iot;

namespace Daenet.IoT.Services
{
    public static class RetryExtension
    {
        public static IotApi RegisterRetry(this IotApi api, int numOfRetries, TimeSpan delayTime)
        {
            RetryModule module = new Services.RetryModule(numOfRetries, delayTime);
            api.RegisterModule(module);
            return api;
        }
    }

    public class RetryModule : ISendModule
    {
        private int m_NumOfRetries = 3;

        private TimeSpan m_DelayTime = TimeSpan.FromMilliseconds(1000);

        private ISendModule m_NextModule;

        public ISendModule NextSendModule
        {
            get
            {
                return m_NextModule;
            }

            set
            {
                m_NextModule = value;
            }
        }

        public RetryModule(int numOfRetries, TimeSpan delayTime)
        {
            m_NumOfRetries = numOfRetries;
            m_DelayTime = delayTime;
        }
      
        public async Task SendAsync(object sensorMessage,
           Action<IList<object>> onSuccess = null,
           Action<IList<object>, Exception> onError = null,
           Dictionary<string, object> args = null)
        {
            int cnt = m_NumOfRetries;

            while (--cnt >= 0)
            {
                await NextSendModule.SendAsync(sensorMessage, (msgs) =>
                {
                    onSuccess?.Invoke(new List<object> { sensorMessage });
                    cnt = 0;
                },
                (msgs, err) =>
                {
                    if (cnt == 0)
                        onError?.Invoke(new List<object> { sensorMessage }, err);
                    else
                        Task.Delay(m_DelayTime).Wait();
                },
                args);
            }
        }

        public void Open(Dictionary<string, object> args = null)
        {
           
        }

        public Task<object> ReceiveAsync(object sensorMessage, Action<IList<object>> onSuccess = null, Action<IList<object>, Exception> onError = null, Dictionary<string, object> args = null)
        {
            throw new NotImplementedException("Retry module should not implement retrying.");
        }
    }
}
