using System;//Enum����������
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    /// <summary>�¼�ϵͳ</summary>
    public class GlobalEvent
    {
        private static Dictionary<Enum, UnityEvent<object>> eventDic = null;

        public static void Init()
        {
            eventDic=new Dictionary<Enum, UnityEvent<object>>();
            
        }

        public static void AddListener(Enum eventName,UnityAction<object> listener)
        {
            //����Ѿ��������ί��
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
