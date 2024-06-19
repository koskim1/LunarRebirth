using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    [field: SerializeField] public List<ScriptableCard> CommonCards { get; private set; }
    [field: SerializeField] public List<ScriptableCard> RareCards { get; private set; }
    [field: SerializeField] public List<ScriptableCard> EpicCards { get; private set; }
    [field: SerializeField] public List<ScriptableCard> LegendaryCards { get; private set; }


}
