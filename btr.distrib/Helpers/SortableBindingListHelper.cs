using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public class SortableBindingList<T> : BindingList<T>
{
    private bool _isSorted;
    private ListSortDirection _sortDirection;
    private PropertyDescriptor _sortProperty;

    protected override bool SupportsSortingCore => true;
    protected override bool IsSortedCore => _isSorted;
    protected override PropertyDescriptor SortPropertyCore => _sortProperty;
    protected override ListSortDirection SortDirectionCore => _sortDirection;

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
        if (Items is List<T> list)
        {
            var comparer = new PropertyComparer<T>(prop, direction);
            list.Sort(comparer);

            _sortDirection = direction;
            _sortProperty = prop;
            _isSorted = true;

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        else
        {
            _isSorted = false;
        }
    }

    protected override void RemoveSortCore()
    {
        _isSorted = false;
    }
}


public class PropertyComparer<T> : IComparer<T>
{
    private readonly PropertyDescriptor _property;
    private readonly ListSortDirection _direction;

    public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
    {
        _property = property;
        _direction = direction;
    }

    public int Compare(T x, T y)
    {
        var valueX = _property.GetValue(x);
        var valueY = _property.GetValue(y);

        int result;
        if (valueX == null)
        {
            result = (valueY == null) ? 0 : -1;
        }
        else if (valueY == null)
        {
            result = 1;
        }
        else
        {
            result = Comparer<object>.Default.Compare(valueX, valueY);
        }

        return _direction == ListSortDirection.Ascending ? result : -result;
    }
}
