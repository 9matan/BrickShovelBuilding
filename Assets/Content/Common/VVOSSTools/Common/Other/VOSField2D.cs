using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface IVOSField2D<TElement> 
	where TElement : class
{
	int fieldRows		{ get; }
	int fieldColumns	{ get; }

	TElement	this[int row, int column] { get; set; }

	void Resize(int rows, int columns);

	TElement	GetElement(int row, int column);
	void		SetElement(TElement element, int row, int column);
}

public class VOSField2D<TElement> : 
	IVOSField2D<TElement>
	where TElement : class
{

	public int fieldRows
	{
		get
		{
			if (_matrix == null)
				return 0;
			return _matrix.GetLength(0);
		}
	}
	public int fieldColumns
	{
		get
		{
			if (_matrix == null)
				return 0;
			return _matrix.GetLength(1);
		}
	}

	public TElement this[int row, int column]
	{
		get { return GetElement(row, column); }
		set { SetElement(value, row, column); }
	}

	protected TElement[,] _matrix
	{
		get;
		private set;
	}


	public VOSField2D()
	{
		Resize(0, 0);
	}

	public VOSField2D(int __rows, int __columns)
	{
		Resize(__rows, __columns);
	}



	public virtual void Resize(int rows, int columns)
	{
		_matrix = new TElement[rows, columns];
	}

	public TElement GetElement(int row, int column)
	{
		return _matrix[row, column];
	}

	public void SetElement(TElement element, int row, int column)
	{
		_matrix[row, column] = element;
	}


	public interface IRow
	{
		int count { get; }
		TElement this[int index] { get; set; }
		void Add(TElement cell);
	}

	[System.Serializable]
	public class Row : IRow
	{
		public int count
		{
			get { return _cells.Count; }
		}

		public TElement this[int index]
		{
			get { return _cells[index]; }
			set { _cells[index] = value; }
		}

		[SerializeField]
		protected List<TElement> _cells = new List<TElement>();

		public void Add(TElement cell)
		{
			_cells.Add(cell);
		}
	}
	
}

[System.Serializable]
public class VOSSerializedField2D<TElement, TRow> : VOSField2D<TElement>,
	IVOSField2D<TElement>,
	ISerializationCallbackReceiver
	where TElement : class
	where TRow : class, VOSField2D<TElement>.IRow, new()
{
	[SerializeField]
	protected List<TRow> _rows = new List<TRow>();
	protected int _serializeRowsCount
	{
		get { return _rows.Count; }
	}
	protected int _serializeColumnsCount
	{
		get
		{
			if (_rows.Count != 0)
				return _rows[0].count;
			return 0;
		}
	}

	public VOSSerializedField2D() : base()
	{
//		_field = new VOSField2D<TElement>();
	}

	public VOSSerializedField2D(int rows, int columns) : base(rows, columns)
	{
//		_field = new VOSField2D<TElement>(rows, columns);
	}

/*	public TElement this[int r, int c]
	{
		get { return _field[r, c]; }
		set { _field[r, c] = value; }
	}

	public int fieldColumns
	{
		get { return _field.fieldColumns; }
	}

	public int fieldRows
	{
		get { return _field.fieldRows; }
	}

	public TElement GetElement(int row, int column)
	{
		return _field.GetElement(row, column);
	}

	public virtual void Resize(int rows, int columns)
	{
		 _field.Resize(rows, columns);
	}

	public void SetElement(TElement element, int rows, int columns)
	{
		_field.SetElement(element, rows, columns);
	}
	*/

	//
	// < Serialization >
	//

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		Resize(_serializeRowsCount, _serializeColumnsCount);

		for (int i = 0; i < fieldRows; ++i)
		{
			for (int j = 0; j < fieldColumns; ++j)
			{
				this[i, j] = _rows[i][j];
			}
		}
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		_rows.Clear();

		for (int i = 0; i < fieldRows; ++i)
		{
			var row = new TRow();
			for (int j = 0; j < fieldColumns; ++j)
				row.Add(this[i, j]);
			_rows.Add(row);
		}
	}

	//
	// </ Serialization >
	//

}


/*
//
// < Serialization >
//

[System.Serializable]
public abstract class VOSField2DSerializer<TElement, TRow, TRows> :
	ISerializationCallbackReceiver
	where TElement : class
	where TRow : VOSField2DSerializeRow<TElement>, new()
	where TRows : VOSField2DSerializeRows<TElement, TRow>, new()
{

	protected abstract IVOSField2D<TElement> _field { get; }

	[SerializeField]
	protected TRows _rows = new TRows();

	
}
	

[System.Serializable]
public class VOSField2DSerializeRows<TElement, TRow>
	where TRow : VOSField2DSerializeRow<TElement>
{
	[SerializeField]
	protected List<TRow> _rows = new List<TRow>();

	public TRow this[int index]
	{
		get { return _rows[index]; }
		set { _rows[index] = value; }
	}

	public int rowsCount
	{
		get { return _rows.Count; }
	}

	public int cellsCount
	{
		get
		{
			if (_rows.Count != 0)
				return _rows[0].count;
			return 0;
		}
	}

	public void Add(TRow row)
	{
		_rows.Add(row);
	}

	public void Clear()
	{
		_rows.Clear();
	}
}

[System.Serializable]
public class VOSField2DSerializeRow<TElement>
{
	public int count
	{
		get { return _cells.Count; }
	}

	public TElement this[int index]
	{
		get { return _cells[index]; }
		set { _cells[index] = value; }
	}

	[SerializeField]
	protected List<TElement> _cells = new List<TElement>();

	public void Add(TElement cell)
	{
		_cells.Add(cell);
	}
}

//
// </ Serialization >
//
*/
