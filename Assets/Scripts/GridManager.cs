using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int _width = 24;
    private int _height = 10;
    private Transform[,] _grid;

    void Awake()
    {
        _grid = new Transform[_width, _height];    
    }

    public bool AddBlockToGrid(Transform block) {
        Vector2Int gridPos = GetGridPosition(block.position);
        
        if (gridPos.x < 0 || gridPos.x >= _width || gridPos.y < 0 || gridPos.y >= _height) {
            return false;
        }

        _grid[gridPos.x, gridPos.y] = block;
        return true;
    }

    public bool IsLineFull(int y) {
        for (int x = 0; x < _width; x++) {
            if (_grid[x, y] == null) {
                return false;
            }
        }

        return true;
    }

    public void ClearLine(int y) {
        for (int x = 0; x < _width; x++) {
            Destroy(_grid[x, y].gameObject);
            _grid[x, y] = null;
        }

        MoveLinesDown(y);
    }

    private void MoveLinesDown(int clearedY) {
        for (int y = clearedY; y < _height - 1; y++) {
            for (int x = 0; x < _width; x++) {
                if (_grid[x, y + 1] != null) {
                    _grid[x, y] = _grid[x, y + 1];
                    _grid[x, y + 1] = null;
                    _grid[x, y].position += Vector3.down;
                }
            }
        }
    }

    private Vector2Int GetGridPosition(Vector3 worldPosition) {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
    }
}
