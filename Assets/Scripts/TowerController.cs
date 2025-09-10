using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private UIController _controller;
    private SoundController _soundController;

    private GameObject blockTemplate;
    [SerializeField]
    private GameObject originalTemplate;

    [SerializeField]
    private int size = 5;

    private int defaultSize = 5;

    public List<GameObject> originalTower = new List<GameObject>();
    public List<GameObject> changedTower = new List<GameObject>();

    public bool reverseDown = true;
    public bool rotateDown = true;

    public List<Color> colorList;
    public List<Material> materialList;

    private int selectedBlock = -1;
    public HighlightedBlocks highlightedBlocks = HighlightedBlocks.None;

    private int selectedLevel = -1;

    private List<Animation> animations = new List<Animation>();

    private float widthMultiplier = 2.0f;

    private float heightMultiplier = 2.0f;

    private float center = 0.0f;

    private float gameTime = 0.0f;

    private float minGameTime = 0.0f;

    private float endOfTheGameTimer = 0.0f;

    private int currentStars = 3;

    private int minMoves = 1;

    private int currentLevel = 0;

    private int _totalMoves = 0;
    private int totalMoves
    {
        get
        {
            return _totalMoves;
        }

        set
        {
            _totalMoves = value;
            //_controller.  (_totalMoves);
        }
    }


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

    public bool tutorialOn = false;
    public TutorialController tutorialController;

    void Start()
    {
        _controller = GameObject.Find("UI").GetComponent<UIController>();
        _soundController = GameObject.Find("SoundController (1)").GetComponent<SoundController>();
        Debug.Log("Pre load");
        SaveController.LoadLevel();
        Debug.Log("Post load");
        tutorialController = new TutorialController(_controller);

        //LoadTower();
        //GenerateTower();
    }

    // Update is called once per frame

    void HighlightTutorialBlock()
    {
        var blockIndex = tutorialController.GetCurrentTutorialBlock();

        if (blockIndex == -1)
        {
            return;
        }

        var block = changedTower[blockIndex];

        var tempBlockMeshRenderer = block.transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>();
        tempBlockMeshRenderer.materials = new Material[2] { tempBlockMeshRenderer.material, materialList[3] };
    }
    void FixedUpdate()
    {
        if (animations.Count > 0)
        {
            if (animations[0].Animate(500))
            {
                animations.Remove(animations[0]);
                _soundController.PlayCombine();
                //Reverse(1);
            }
        }

        foreach (var block in changedTower)
        {
            var tempBlockMeshRenderer = block.transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>();
            tempBlockMeshRenderer.materials = new Material[2] { tempBlockMeshRenderer.material,
                    blockTemplate.transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().materials[1] };
        }

        if (tutorialOn)
        {
            HighlightTutorialBlock();
        }

        if (selectedBlock >= 0)
        {
            var tempBlockMeshRenderer = changedTower[selectedBlock].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>();
            tempBlockMeshRenderer.materials = new Material[2] { tempBlockMeshRenderer.material, materialList[4] };

            if (highlightedBlocks == HighlightedBlocks.Top)
            {
                for (int i = selectedBlock + 1; i < changedTower.Count; i++)
                {
                    tempBlockMeshRenderer = changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>();
                    tempBlockMeshRenderer.materials = new Material[2] { tempBlockMeshRenderer.material, materialList[4] };
                }
            }
            else if (highlightedBlocks == HighlightedBlocks.Bottom)
            {
                for (int i = selectedBlock - 1; i >= 0; i--)
                {
                    tempBlockMeshRenderer = changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>();

                    tempBlockMeshRenderer.materials = new Material[2] { tempBlockMeshRenderer.material, materialList[4] };
                }
            }
        }

        if (endOfTheGameTimer > 0)
        {
            endOfTheGameTimer -= Time.fixedDeltaTime;
        }
        else
        {
            SetupStars();
            gameTime += Time.fixedDeltaTime;
            _controller.UpdateTime(gameTime);
        }

        if (endOfTheGameTimer < 0)
        {
            Win();
        }
    }

    private void SetupStars()
    {
        int stars = 0;

        if (gameTime < minGameTime)
        {
            stars++;
        }

        if (totalMoves / minMoves < 1)
        {
            stars += 2;
        }
        else if (totalMoves / minMoves < 2)
        {
            stars += 1;
        }

        if (currentStars != stars)
        {
            currentStars = stars;
            _controller.SetStars(stars);
        }
    }

    private void Win()
    {
        ClearTower();

        endOfTheGameTimer = 0;
        SaveController.UpdateLevelStars(currentLevel, currentStars);
        _controller.UpdatePlayerStars(currentStars);

        _controller.CheckNextLevelButton(currentLevel);

        _controller.EndGame();

        _controller.ChangeMenu(8);
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

        endOfTheGameTimer = 3.0f;
    }

    public void ClearTower()
    {
        tutorialOn = false;
        foreach (var block in originalTower)
        {
            Destroy(block.gameObject);
        }

        foreach (var block in changedTower)
        {
            Destroy(block.gameObject);
        }

        animations.Clear();
        originalTower.Clear();
        changedTower.Clear();

        selectedBlock = -1;
        _totalMoves = 0;
        gameTime = 0;
    }

    public void RotateStatic(int splitPoint)
    {
        for (int i = 0; i <= splitPoint / 2; i++)
        {
            (changedTower[splitPoint - i], changedTower[i]) = (changedTower[i], changedTower[splitPoint - i]);

            changedTower[i].transform.position = new Vector3(widthMultiplier * Width * 0, i * Height, 0);
            changedTower[splitPoint - i].transform.position = new Vector3(widthMultiplier * Width * 0, (splitPoint - i) * Height, 0);
        }
    }

    public void RotateDownStatic(int splitPoint)
    {
        for (int i = splitPoint; i < (changedTower.Count - splitPoint + 1) / 2 + splitPoint; i++)
        {
            (changedTower[changedTower.Count - 1 + splitPoint - i], changedTower[i])
                = (changedTower[i], changedTower[changedTower.Count - 1 + splitPoint - i]);

            changedTower[i].transform.position = new Vector3(widthMultiplier * Width * 0, i * Height, 0);
            changedTower[changedTower.Count - 1 + splitPoint - i].transform.position
                = new Vector3(widthMultiplier * Width * 0, (changedTower.Count - 1 + splitPoint - i) * Height, 0);
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
            changedTower[i].transform.position = new Vector3(widthMultiplier * Width * 0, i * Height, 0);
        }
    }
    public void Rotate(int splitPoint)
    {
        if (CheckTutorialMoveBlock(true, rotateDown))
        {
            return;
        }

        if (splitPoint == 0)
        {
            if (selectedBlock != -1 && selectedBlock != changedTower.Count)
            {
                splitPoint = selectedBlock;
            }
            else
            {
                return;
            }
        }

        totalMoves++;

        _soundController.PlaySplit();

        Vector3 rotationPoint;
        BlockRotation newAnimation;
        if (rotateDown)
        {
            rotationPoint = new Vector3(widthMultiplier * Width * 0, (float)splitPoint / 2 * Height + center, 0);
            newAnimation = new BlockRotation(rotationPoint, true);

            for (int i = 0; i <= splitPoint / 2; i++)
            {
                (changedTower[splitPoint - i], changedTower[i]) = (changedTower[i], changedTower[splitPoint - i]);
            }

            for (int i = 0; i <= splitPoint; i++)
            {
                newAnimation.animatedBlocks.Add(changedTower[i]);
            }
        }
        else
        {
            rotationPoint = new Vector3(widthMultiplier * Width * 0,
                (((float)splitPoint + changedTower.Count - 1) / 2) * Height + center, 0);

            newAnimation = new BlockRotation(rotationPoint, false);

            for (int i = splitPoint; i < (changedTower.Count - splitPoint + 1) / 2 + splitPoint; i++)
            {
                (changedTower[changedTower.Count - 1 + splitPoint - i], changedTower[i])
                    = (changedTower[i], changedTower[changedTower.Count - 1 + splitPoint - i]);
            }

            for (int i = splitPoint; i < changedTower.Count; i++)
            {
                newAnimation.animatedBlocks.Add(changedTower[i]);
            }
        }


        animations.Add(newAnimation);
        CheckVictory();

        selectedBlock = -1;
    }

    public void Reverse(int splitPoint)
    {
        if (CheckTutorialMoveBlock(false, reverseDown))
        {
            return;
        }

        if (splitPoint == 0)
        {
            if (selectedBlock != -1)
            {
                splitPoint = selectedBlock;
                if (!reverseDown)
                {
                    splitPoint -= 1;
                }
                else if (selectedBlock == changedTower.Count - 1)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        totalMoves++;

        _soundController.PlaySplit();

        Vector3 reversalPoint = new Vector3(widthMultiplier * Width * 0, (float)splitPoint * Height + center, 0);
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

        selectedBlock = -1;
    }

    public bool SelectPoint(Vector3 point)
    {
        if (point.x > widthMultiplier * 0 - Width && point.x < widthMultiplier * 0 + Width)
        {
            for (int i = 0; i <= size; i++)
            {
                if (point.y > (0.5 + (i - 1)) * Height && point.y < (0.5 + i) * Height)
                {
                    selectedBlock = i;
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetPoint()
    {
        selectedBlock = -1;
    }

    public void GenerateTower()
    {
        _controller.NewGame();

        selectedLevel = -1;
        totalMoves = 0;
        gameTime = 0;

        blockTemplate = GameObject.Instantiate(originalTemplate);

        blockTemplate.transform.position = new Vector3(0, 0, -100);

        float modifier = (float)defaultSize / 7.0f;

        blockTemplate.transform.localScale = new Vector3(blockTemplate.transform.localScale.x,
            blockTemplate.transform.localScale.y * modifier, blockTemplate.transform.localScale.z);

        size = 7;

        for (int i = 0; i < size; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            var material = Random.Range(0, materialList.Count);

            originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            originalTower[i].transform.position = new Vector3(-widthMultiplier * Width * 1.5f, i * Height, 3 + 0.01f * i);

            originalTower[i].transform.parent = transform;

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            changedTower[i].transform.position = new Vector3(0, i * Height, 0.01f * i);

            changedTower[i].transform.parent = transform;

        }

        RandomizeTower();
    }

    public void RandomizeTower()
    {
        var timesRandomized = Random.Range(4, 8);

        minMoves = timesRandomized;
        minGameTime = minMoves * 7;

        for (int i = 0; i < timesRandomized; i++)
        {
            var split = Random.Range(1, size - 1);
            var reverse = Random.Range(0, 1);

            if (reverse == 1)
            {
                ReverseStatic(split);
                Debug.Log("Reverse: " + split);
            }
            else
            {
                RotateStatic(split);
                Debug.Log("Rotate: " + split);
            }
        }

    }

    public void StartTutorial()
    {
        tutorialController = new TutorialController(_controller);

        tutorialController.ChangeTutorialDescription();

        tutorialOn = true;

        GenerateTutorial();
    }

    private void GenerateTutorial()
    {
        _controller.NewGame();

        selectedLevel = 0;
        totalMoves = 0;
        gameTime = 0;

        blockTemplate = GameObject.Instantiate(originalTemplate);

        blockTemplate.transform.position = new Vector3(0, 0, -100);

        originalTower = new List<GameObject>();

        var materials = new List<int>() { 0, 0, 1, 1, 0, 1 };

        size = materials.Count;

        var modifier = (float)defaultSize / size;

        blockTemplate.transform.localScale = new Vector3(blockTemplate.transform.localScale.x,
            blockTemplate.transform.localScale.y * modifier, blockTemplate.transform.localScale.z);

        for (int i = 0; i < size; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            var material = materials[i];

            originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            originalTower[i].transform.position = new Vector3(-widthMultiplier * Width * 1.5f, i * Height, 3 + 0.01f * i);

            originalTower[i].transform.parent = transform;

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[material];

            changedTower[i].transform.position = new Vector3(0, i * Height, 0.01f * i);

            changedTower[i].transform.parent = transform;

        }

        ReverseStatic(3);
        RotateStatic(2);

        minMoves = 11;
        minGameTime = minMoves * 7;
    }

    public bool CheckTutorialMoveBlock(bool rotate, bool backwards)
    {
        if (tutorialOn)
        {
            return !tutorialController.CheckMoveValidity(rotate, backwards, selectedBlock);
        }
        return false;
    }

    public void LoadLevel(int level)
    {
        currentLevel = level;
        LoadTower();
    }

    public void NextLevel()
    {
        if (selectedLevel != -1)
        {
            currentLevel++;
            LoadTower();
        }
        else
        {
            GenerateTower();
        }
    }

    public void PreviousLevel()
    {
        if (selectedLevel != -1)
        {
            if (currentLevel > 0)
            {
                currentLevel--;
            }
            LoadTower();
        }
        else
        {
            GenerateTower();
        }
    }

    public void LoadTower()
    {
        _controller.NewGame();

        selectedLevel = 0;
        totalMoves = 0;
        gameTime = 0;

        blockTemplate = GameObject.Instantiate(originalTemplate);

        blockTemplate.transform.position = new Vector3(0, 0, -100);

        originalTower = new List<GameObject>();

        if (SaveController.leveleMateuszka.Count <= currentLevel)
        {
            currentLevel = SaveController.leveleMateuszka.Count - 1;
        }

        var loadedTower = SaveController.leveleMateuszka[currentLevel];

        size = loadedTower.colors.Count;

        var modifier = (float)defaultSize / loadedTower.colors.Count;

        blockTemplate.transform.localScale = new Vector3(blockTemplate.transform.localScale.x,
            blockTemplate.transform.localScale.y * modifier, blockTemplate.transform.localScale.z);

        for (int i = 0; i < loadedTower.colors.Count; i++)
        {
            originalTower.Add(Instantiate(blockTemplate));

            originalTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[loadedTower.colors[i]];
            originalTower[i].transform.position = new Vector3(-widthMultiplier * Width * 1.5f, i * Height, 3);

            originalTower[i].transform.parent = transform;

            //originalBlocks.Add(new TowerBlock { blockPosition = i, color = colorList[loadedTower.colors[i], targetWorldPosition = originalTower[i].transform.position });

            changedTower.Add(Instantiate(blockTemplate));

            changedTower[i].transform.Find("TrueCenter/Cube.001").GetComponent<MeshRenderer>().material
                = materialList[loadedTower.colors[i]];
            changedTower[i].transform.position = new Vector3(widthMultiplier * Width * 0, i * Height, 0.001f * i);

            changedTower[i].transform.parent = transform;

            //changedBlocks.Add(new TowerBlock { blockPosition = i, color = color, targetWorldPosition = changedTower[i].transform.position });
        }

        foreach (var transformation in loadedTower.transforms)
        {
            if (transformation.rotate)
            {
                if (transformation.normal)
                {
                    RotateStatic(transformation.splitPoint);
                }
                else
                {
                    RotateDownStatic(transformation.splitPoint);
                }
            }
            else
            {
                ReverseStatic(transformation.splitPoint);
            }
        }

        minMoves = loadedTower.transforms.Count;
        minGameTime = minMoves * 7;
    }

    public void RestartLevel()
    {
        ClearTower();
        if (selectedLevel == -1)
        {
            GenerateTower();
        }
        else
        {
            LoadTower();
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
    public bool rotateDown = true;

    public BlockRotation(Vector3 rotationPoint, bool rotateDown)
    {
        this.rotationPoint = rotationPoint;
        this.rotateDown = rotateDown;
    }

    public bool Animate(float speed)
    {
        maxRotation += speed * Time.deltaTime;

        float correction = 0;

        if (maxRotation > 180)
        {
            correction = maxRotation - 180;
        }

        if (rotateDown)
        {
            foreach (var block in animatedBlocks)
            {
                block.transform.RotateAround(rotationPoint, Vector3.forward, speed * Time.deltaTime - correction);
            }
        }
        else
        {
            foreach (var block in animatedBlocks)
            {
                block.transform.RotateAround(rotationPoint, Vector3.forward, speed * Time.deltaTime - correction);
            }
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
    public List<Vector3> topReversalPoints = new List<Vector3>();

    int splitPoint;

    bool moveDown = false;
    float height = 0;

    public BlockReversal(Vector3 reversalPoint, int splitPoint, int count, float widthMultiplier, float height)
    {
        this.splitPoint = splitPoint;
        topReversalPoint = reversalPoint - new Vector3(0, splitPoint * height, 0);

        topReversalPoints.Add(reversalPoint - new Vector3(-widthMultiplier * 0.75f, 0, 0));
        topReversalPoints.Add(reversalPoint - new Vector3(-widthMultiplier * 0.75f, splitPoint * height, 0));
        topReversalPoints.Add(reversalPoint - new Vector3(0, splitPoint * height, 0));

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
        //foreach (var block in topBlocks)
        //{
        //    if (topReversalPoint - new Vector3(0, counter, 0) == block.transform.position || !moveDown)
        //    {
        //        break;
        //    }
        //    var change = topReversalPoint - new Vector3(0, counter * height, 0) - block.transform.position;
        //    counter++;

        //    change = Vector3.ClampMagnitude(change, 0.0005f * speed);

        //    block.transform.position += change;
        //    moved = true;
        //}

        counter = 0;

        foreach (var block in topBlocks)
        {
            if (topReversalPoints.Count == 0)
            {
                break;
            }
            var change = topReversalPoints[0] + new Vector3(0, (counter) * height, 0) - block.transform.position;
            if (change == Vector3.zero)
            {
                topReversalPoints.Remove(topReversalPoints[0]);
                moveDown = true;
                if (topReversalPoints.Count == 0)
                {
                    break;
                }
                change = topReversalPoints[0] + new Vector3(0, (counter) * height, 0) - block.transform.position;
            }
            counter++;

            var multiplier = 2;

            if (topReversalPoints.Count == 1)
            {
                multiplier = 1;
            }

            change = Vector3.ClampMagnitude(change, 0.0002f * speed * multiplier);

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

public enum HighlightedBlocks
{
    None, Top, Bottom
}