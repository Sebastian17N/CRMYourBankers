using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;

namespace CRMYourBankers.Converters
{
	public  class FromCollectionToListCollectionViewWithDescriptionConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameters, CultureInfo culture)
        {
            if (value == null || parameters == null)
                return null;

            var parametersValues = parameters.ToString().Split(',');

            if (parametersValues.Length != 2)
                return null;

            ListCollectionView collectionView = new ListCollectionView((IList)value);
            collectionView.SortDescriptions.Add(new SortDescription(parametersValues[0], ListSortDirection.Descending));
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription(parametersValues[1]));
            
            return collectionView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
