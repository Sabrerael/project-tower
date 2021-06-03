using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

/// <summary>
/// An inventory item that can be placed in the action bar and "Used".
/// </summary>
/// <remarks>
/// This class should be used as a base. Subclasses must implement the `Use`
/// method.
/// </remarks>
[CreateAssetMenu(menuName = ("Inventory/Passive Item"))]
public class PassiveItem : InventoryItem {}
