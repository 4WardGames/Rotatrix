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


    public List<Color> colorList;

    private int selectedBlock = -1;

    void Start()
    {
        GenerateTower();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < changedBlocks.Count; i++)
        {
            if (changedBlocks[i].targetWorldPosition != changedTower[i].transform.position)
            {
                var change = changedBlocks[i].targetWorldPosition - changedTower[i].transform.position;

                change = Vector3.ClampMagnitude(change, 0.05f);

                changedTower[i].transform.position += change;
            }
        }
    }

    private void CheckVictory()
    {
        for (int i = 0; i < originalTower.Count; i++)
        {
            if (originalTower[i].GetComponent<Renderer>().material.color != changedTower[i].GetComponent<Renderer>().material.color)
            {
                return;
            }
        }

        Debug.Log("Jest dobrze!");
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
        for (int i = 0; i <= splitPoint / 2; i++)
        {
            (changedTower[splitPoint - i], changedTower[i]) = (changedTower[i], changedTower[splitPoint - i]);
            changedBlocks[i].blockPosition = i;
            changedBlocks[i].targetWorldPosition = new Vector3(2, i * 1, 0);

            changedBlocks[splitPoint - i].blockPosition = splitPoint - i;
            changedBlocks[splitPoint - i].targetWorldPosition = new Vector3(2, (splitPoint - i) * 1, 0);
            //changedTower[i].transform.position = new Vector3(2, i * 1, 0);
            //changedTower[splitPoint - i].transform.position = new Vector3(2, (splitPoint - i) * 1, 0);
        }

        CheckVictory();
    }

    public void Reverse(int splitPoint)
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
        splitPoint++;
        List<GameObject> lastElements = changedTower.GetRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.RemoveRange(splitPoint, changedTower.Count - splitPoint);
        changedTower.InsertRange(0, lastElements);

        List<TowerBlock> lastBlocks = changedBlocks.GetRange(splitPoint, changedTower.Count - splitPoint);
        changedBlocks.RemoveRange(splitPoint, changedTower.Count - splitPoint);
        changedBlocks.InsertRange(0, lastBlocks);

        for (int i = 0; i < changedBlocks.Count; i++)
        {
            changedBlocks[i].blockPosition = i;
            changedBlocks[i].targetWorldPosition = new Vector3(2, i * 1, 0);
        }

        CheckVictory();
    }

    public void selectPoint(Vector3 point)
    {
        if (point.x > 1 && point.x < 3)
        {
            for (int i = 0; i < size; i++)
            {
                if (point.y > 0.5 + (i - 1) && point.y < 0.5 + i)
                {
                    selectedBlock = i;

                    Debug.Log(selectedBlock);
                    break;
                }
            }
        }
    }

    public void GenerateTower()
    {
        for (int i = 0; i < size; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            var color = Random.Range(0, colorList.Count);

            originalTower[i].GetComponent<Renderer>().material.color
                = colorList[color];

            originalTower[i].transform.position = new Vector3(-2, i * 1, 0);

            originalTower[i].transform.parent = transform;

            originalBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = originalTower[i].transform.position });

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].GetComponent<Renderer>().material.color
                = colorList[color];

            changedTower[i].transform.position = new Vector3(2, i * 1, 0);

            changedTower[i].transform.parent = transform;

            changedBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = changedTower[i].transform.position });
        }
        //Rotate(2);
        Reverse(3);
    }
}

public class TowerBlock
{
    public int blockPosition;
    public int color;
    public Vector3 targetWorldPosition;
}
