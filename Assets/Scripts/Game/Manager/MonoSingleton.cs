using Core;
using UnityEngine;

/// <summary>
/// 继承自MonoBehavior的懒汉单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> :MonoBehaviour where T:MonoSingleton<T>
{
    //避免单例模式基类在外部被调用
    protected MonoSingleton() { }


    //记录单例模式对象是否存在，用于防止在OnDestory方法中访问单例对象报错
    public static bool isExisted { get; private set; } = false;

    //提供一个属性给外部访问，这个属性就相当于单例对象本身
    private static T instance; 

    /// <summary>
    /// 懒汉式单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);//反射更改单例对象的名字

                    instance=go.AddComponent<T>();//添加脚本组件

                    isExisted = true;//单例模式对象已经销毁
                    //可选：是否在跳转场景时销毁单例
                }
            }
            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        isExisted = false;
    }


}
