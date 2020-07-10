using UnityEngine;

/// <summary>
/// Defines a deck, and more importantly the different textures for that deck
/// </summary>
[CreateAssetMenu(fileName = "Deck.asset", menuName = "Assets/Create Deck")]
public class DeckDefinition : ScriptableObject
{
    public Texture2D[] DeckTextures;
}
