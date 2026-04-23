using UnityEngine;

namespace MyCasino.Slots.Data
{
    [CreateAssetMenu(menuName = "MyCasino/Slots/Symbol", fileName = "SlotSymbol")]
    public class SlotSymbol : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; } = "symbol";
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int PayoutMultiplier { get; private set; } = 1;
        [field: SerializeField, Range(0.01f, 1f)] public float Weight { get; private set; } = 0.2f;
    }
}
