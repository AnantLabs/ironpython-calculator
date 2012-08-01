using System;
using System.Threading.Tasks;
using IronPython.Runtime;

namespace Calculator.Mathematics
{
    public class Matrix
    {
        private object[] _data;


        public Matrix(int Rows, int Columns)
        {
            if (Rows < 1) throw new ArgumentException("must be greater than 0", "Rows");
            if (Columns < 1) throw new ArgumentException("must be greater than 0", "Columns");
            this.Rows = Rows;
            this.Columns = Columns;
            _data = new object[Rows * Columns];
        }

        public Matrix(int Rows, int Columns, object Defaultvalue): this(Rows, Columns)
        {
            Parallel.For(0, _data.Length, i =>
            {
                _data[i] = Defaultvalue;
            });
        }

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public object this[int Row, int Column]
        {
            get { return _data[Column * Rows + Row]; }
            set { _data[Column * Rows + Row] = value; }
        }

        public object this[int item]
        {
            get { return _data[item]; }
            set { _data[item] = value; }
        }

        public List GetRow(int Row)
        {
            if (Row < 0 || Row > Rows) throw new ArgumentOutOfRangeException("Row");
            List ret = new List();
            for (int i = 0; i < Columns; i++)
                ret.Add(this[Row, i]);
            return ret;
        }

        public List GetColumn(int Column)
        {
            if (Column < 0 || Column > Columns) throw new ArgumentOutOfRangeException("Column");
            List ret = new List();
            for (int i = 0; i < Rows; i++)
                ret.Add(this[i, Column]);
            return ret;
        }

        public void SetRow(int Row, List Values)
        {
            if (Row < 0 || Row > Rows) throw new ArgumentOutOfRangeException("Row");
            if (Values.Count < Columns) throw new ArgumentException("Values does not have enough data");
            for (int i = 0; i < Columns; i++)
                this[Row, i] = Values[i];
        }

        public void SetColumn(int Column, List Values)
        {
            if (Column < 0 || Column > Columns) throw new ArgumentOutOfRangeException("Column");
            if (Values.Count < Rows) throw new ArgumentException("Values does not have enough data");
            for (int i = 0; i < Rows; i++)
                this[i, Column] = Values[i];
        }

        public static Matrix operator + (Matrix Left, Matrix Right)
        {
            if (Left == null) throw new ArgumentNullException("Left");
            if (Right == null) throw new ArgumentNullException("Right");
            if (Left.Rows != Right.Rows || Left.Columns != Right.Columns) throw new Exception("Input matrix row/column count mismatch");
            Matrix ret = new Matrix(Left.Rows, Right.Columns);
            for (int i = 0; i < Left.Rows; i++)
            {
                Parallel.For(0, Right.Columns, j =>
                    {
                        dynamic tmp = (dynamic)Left[i, j] + (dynamic)Right[i, j];
                        ret[i, j] = (object)tmp;
                    });
            }
            return ret;
        }

        public static Matrix operator + (Matrix m, object item)
        {
            if (m == null) throw new ArgumentNullException("m");
            Matrix ret = new Matrix(m.Rows, m.Columns);
            Parallel.For(0, m._data.Length, i=>
            {
                ret._data[i] = (object)((dynamic)m._data[i] + (dynamic)item);
            });
            return ret;
        }

        public static Matrix operator -(Matrix Left, Matrix Right)
        {
            if (Left == null) throw new ArgumentNullException("Left");
            if (Right == null) throw new ArgumentNullException("Right");
            if (Left.Rows != Right.Rows || Left.Columns != Right.Columns) throw new Exception("Input matrix row/column count mismatch");
            Matrix ret = new Matrix(Left.Rows, Right.Columns);
            for (int i = 0; i < Left.Rows; i++)
            {
                Parallel.For(0, Right.Columns, j =>
                {
                    dynamic tmp = (dynamic)Left[i, j] - (dynamic)Right[i, j];
                    ret[i, j] = (object)tmp;
                });
            }
            return ret;
        }

        public static Matrix operator -(Matrix m, object item)
        {
            if (m == null) throw new ArgumentNullException("m");
            Matrix ret = new Matrix(m.Rows, m.Columns);
            Parallel.For(0, m._data.Length, i =>
            {
                ret._data[i] = (object)((dynamic)m._data[i] - (dynamic)item);
            });
            return ret;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Columns != right.Rows) throw new Exception("Input matrix row/column count mismatch");
            Matrix ret = new Matrix(left.Rows, right.Columns);
            for (int j = 0; j < right.Columns; ++j)
            {
                //for (int i = 0; i < left.Rows; ++i)
                Parallel.For(0, left.Rows, i =>
                {
                    dynamic s = 0.0;
                    for (int l = 0; l != left.Columns; l++)
                    {
                        s += (dynamic)left[i, l] * (dynamic)right[l, j];
                    }
                    ret[i, j] = (object)s;
                });
            }
            return ret;
        }

        public static Matrix operator *(Matrix m, object item)
        {
            if (m == null) throw new ArgumentNullException("m");
            Matrix ret = new Matrix(m.Rows, m.Columns);
            Parallel.For(0, m._data.Length, i =>
            {
                ret._data[i] = (object)((dynamic)m._data[i] * (dynamic)item);
            });
            return ret;
        }

        public static Matrix operator /(Matrix m, object item)
        {
            if (m == null) throw new ArgumentNullException("m");
            Matrix ret = new Matrix(m.Rows, m.Columns);
            Parallel.For(0, m._data.Length, i =>
            {
                ret._data[i] = (object)((dynamic)m._data[i] / (dynamic)item);
            });
            return ret;
        }
    }
}
