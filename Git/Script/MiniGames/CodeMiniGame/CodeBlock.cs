using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    public enum BlockType
    {
        Function,
        Variable,
        Operator,
    }

    [SerializeField] private BlockType blockType;
    [SerializeField] private string blockName;
    [SerializeField] private string blockText;
    [SerializeField] private int orderIndex;

    public BlockType Type => blockType;

    public int OrderIndex => orderIndex;
}
