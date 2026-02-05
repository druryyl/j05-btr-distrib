using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Shared
{
    using Microsoft.Win32;
    using System;

    public class RegistryHelper
    {
        private const string BasePath = @"DrurySoftware\BTRApp";

        // Read string value
        public string ReadString(string valueName, string defaultValue = "")
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(BasePath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        return value?.ToString() ?? defaultValue;
                    }
                }
            }
            catch (Exception)
            {
                // Log error if needed
            }
            return defaultValue;
        }

        // Write string value
        public void WriteString(string valueName, string value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(BasePath))
                {
                    key.SetValue(valueName, value, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to write to registry", ex);
            }
        }

        // Read integer value
        public int ReadInt(string valueName, int defaultValue = 0)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(BasePath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        if (value != null)
                        {
                            return Convert.ToInt32(value);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Log error if needed
            }
            return defaultValue;
        }

        // Write integer value
        public void WriteInt(string valueName, int value)
        {
            WriteString(valueName, value.ToString());
        }

        // Read boolean value
        public bool ReadBool(string valueName, bool defaultValue = false)
        {
            int intValue = ReadInt(valueName, defaultValue ? 1 : 0);
            return intValue != 0;
        }

        // Write boolean value
        public void WriteBool(string valueName, bool value)
        {
            WriteInt(valueName, value ? 1 : 0);
        }

        // Delete a value
        public void DeleteValue(string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(BasePath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(valueName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete registry value", ex);
            }
        }

        // Check if value exists
        public bool ValueExists(string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(BasePath))
                {
                    if (key != null)
                    {
                        return key.GetValue(valueName) != null;
                    }
                }
            }
            catch (Exception)
            {
                // Log error if needed
            }
            return false;
        }
    }
}
