using DG.Tweening;
using UnityEngine;

namespace Guinea.Core.UI.Effects
{
    public interface IEffect
    {
        void InsertTween(ref Sequence sequence, GameObject go=null){}
    }

    public enum AppendType
    {
        Append,
        Join,
    }
}