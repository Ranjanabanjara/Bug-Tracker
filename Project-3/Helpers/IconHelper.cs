using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Project_3.Helpers
{
    public class IconHelper
    {
        public static string GetIcon(string filename)
        {
            var keyValue = WebConfigurationManager.AppSettings[Path.GetExtension(filename)];
            var defaultValue = WebConfigurationManager.AppSettings["DefaultIcon"];
            return string.IsNullOrEmpty(keyValue) ? defaultValue : keyValue;


        }
    }
}