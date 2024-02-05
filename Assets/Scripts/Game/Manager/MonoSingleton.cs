using Core;
using UnityEngine;

/// <summary>
/// �̳���MonoBehavior����������ģʽ
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> :MonoBehaviour where T:MonoSingleton<T>
{
    //���ⵥ��ģʽ�������ⲿ������
    protected MonoSingleton() { }


    //��¼����ģʽ�����Ƿ���ڣ����ڷ�ֹ��OnDestory�����з��ʵ������󱨴�
    public static bool isExisted { get; private set; } = false;

    //�ṩһ�����Ը��ⲿ���ʣ�������Ծ��൱�ڵ���������
    private static T instance; 

    /// <summary>
    /// ����ʽ����
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
                    GameObject go = new GameObject(typeof(T).Name);//������ĵ������������

                    instance=go.AddComponent<T>();//��ӽű����

                    isExisted = true;//����ģʽ�����Ѿ�����
                    //��ѡ���Ƿ�����ת����ʱ���ٵ���
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
