using UnityEngine;

[CreateAssetMenu(fileName = "Elements", menuName = "Scriptable Objects/Elements")]
public class Elements : ScriptableObject
{
    [SerializeField] string elementName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] private Elements[] StrongAgainst;
    [SerializeField] private Elements[] WeakAgainst;
    public Elements[] StrongAgainstElements { get => StrongAgainst; set => StrongAgainst = value; }
    public Elements[] WeakAgainstElements { get => WeakAgainst; set => WeakAgainst = value; }
    public bool IsStrongAgainst(Elements otherElement)
    {
        foreach (var element in StrongAgainst)
        {
            if (element == otherElement)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsWeakAgainst(Elements otherElement)
    {
        foreach (var element in WeakAgainst)
        {
            if (element == otherElement)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsNeutralAgainst(Elements otherElement)
    {
        return !IsStrongAgainst(otherElement) && !IsWeakAgainst(otherElement);
    }

}
