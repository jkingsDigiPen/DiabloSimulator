//------------------------------------------------------------------------------
//
// File Name:	ObservableDictionaryNoThrow.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using DrWPF.Windows.Data;

namespace DiabloSimulator
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class ObservableDictionaryNoThrow<TKey, TValue> :
        IDictionary<TKey, TValue>,
        INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public ObservableDictionaryNoThrow()
            : base()
        {
        }

        public ObservableDictionaryNoThrow(ObservableDictionaryNoThrow<TKey, TValue> other)
            : base()
        {
            dictionary = new ObservableDictionary<TKey, TValue>(other.dictionary);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (dictionary.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    return default(TValue);
                }
            }
            set
            {
                dictionary[key] = value;
                OnPropertyChanged("Item[]");
                OnPropertyChanged("Values");
            }
        }

        public void Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)dictionary).Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)dictionary).ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TValue>)dictionary).Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return ((IDictionary<TKey, TValue>)dictionary).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)dictionary).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<TKey, TValue>)dictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)dictionary).GetEnumerator();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)dictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)dictionary).Values;

        public int Count => ((IDictionary<TKey, TValue>)dictionary).Count;

        public bool IsReadOnly => ((IDictionary<TKey, TValue>)dictionary).IsReadOnly;

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)dictionary).GetEnumerator();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private ObservableDictionary<TKey, TValue> dictionary 
            = new ObservableDictionary<TKey, TValue>();
    }
}
