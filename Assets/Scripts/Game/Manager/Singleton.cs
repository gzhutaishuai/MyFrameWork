using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不继承自MonoBehavior的懒汉单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : Singleton<T>
{
    protected Singleton() { }

    private volatile static T instance;

    //线程锁，当多线程访问时，同一时刻仅允许一个线程访问
    private static object locker=new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = Activator.CreateInstance(typeof(T), true) as T;//使用反射，调用无参构造方法创建对象
                    }
                }
            }
            return instance;
        }
    }
}
