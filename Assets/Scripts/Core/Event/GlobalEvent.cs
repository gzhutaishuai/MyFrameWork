using System;//Enum类在这里面
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    /// <summary>事件系统</summary>
    public class GlobalEvent
    {
        private static Dictionary<Enum, UnityEvent<object>> eventDic = null;

        public static void Init()
        {
            eventDic=new Dictionary<Enum, UnityEvent<object>>();
            
        }

        public static void AddListener(Enum eventName,UnityAction<object> listener)
        {
            //如果已经存在这个委托
            if(eventDic.TryGetValue(eventName,out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent=new UnityEvent<object>();
                thisEvent.AddListener(listener);
                eventDic.Add(eventName, thisEvent);
            }
        }
    }
}
