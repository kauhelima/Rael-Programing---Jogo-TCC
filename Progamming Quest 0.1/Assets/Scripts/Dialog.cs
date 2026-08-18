using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    [TextArea(2, 5)]
    [SerializeField] private List<string> lines;

    public List<string> Lines => lines;
}