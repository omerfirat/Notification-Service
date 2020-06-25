using NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace NotificationService
{
    public static class Common
    {
        public static readonly object locked = new object();
        internal static string CreateParameterString(params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            string parameterText = string.Empty;
            int index = 1;
            string variableValue = null;
            foreach (var i in parameters)
            {
                if (i != null)
                {
                    if (i is string || i is DateTime)
                    {
                        variableValue = "'" + i.ToString() + "'";
                    }
                    else
                    {
                        variableValue = i.ToString();
                    }
                    sb.Append(index).Append(" --> ").Append(variableValue).Append(" # ");
                 
                }
                else
                {
                    sb.Append(index).Append(" --> ").Append("null").Append(" # ");
                }

                index++;
            }
            parameterText = sb.ToString();
            sb = null;
            return parameterText;
        }
    }

    

}
