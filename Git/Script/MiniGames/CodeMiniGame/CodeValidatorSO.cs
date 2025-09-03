using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CodeValidatorSO", menuName = "CodeValidatorSO")]
public class CodeValidatorSO : ScriptableObject
{
    [SerializeField] List<int> correctOrder;

    public List<int> CorrectOrder => correctOrder;
}
