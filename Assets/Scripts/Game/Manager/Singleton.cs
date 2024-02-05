using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̳���MonoBehavior����������ģʽ
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : Singleton<T>
{
    protected Singleton() { }

    private volatile static T instance;

    //�߳����������̷߳���ʱ��ͬһʱ�̽�����һ���̷߳���
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
                        instance = Activator.CreateInstance(typeof(T), true) as T;//ʹ�÷��䣬�����޲ι��췽����������
                    }
                }
            }
            return instance;
        }
    }
}
