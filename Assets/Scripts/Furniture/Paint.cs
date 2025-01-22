using UnityEngine;

public class Paint : MonoBehaviour
{
    public string PaintName;
    public Sprite PaintIcon;
    public int Price;
    public Material material;

    public enum Type { Floor, Wall };
    public Type type;
}
