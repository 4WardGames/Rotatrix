using System.Collections.Generic;

using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private GameObject blockTemplate;

    [SerializeField]
    private int size = 5;

    private List<TowerBlock> originalBlocks = new List<TowerBlock>();
    private List<TowerBlock> changedBlocks = new List<TowerBlock>();

    public List<GameObject> originalTower = new List<GameObject>();
    public List<GameObject> changedTower = new List<GameObject>();

    public bool reverseDown = true;

    public List<Color> colorList;
    public List<Material> materialList;

    private int selectedBlock = -1;

    private List<Animation> animations = new List<Animation>();

    private float widthMultiplier = 2.0f;

    private float heightMultiplier = 2.0f;

    private float center = 0.0f;


    private float Width
    {
        get
        {
            //return blockTemplate.GetComponent<Renderer>().bounds.size.x;
            return blockTemplate.transform.localScale.x * 3;
        }
    }

    private float Height
    {
        get
        {
            return blockTemplate.transform.localScale.y;
        }
    }

    void Start()
    {
        SaveController.LoadLevel();
        LoadTower();
        //GenerateTower();
        Debug.Log(Height);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (animations.Count > 0)
        {
            if (animations[0].Animate(500))
            {
                animations.Remove(animations[0]);
                //Reverse(1);
            }
        }
        //for (int i = 0; i < changedBlocks.Count; i++)
        //{
        //    if (changedBlocks[i].targetWorldPosition != changedTower[i].transform.position)
        //    {
        //        var change = changedBlocks[i].targetWorldPosition - changedTower[i].transform.position;

        //        change = Vector3.ClampMagnitude(change, 0.05f);

        //        changedTower[i].transform.position += change;
        //    }
        //}
    }

    private void CheckVictory()
    {
        for (int i = 0; i < originalTower.Count; i++)
        {
            if (originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material.color
                != changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material.color)
            {
                return;
            }
        }

        Debug.Log("Jest dobrze!");
    }
    public void RotateStatic(int splitPoint)
    {
        for (int i = 0; i <= splitPoint / 2; i++)
        {
            (changedTower[splitPoint - i], changedTower[i]) = (changedTower[i], changedTower[splitPoint - i]);

            changedTower[i].transform.position = new Vector3(widthMultiplier * Width, i * Height, 0);
            changedTower[splitPoint - i].transform.position = new Vector3(widthMultiplier * Width, (splitPoint - i) * Height, 0);
        }
    }
    public void ReverseStatic(int splitPoint)
    {
        splitPoint++;
        List<GameObject> lastElements = changedTower.GetRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.RemoveRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.InsertRange(0, lastElements);

        for (int i = 0; i < changedTower.Count; i++)
        {
            changedTower[i].transform.position = new Vector3(widthMultiplier * Width, i * Height, 0);
        }
    }
    public void Rotate(int splitPoint)
    {
        if (splitPoint == 0)
        {
            if (selectedBlock != -1)
            {
                splitPoint = selectedBlock;
            }
            else
            {
                return;
            }
        }
        Vector3 rotationPoint = new Vector3(widthMultiplier * Width, (float)splitPoint / 2 * Height + center, 0);
        BlockRotation newAnimation = new BlockRotation(rotationPoint);

        for (int i = 0; i <= splitPoint / 2; i++)
        {
            (changedTower[splitPoint - i], changedTower[i]) = (changedTower[i], changedTower[splitPoint - i]);
        }

        for (int i = 0; i <= splitPoint; i++)
        {
            newAnimation.animatedBlocks.Add(changedTower[i]);
        }

        animations.Add(newAnimation);
        CheckVictory();
    }

    public void Reverse(int splitPoint)
    {
        if (splitPoint == 0)
        {
            if (selectedBlock != -1)
            {
                splitPoint = selectedBlock;
                if (!reverseDown)
                {
                    splitPoint -= 1;
                }
            }
            else
            {
                return;
            }
        }
        Vector3 reversalPoint = new Vector3(widthMultiplier * Width, (float)splitPoint * Height + center, 0);
        BlockReversal newAnimation = new BlockReversal(reversalPoint, splitPoint, changedTower.Count, widthMultiplier, Height);

        for (int i = 0; i <= splitPoint; i++)
        {
            newAnimation.bottomBlocks.Add(changedTower[i]);
        }
        for (int i = splitPoint + 1; i < changedTower.Count; i++)
        {
            newAnimation.topBlocks.Add(changedTower[i]);
        }
        splitPoint++;
        List<GameObject> lastElements = changedTower.GetRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.RemoveRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.InsertRange(0, lastElements);


        animations.Add(newAnimation);
        CheckVictory();
    }

    public bool SelectPoint(Vector3 point)
    {
        if (point.x > widthMultiplier - Width && point.x < widthMultiplier + Width)
        {
            for (int i = 0; i < size; i++)
            {
                Debug.Log((0.5 + (i - 1)) * Height + i);
                if (point.y > (0.5 + (i - 1)) * Height && point.y < (0.5 + i) * Height)
                {
                    selectedBlock = i;

                    Debug.Log(selectedBlock);
                    return true;
                }
            }
        }

        return false;
    }

    public void GenerateTower()
    {
        for (int i = 0; i < size; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            var material = Random.Range(0, materialList.Count);

            originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            originalTower[i].transform.position = new Vector3(-widthMultiplier * Width, i * Height, 0);

            originalTower[i].transform.parent = transform;

            //originalBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = originalTower[i].transform.position });

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            changedTower[i].transform.position = new Vector3(widthMultiplier * Width, i * Height, 0);

            changedTower[i].transform.parent = transform;

            //changedBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = changedTower[i].transform.position });
        }
        ReverseStatic(3);
        RotateStatic(3);
    }

    public void LoadTower()
    {
        originalTower = new List<GameObject>();
        var loadedTower = SaveController.towerData;

        blockTemplate.transform.localScale = new Vector3(blockTemplate.transform.localScale.x,
            blockTemplate.transform.localScale.y, blockTemplate.transform.localScale.z);

        for (int i = 0; i < loadedTower.colors.Count; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[loadedTower.colors[i]];

            originalTower[i].transform.position = new Vector3(-widthMultiplier * Width, i * Height, 0);

            originalTower[i].transform.parent = transform;

            //originalBlocks.Add(new TowerBlock { blockPosition = i, color = colorList[loadedTower.colors[i], targetWorldPosition = originalTower[i].transform.position });

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[loadedTower.colors[i]];
            changedTower[i].transform.position = new Vector3(widthMultiplier * Width, i * Height, 0);

            changedTower[i].transform.parent = transform;

            //changedBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = changedTower[i].transform.position });
        }

        foreach (var transformation in loadedTower.transforms)
        {
            if (transformation.rotate)
            {
                RotateStatic(transformation.splitPoint);
            }
            else
            {
                ReverseStatic(transformation.splitPoint);
            }
        }
    }
}

