using Socona.ToolBox.Extenstions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Socona.ToolBox.Graphs
{
    public class KuhnMunkrasMatching
    {

        private int _sizeX, _sizeY;
        private double[,] _weights;


        private double[] _labelX;
        private double[] _labelY;

        private bool[] _visitedX;
        private bool[] _visitedY;

        private int[] _match;

        public KuhnMunkrasMatching(int sizeX, int sizeY, double[,] weights = null)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _weights = new double[sizeX, sizeY];
            _labelX = new double[sizeX];
            _labelY = new double[sizeY];

            _visitedX = new bool[sizeX];
            _visitedY = new bool[sizeY];
            _match = new int[sizeY];
            if (weights != null)
            {
                Array.Copy(weights, _weights, sizeX * sizeY);
            }
        }
        private bool FindExtendedPath(int i)
        {

            _visitedX[i] = true;
            for (int j = 0; j < _sizeY; j++)
            {
                if (!_visitedY[j] && (_labelX[i] + _labelY[j] == _weights[i, j]))
                {
                    _visitedY[j] = true;
                    if (_match[j] == -1 || FindExtendedPath(_match[j]))
                    {
                        _match[j] = i;
                        return true;
                    }
                }
            }
            return false;
        }
        public int[] MaxWeightedMatch(out double sumWeight)
        {
            //初始化
            for (int i = 0; i < _sizeX; i++)
            {
                _visitedX[i] = false;
                _labelX[i] = int.MinValue;
                for (int j = 0; j < _sizeY; j++)
                {
                    if (_labelX[i] < _weights[i, j])
                    {
                        _labelX[i] = _weights[i, j];
                    }
                }
            }
            for (int j = 0; j < _sizeY; j++)
            {
                _labelY[j] = 0;
                _visitedY[j] = false;
            }
            _match.Fill(-1);

            //为每个点找匹配
            for (int i = 0; i < _sizeX; i++)
            {
                while (true)
                {
                    _visitedX.Fill(false);
                    _visitedY.Fill(false);

                    if (FindExtendedPath(i))
                    {
                        break;
                    }

                    double minDelta = double.MaxValue;
                    for (int u = 0; u < _sizeX; u++)
                    {
                        if (_visitedX[u])
                        {
                            for (int v = 0; v < _sizeY; v++)
                            {
                                double delta = _labelX[u] + _labelY[v] - _weights[u, v];
                                if (!_visitedY[v] && (delta < minDelta))
                                {
                                    minDelta = delta;
                                }
                            }
                        }
                    }

                    if (minDelta == double.MaxValue)
                    {                        
                        throw new Exception();
                    }

                    for (int u = 0; u < _sizeX; u++)
                    {
                        if (_visitedX[u])
                        {
                            _labelX[u] -= minDelta;
                        }
                    }
                    for (int v = 0; v < _sizeY; v++)
                    {
                        if (_visitedY[v])
                        {
                            _labelY[v] -= minDelta;
                        }
                    }
                }
            }

            var sum = 0.0;
            for (int j = 0; j < _sizeY; j++)
            {
                if (_match[j] >= 0)
                {
                    sum += _weights[_match[j], j];
                }
            }
            //sum是权重的和
            sumWeight = sum;
            int[] result = new int[_sizeY];
            Array.Copy(_match, result, _match.Length);
            return result;
        }
        public int[] MinWeightedMatch(out double sumWeight)
        {
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    _weights[i, j] = -_weights[i, j];
                }
            }

            var result = MaxWeightedMatch(out sumWeight);
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    _weights[i, j] = -_weights[i, j];
                }
            }
            sumWeight = -sumWeight;
            return result;
        }


    }
}
