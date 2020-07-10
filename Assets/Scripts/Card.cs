using UnityEngine;

/// <summary>
/// Represents a single card, and sets the texture of the card based on the offset in the deck
/// </summary>
public class Card : MonoBehaviour
{
    public DeckDefinition DeckDefinition;
    public GameObject FrontPrefab;
    public int Offset;
    public bool isFlipped;

    private CardFront front;

    void Start()
    {
        isFlipped = false;
        var go = Instantiate(FrontPrefab, transform);
        front = go.GetComponent<CardFront>();
        front.Texture = DeckDefinition.DeckTextures[Offset];
    }
}