public class TowerBlock
{
    public int blockPosition;
    public int color;
    public Vector3 targetWorldPosition;
}

public interface Animation
{
    public bool Animate(float speed);
}

public class BlockRotation : Animation
{
    public List<GameObject> animatedBlocks = new List<GameObject>();

    public Vector3 rotationPoint;

    public float maxRotation = 0;

    public BlockRotation(Vector3 rotationPoint)
    {
        this.rotationPoint = rotationPoint;
    }

    public bool Animate(float speed)
    {
        maxRotation += speed * Time.deltaTime;

        float correction = 0;

        if (maxRotation > 180)
        {
            correction = maxRotation - 180;
        }

        foreach (var block in animatedBlocks)
        {
            block.transform.RotateAround(rotationPoint, Vector3.forward, speed * Time.deltaTime - correction);
        }



        if (maxRotation >= 180)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class BlockReversal : Animation
{
    public List<GameObject> topBlocks = new List<GameObject>();
    public List<GameObject> bottomBlocks = new List<GameObject>();

    public Vector3 topReversalPoint;
    public List<Vector3> bottomReversalPoints = new List<Vector3>();

    int splitPoint;

    bool moveDown = false;
    float height = 0;

    public BlockReversal(Vector3 reversalPoint, int splitPoint, int count, float widthMultiplier, float height)
    {
        this.splitPoint = splitPoint;
        topReversalPoint = reversalPoint - new Vector3(0, splitPoint * height, 0);
        bottomReversalPoints.Add(reversalPoint - new Vector3(widthMultiplier * 0.75f, 0, 0));
        bottomReversalPoints.Add(reversalPoint - new Vector3(widthMultiplier * 0.75f, (splitPoint - count + 1) * height, 0));
        bottomReversalPoints.Add(reversalPoint - new Vector3(0, (splitPoint - count + 1) * height, 0));

        this.height = height;
        //this.count = count;
    }

    public bool Animate(float speed)
    {
        bool moved = false;
        int counter = 0;
        foreach (var block in topBlocks)
        {
            if (topReversalPoint - new Vector3(0, counter, 0) == block.transform.position || !moveDown)
            {
                break;
            }
            var change = topReversalPoint - new Vector3(0, counter * height, 0) - block.transform.position;
            counter++;

            change = Vector3.ClampMagnitude(change, 0.0005f * speed);

            block.transform.position += change;
            moved = true;
        }

        counter = 0;
        foreach (var block in bottomBlocks)
        {
            if (bottomReversalPoints.Count == 0)
            {
                break;
            }
            var change = bottomReversalPoints[0] - new Vector3(0, (splitPoint - counter) * height, 0) - block.transform.position;
            if (change == Vector3.zero)
            {
                bottomReversalPoints.Remove(bottomReversalPoints[0]);
                moveDown = true;
                if (bottomReversalPoints.Count == 0)
                {
                    break;
                }
                change = bottomReversalPoints[0] - new Vector3(0, (splitPoint - counter) * height, 0) - block.transform.position;
            }
            counter++;

            var multiplier = 2;

            if (bottomReversalPoints.Count == 1)
            {
                multiplier = 1;
            }

            change = Vector3.ClampMagnitude(change, 0.0002f * speed * multiplier);

            block.transform.position += change;
            moved = true;
        }

        if (moved)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}