using System;
using System.Linq;

namespace TradingBot.Core
{
    public class FixedRingBuffer<T>
    {
        private T[] _buffer;
        private int _maxSize;
        private int _count;

        public int Count => _count;

        public T[] Buffer => _buffer;

        public T GetLast() {
            if(_count == 0) {
                throw new IndexOutOfRangeException();
            }
            return _buffer[_count - 1];
        }

        public T GetLastNth(int index) {
            if(index >= _count) {
                throw new IndexOutOfRangeException();
            }
            return _buffer[_count - 1 - index];
        }
        public FixedRingBuffer(int maxSize) {
            _maxSize = maxSize;
            _buffer = new T[maxSize];
            _count = 0;
        }

        public void Push(T element) {
            if(_count == _maxSize) {
                for(int i = 1; i < _maxSize; i++) {
                    _buffer[i-1] = _buffer[i];
                }
                _count--;
            }
            _buffer[_count] = element;
            _count++;
        }


    }
}