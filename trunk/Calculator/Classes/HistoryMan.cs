using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Calculator.Classes
{
    internal class HistoryMan
    {
        private int _index;
        private ObservableCollection<string> _items;

        public HistoryMan()
        {
            _items = new ObservableCollection<string>();
            _index = 0;
        }

        public void AddItem(string data)
        {
            if (string.IsNullOrEmpty(data)) return;
            int itemindex = _items.IndexOf(data);
            if (itemindex >= 0) _items.RemoveAt(itemindex);
            _items.Add(data);
            SetIndex(0);
        }

        private void SetIndex(int value)
        {
            if (value == 0)
            {
                _index = _items.Count;
                return;
            }
            _index += value;
            if (_index < 0) _index = 0;
            if (_index > _items.Count - 1) _index = _items.Count - 1;
        }

        public string Previous()
        {
            SetIndex(-1);
            return _items[_index];
        }

        public string Next()
        {
            SetIndex(+1);
            return _items[_index];
        }

        public ObservableCollection<string> ItemBind
        {
            get { return _items; }
        }
    }
}
