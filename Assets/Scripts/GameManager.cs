using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _blockPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ScoreManager _scoreManager;

    private GameObject activeBlock;
    private bool _isGameActive = true;

    void Start()
    {
        StartGame();    
    }

    void StartGame()
    {
        SpawnBlock(0); // Player 1
    }

    public void SpawnBlock(int playerIndex) {
        if (!_isGameActive) {
            return;
        }

        if (activeBlock != null) {
            return;
        }

        int randomIndex = Random.Range(0, _blockPrefabs.Length);
        activeBlock = Instantiate(_blockPrefabs[randomIndex], _spawnPoints[playerIndex].position, Quaternion.identity);
        BlockController blockController = activeBlock.GetComponent<BlockController>();
        blockController.enabled = true;
    }

    public void OnBlockLanded() {
        activeBlock = null;

        SpawnBlock(0); // Player 1
        SpawnBlock(1); // Player 2
    }

    public void LineCleared(int playerIndex) {
        _scoreManager.AddScore(playerIndex, 100);
    }

    public void CheckForLines() {
        for (int y = 0; y < 20; y++) {
            if (_gridManager.IsLineFull(y)) {
                _gridManager.ClearLine(y);
                LineCleared(0); // Player 1 score - temporary
            }
        }
    }

    public void GameOver(int playerIndex) {
        _isGameActive = false;
        Debug.Log($"Game Over! Player {playerIndex + 1} wins!");
    }
}
