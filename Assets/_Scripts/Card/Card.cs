using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines what a card is and can be, will connect all data and behaviours
/// </summary>
/// 
[RequireComponent(typeof(CardUI))] // will automatically attach the CardUI Script to every object that
//is a card
public class Card : MonoBehaviour
{
    [field: SerializeField] public ScriptableCard CardData { get; set; }
}
