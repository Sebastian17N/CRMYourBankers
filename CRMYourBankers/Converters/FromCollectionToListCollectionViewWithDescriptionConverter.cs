using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;

namespace CRMYourBankers.Converters
{
	public  class FromCollectionToListCollectionViewWithDescriptionConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            ListCollectionView collectionView = new ListCollectionView((IList)value);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription(parameter.ToString()));
            collectionView.SortDescriptions.Add(new SortDescription(parameter.ToString(), ListSortDirection.Descending));

            return collectionView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
