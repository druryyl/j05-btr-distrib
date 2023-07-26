using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace btr.nuna.Infrastructure
{
    public static class SqlBulkCopyHelper
    {
        public static void AddMap(this SqlBulkCopy bcp, string source, string target)
        {
            bcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(source, target));
        }

        public static void AddMap(this SqlBulkCopy bcp, string source)
        {
            var target = source;
            bcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(source, target));
        }

        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        
    }
}