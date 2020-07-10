using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all the logic for moving the deck around
/// </summary>
public class DeckManager : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject CardPrefab;

    public DeckDefinition DeckDefinition;
    public Text QueueSizeText;

    private List<Card> Cards;

    // This is our Queue of moves, which we can add to
    private Queue<CardMove> MoveCommands;
    private Camera cam;

    void Start()
    {
        MoveCommands = new Queue<CardMove>();
        cam = Camera.main;
        var screenAspect = (float) Screen.width / (float) Screen.height;
        var camHeight    = cam.orthographicSize * 2;
        var bounds       = new Bounds(Vector3.zero, new Vector3(camHeight * screenAspect, camHeight, 0));

        var cardHeightScale = bounds.size.y / 10 / 5;
        var cardWidthScale  = cardHeightScale * .75f;

        Cards = new List<Card>(52);

        for (var i = 0; i < 52; i++)
        {
            var go = Instantiate(CardPrefab, transform);
            go.transform.localScale = new Vector3(cardWidthScale, 1, cardHeightScale);

            var card = go.GetComponent<Card>();
            card.Offset = i;
            card.DeckDefinition = DeckDefinition;
            Cards.Add(card);
        }

        Reset();
    }

    void Update()
    {
        MoveNext();
        QueueSizeText.text = $"Queue Size: {MoveCommands.Count}";
    }

    /// <summary>
    /// Resets the position of all cards to the top of the screen.  Queues them up so they animate 1 card at at time
    /// </summary>
    public void Reset()
    {
        var height = cam.orthographicSize * 2;
        var width  = height * cam.aspect;

        for (var i = 0; i < 52; i++)
        {
            MoveCommands.Enqueue(new CardMove()
            {
                destination = new Vector3(-width / 2 * .9f + (width * 0.9f / 52 * i), height / 2 * .75f),
                go = Cards[i].gameObject,
            });
        }
    }

    /// <summary>
    /// Deals 5 random cards from the deck and places then at the bottom of the screen
    /// </summary>
    public void Deal()
    {
        var height = cam.orthographicSize * 2;
        var width  = height * cam.aspect;
        var cards  = new HashSet<int>();

        while (cards.Count < 5)
        {
            cards.Add(Random.Range(0, 52));
        }

        var i = 0;
        foreach (var offset in cards)
        {
            MoveCommands.Enqueue(new CardMove()
            {
                destination = new Vector3((width * 0.9f / 10 * i++), height / 2 * -.25f),
                go = Cards[offset].gameObject,
                moveSpeed = 0.15f,
            });
        }
    }

    /// <summary>
    /// Moves the next card in the queue 
    /// </summary>
    void MoveNext()
    {
        if (MoveCommands.Count == 0)
            return;

        var first = MoveCommands.Peek();
        if (first.moving)
            return;

        if (first.go.transform.position == first.destination)
        {
            MoveCommands.Dequeue();
            MoveNext();
            return;
        }

        // We actually need to move the card
        first.moving = true;
        var cm = first.go.AddComponent<CardMover>();
        cm.Callback = MoveDone;
        cm.Duration = first.moveSpeed;
        cm.TargetPosition = first.destination;
    }

    /// <summary>
    /// Called by the CardMover as a delegate.  Removes the first item from the MoveCommands queue, which has been completed
    /// </summary>
    void MoveDone()
    {
        if (MoveCommands.Count > 0)
            MoveCommands.Dequeue();
    }
}

class CardMove
{
    public GameObject go;
    public Vector3 destination;
    public float moveSpeed = 0.05f;
    public bool moving;
}
