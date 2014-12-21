using UnityEngine;
using System.Collections;
/// <summary>
/// Dodaje tło pod tekstem (obramowanie).
/// </summary>
public class TextBorder : MonoBehaviour
{
    /// <summary>
    /// Kolor tła.
    /// </summary>
    public Color color;

    /// <summary>
    /// Rozmiar tła.
    /// </summary>
    public int size;

    /// <summary>
    /// Pozycja tła.
    /// </summary>
    public Vector2 position;

    GameObject text2;
    TextMesh textMesh;
    TextMesh textMesh_;

    // Use this for initialization
    void Start()
    {
        text2 = new GameObject("bg");
        text2.transform.parent = transform;
        text2.transform.localPosition = new Vector3(position.x, position.y, 0.001f);

        text2.AddComponent("MeshRenderer");
        MeshRenderer meshRenderer = text2.GetComponent<MeshRenderer>();
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;

        text2.AddComponent("TextMesh");
        textMesh = text2.GetComponent<TextMesh>();
        textMesh_ = GetComponent<TextMesh>();

        textMesh.offsetZ = textMesh_.offsetZ;
        textMesh.characterSize = textMesh_.characterSize;
        textMesh.lineSpacing = textMesh_.lineSpacing;
        textMesh.anchor = textMesh_.anchor;
        textMesh.alignment = textMesh_.alignment;
        textMesh.tabSize = textMesh_.tabSize;
        textMesh.fontSize = textMesh_.fontSize + size;
        textMesh.fontStyle = textMesh_.fontStyle;
        textMesh.richText = textMesh_.richText;
        textMesh.font = textMesh_.font;
        textMesh.color = color;
        textMesh.text = textMesh_.text;
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = textMesh_.text;
    }
}