using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    /// <summary>UI������/// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>UI���ļ��ϣ���¼��ǰ�����е��������// </summary>
        public static Dictionary<Type, UIPanelBase> PanelDic = null;

        /// <summary>������㼶�ļ���// </summary>
        public static Dictionary<UIPanelLayer, RectTransform> layers = null;


        #region ��ʼ���������
        /// <summary>UI����// </summary>
        public static Canvas Canvas { set; private get; } = null;

        /// <summary>UI�¼���Ӧ// </summary>
        public static EventSystem EventSystem { set; private get; } = null;

        /// <summary>UIManager������ȫ��Ψһ// </summary>
        public static UIManager Instance { set; private get; } = null;

        /// <summary>UI�������// </summary>
        public static Camera Camera { set; private get; } = null;
        #endregion

        /// <summary>��ȡָ����UI�㼶λ��// </summary>
        public static RectTransform GetLayer(UIPanelLayer layer)
        {
            return layers[layer];
        }

        /// <summary>
        /// ��ʼ��UI���
        /// </summary>
        public static void Init()
        {
            PanelDic = new Dictionary<Type, UIPanelBase>();

            //��¡UIManager
            var obj = Resources.Load("Prefabs/UI/UIRoot/UIRoot");
            Instance = GameObject.Instantiate(obj).AddComponent<UIManager>();
            Instance.name = nameof(UIManager);
            DontDestroyOnLoad(Instance);

            //��ȡ���
            Canvas = Instance.GetComponentInChildren<Canvas>();
            EventSystem = Instance.GetComponentInChildren<EventSystem>();
            Camera = Instance.GetComponentInChildren<Camera>();

            //��ȡUIRoot�����в㼶
            layers = new Dictionary<UIPanelLayer, RectTransform>();

            //��ö���и�ֵ
            foreach (UIPanelLayer layer in Enum.GetValues(typeof(UIPanelLayer)))
            {
                //�ӻ��������������ҵ���Ӧ���Ƶ�UI�㼶��ͨ��ת������ΪRectTransfrom
                layers.Add(layer, Canvas.transform.Find(layer.ToString()) as RectTransform);
            }
            Debug.Log("UIRoot�����ɹ�");
        }


        /// <summary>
        /// ��һ�����
        /// </summary>
        /// <typeparam name="T">Ҫ�򿪵��������</typeparam>
        /// <param name="data">Ҫ���������</param>
        public static void Open<T>(object data = null) where T : UIPanelBase
        {
            //�����
            void openPanel(UIPanelBase panel)
            {
                //���´򿪵�UI�������Ϊ��ǰ�㼶�����·��������Ⱦ��������ʾ
                panel.transform.SetAsLastSibling();

                //��������
                panel.SetData(data);

                //�����
                panel.OnUIEnable();
            }

            UIPanelBase clonePanel()
            {
                //�������·��
                var panelRootPath = "Prefabs/UI";
                var panelSubPath = $"UIPanel/{typeof(T).Name}";
                var panelPath = $"{panelRootPath}/{panelSubPath}";

                //�������
                var obj = Resources.Load(panelPath);
                if (obj == null)
                {
                    Debug.LogError($"��·����:{panelRootPath}δ���ҵ������:{typeof(T).Name}");
                }

                //����������ڲ㼶
                RectTransform layer = UIManager.GetLayer(UIPanelLayer.Game);

                var objects = typeof(T).GetCustomAttributes(typeof(UILayerAttribute), true);
                if (objects?.Length > 0)
                {
                    var layerAttr = objects[0] as UILayerAttribute;
                    layer = UIManager.GetLayer(layerAttr.layer);
                }

                //��¡���
                var go = GameObject.Instantiate(obj, layer);
                go.name = typeof(T).Name;

                var newPanel = go.AddComponent<T>();//��������Ӧ�Ľű�

                return newPanel;

            }

            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {

                //�����
                openPanel(panel);
            }
            else
            {
                //��¡���
                panel = clonePanel();

                //��������¼���ֵ���ȥ
                PanelDic.Add(typeof(T), panel);

                //��ʼ�����
                panel.OnUIAwake();

                //�ӳ�һ֡����OnUIStart
                Instance.StartCoroutine(Invoke(panel.OnUIStart));

                //�����
                openPanel(panel);
            }
        }

        /// <summary>
        /// �ر�һ�����
        /// </summary>
        /// <typeparam name="T">Ҫ�رյ��������</typeparam>
        public static void Close<T>() where T : UIPanelBase
        {
            if (!PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                Debug.LogError($"������δ���ҵ�{typeof(T).Name}���");
            }
            else
            {
                Close(panel);
            }
        }
        /// <summary>
        /// �ر����ķ�����չ
        /// </summary>
        /// <param name="panel">������ͬ��T������ֱ�Ӵ����������</param>
        public static void Close(UIPanelBase panel)
        {
            panel?.OnUIDisable();
        }
        /// <summary>
        /// �ر��������
        /// </summary>
        public static void CloseAll()
        {
            foreach (var panel in PanelDic.Values)
            {
                UIManager.Close(panel);
            }
        }
        /// <summary>
        /// ɾ��һ�����
        /// </summary>
        /// <typeparam name="T">Ҫɾ�����������</typeparam>
        public static void Destory<T>() where T : UIPanelBase
        {
            if (!PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                Debug.LogWarning($"������δ���ҵ�{typeof(T).Name}���");
            }
            else
            {
                UIManager.Destory(panel);
            }
        }
        /// <summary>
        /// ɾ��������չ����
        /// </summary>
        /// <param name="panel">������ͬ������ֱ�Ӵ����������</param>
        public static void Destory(UIPanelBase panel)
        {
            panel?.OnUIDisable();//�ȹر�panel
            panel?.OnUIDestory();

            //���������
            if (PanelDic.ContainsKey(panel.GetType()))
            {
                PanelDic.Remove(panel.GetType());
            }
        }
        /// <summary>
        /// ɾ���������
        /// </summary>
        public static void DestoryAll()
        {
            List<UIPanelBase> panels = new List<UIPanelBase>(PanelDic.Values);
            foreach (var panel in panels)
            {
                UIManager.Destory(panel);
            }
            panels.Clear();
            PanelDic.Clear();
        }
        /// <summary>
        /// ��ȡһ�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : UIPanelBase
        {
            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                return panel as T;
            }
            else
            {
                Debug.LogError($"������δ���ҵ�{typeof(T).Name}���");
                return default(T);
            }

        }
        /// <summary>
        /// ��֤OnUIStart����OnUIAwake����
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private static IEnumerator Invoke(Action callback)
        {
            yield return new WaitForEndOfFrame();

            callback?.Invoke();
        }
    }
}
