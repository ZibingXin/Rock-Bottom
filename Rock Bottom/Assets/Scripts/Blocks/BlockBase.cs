using UnityEngine;

public abstract class BlockBase : MonoBehaviour
{
    protected bool isUsed = false;
    protected int depth;
    protected float priceToDig;

    public abstract void Dig();
}
