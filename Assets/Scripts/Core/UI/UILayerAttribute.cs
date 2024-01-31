using System;

namespace Core.UI
{
public class UILayerAttribute :Attribute
{
    public UIPanelLayer layer { get; }

    /// <summary>
    /// ָ���㼶��壬ͨ��ӳ���layer��ȷ��UI�㼶
    /// </summary>
    /// <param name="layer"></param>
    public UILayerAttribute(UIPanelLayer layer)
    {
        this.layer = layer;
    }
}
}