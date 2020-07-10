using UnityEngine;

/// <summary>
/// Represents the front of the card, simply for seperation of texture setting for child game objects 
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
// FIXME: There's probably a way to do this without using a separate gameobject for the front of the card
public class CardFront : MonoBehaviour
{
    private Material _material;
    private MeshRenderer _meshRenderer;
    public Texture2D Texture;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = new Material(Shader.Find("Standard"));
        _meshRenderer.material = _material;
        _meshRenderer = GetComponent<MeshRenderer>();
        _material.mainTexture = Texture;
    }
}
