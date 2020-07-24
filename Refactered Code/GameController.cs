using System;
using System.Collections;
using Data;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Domain
{
    public class GameController : MonoBehaviour
    {
        public BlockData[] block;
        public GameObject canvas;
        public Text scoreText;

        private readonly System.Random _rand = new System.Random();
        private int _stackN;
        private int[] _stack;
        private BlockData _tmp;
        private int[] _down;
        //private AudioManager _audioManager;
        private int _currentBlock;

        private void Start()
        {
            //_audioManager = FindObjectOfType<AudioManager>();
            BlockHandler.Initialize(canvas);
            block = new BlockData[35];
            _stack = new int[35];
            _down = new int[35];
            _stackN = 0;
            ScoreData.currentScore = 0;

            for (var i = 0; i < 5; ++i)
            for (var j = 0; j < 7; ++j)
            {
                var x = i * 7 + j;
                block[x] = BlockHandler.CreateBlock();
                BlockHandler.SetPosition(block[x], i, j + 4);
                BlockHandler.SetNumber(block[x], Rand());
                BlockHandler.Move(block[x], i, j, 0.5f);
            }

            StartCoroutine(Play());
        }

        private IEnumerator Play()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (GameInput.isPressed)
                {
                    yield return FindMouseOnBlock();
                    if (_currentBlock == -1) continue;

                    if (RemoveBlockFromList()) continue;
                    if (block[_currentBlock].isClicked) continue;
                    AddBlockToList();
                }
                else
                {
                    if (_stackN == 1) BlockHandler.UndoClick(block[_stack[0]]);

                    if (_stackN > 1) yield return SolveList();
                    
                    _stackN = 0;
                    if (!CheckEndGame()) continue;

                    yield return new WaitForSeconds(1.5f);

                    for (var i = 0; i < 35; ++i)
                        BlockHandler.ShakePosition(block[i], 1.5f);
                    yield return new WaitForSeconds(2f);
                    Debug.Log("EndGame");

                    FindObjectOfType<GameScene>().ChangeScene();
                    break;
                }
            }
        }

        private IEnumerator FindMouseOnBlock()
        {
            _currentBlock = -1;

            for (var i = 0; i < 35; ++i)
            {
                var p = block[i].block.transform.position;
                if (!GameInput.IsMouseOnBlock(p.x, p.y)) continue;
                _currentBlock = i;
                break;
            }

            yield return null;
        }

        private bool RemoveBlockFromList()
        {
            if (_stackN <= 1 || _stack[_stackN - 2] != _currentBlock) return false;
            FindObjectOfType<AudioManager>().Play("Turn");
            BlockHandler.UndoClick(block[_stack[_stackN - 1]]);
            BlockHandler.UndoMatch(_stackN - 1, _stack[_stackN - 2], _stack[_stackN - 1], 0.1f);
            _stackN--;
            return true;
        }

        private void AddBlockToList()
        {
            var check = false;
            if (_stackN == 0) check = true;
            else
            {
                var topNumber = block[_stack[_stackN - 1]].number;
                var currentNumber = block[_currentBlock].number;
                if (_stackN == 1 && topNumber == currentNumber) check = true;
                if (_stackN > 1 && (topNumber == currentNumber || 2 * topNumber == currentNumber)) check = true;
                if (!CheckAdjacent(_currentBlock, _stack[_stackN - 1])) check = false;
            }

            if (!check) return;
            FindObjectOfType<AudioManager>().Play("Turn");
            _stack[_stackN] = _currentBlock;
            _stackN++;
            BlockHandler.Click(block[_currentBlock]);
            if (_stackN > 1)
                BlockHandler.Match(_stackN - 1, _stack[_stackN - 2], _stack[_stackN - 1], 0.1f);
        }

        private IEnumerator SolveList()
        {
            FindObjectOfType<AudioManager>().Play("Delete");
            for (var i = 1; i < _stackN; ++i)
            {
                BlockHandler.DestroyLine(i);
                var x = _stack[i] / 7;
                var y = _stack[i] % 7;
                for (var j = 0; j < i; ++j)
                {
                    BlockHandler.Move(block[_stack[j]], x, y, 0.08f);
                }

                yield return new WaitForSeconds(0.08f);
            }

            var sum = 0;
            for (var i = 0; i < _stackN; ++i)
                sum += block[_stack[i]].number;
            AddScore(sum);

            var s = 2;
            var k = 0;
            while (s <= sum) s *= 2;
            s /= 2;

            BlockHandler.UndoClick(block[_stack[_stackN - 1]]);

            for (var i = 0; i < 5; ++i)
            {
                var p = 0;
                var c = 0;

                for (var j = 0; j < 7; ++j)
                {
                    var x = i * 7 + j;
                    var y = i * 7 + p;

                    if (!block[x].isClicked) 
                    {
                        _tmp = block[x];
                        block[x] = block[y];
                        block[y] = _tmp;

                        _down[y] = c;
                        p++;
                        if (x == _stack[_stackN - 1]) k = y;
                    }
                    else
                    {
                        BlockHandler.UndoClick(block[x]);
                        c++;
                    }
                }

                for (var j = p; j < 7; ++j)
                {
                    var x = i * 7 + j;
                    BlockHandler.SetNumber(block[x], Rand());
                    BlockHandler.SetPosition(block[x], i, j + c);
                    _down[x] = c;
                }
            }

            BlockHandler.ChangeNumber(block[k], s);
            for (var i = 0; i < 35; ++i)
                BlockHandler.Move(block[i], i / 7, i % 7, 0.6f);
            yield return null;
        }

        private int Rand()
        {
            var x = _rand.Next(1, 5); //isToggleOn ? _rand.Next(4, 11) : _rand.Next(1, 5);
            return (int) Math.Pow(2, x);
        }
        
        private static bool CheckAdjacent(int x,int y) {
            return Math.Abs(x / 7 - y / 7) + Math.Abs(x % 7 - y % 7) == 1;
        }

        private void AddScore(int score)
        {
            StartCoroutine(GameUiChanger.CoChangeNumber(scoreText, ScoreData.currentScore,
                ScoreData.currentScore + score));
            ScoreData.currentScore += score;
        }

        private bool CheckEndGame()
        {
            int[] s1 = {-1, 0, 1, 0};
            int[] s2 = {0, 1, 0, -1};
            
            for (var i = 0; i < 5; ++i)
            for (var j = 0; j < 7; ++j)
            {
                var b = i * 7 + j;
                for (var k = 0; k < 4; ++k)
                {
                    var x = i + s1[k];
                    var y = j + s2[k];
                    var z = x * 7 + y;
                    if (x < 0 || x >= 5) continue;
                    if (y < 0 || y >= 7) continue;
                    if (block[b].number == block[z].number) return false;
                }
            }

            return true;
        }
    }
}
