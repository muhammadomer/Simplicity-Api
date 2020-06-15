using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimplicityOnlineWebApi.Commons
{

    public static class ClassUtils
    {
        // Extension classes written by Faheem Salik

        #region extentions
        public static void CopyPropertyValues(this object destination, object source, String[] ignoreProperties = null)
        {
            if (source != null && destination != null)
            {
                //if (!(destination.GetType().Equals(source.GetType())))
                //    throw new ArgumentException("Type mismatch");

                if (destination is IEnumerable)
                {
                    var dest_enumerator = (destination as IEnumerable).GetEnumerator();
                    var src_enumerator = (source as IEnumerable).GetEnumerator();

                    while (dest_enumerator.MoveNext() && src_enumerator.MoveNext())
                    {
                        dest_enumerator.Current.CopyPropertyValues(src_enumerator.Current);
                    }
                }
                else
                {
                    var destProperties = destination.GetType().GetRuntimeProperties();
                    foreach (var sourceProperty in source.GetType().GetRuntimeProperties())
                    {
                        foreach (var destProperty in destProperties)
                        {
                            if (ignoreProperties == null || Array.IndexOf(ignoreProperties, destProperty.Name) < 0)
                            {
                                if (destProperty.CanWrite
                                    && destProperty.Name == sourceProperty.Name
                                    && destProperty.PropertyType.GetTypeInfo().IsAssignableFrom(sourceProperty.PropertyType.GetTypeInfo())
                                    && destination.GetType().GetProperty(destProperty.Name).GetSetMethod() != null
                                    && destination.GetType().GetProperty(destProperty.Name).GetSetMethod().IsVirtual == false
                                    && destination.GetType().GetProperty(destProperty.Name).GetCustomAttributes(typeof(NotMappedAttribute), false).Length == 0)
                                {
                                    if (sourceProperty.PropertyType == typeof(string))
                                    {
                                        if (sourceProperty.GetValue(source, new object[] { }) != null)
                                            destProperty.SetValue(destination, sourceProperty.GetValue(source, new object[] { }).ToString().Trim(), new object[] { });
                                        else
                                            destProperty.SetValue(destination, null, new object[] { });
                                    }
                                    else
                                        destProperty.SetValue(destination, sourceProperty.GetValue(source, new object[] { }), new object[] { });
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static T CopyPropertyValuesWithObject<T>(this object destination, object source, String[] ignoreProperties = null)
        {
            destination.CopyPropertyValues(source, ignoreProperties);
            return (T)destination;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", typeof(T).FullName);
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static object ConvertToType<T>(this object obj, T type)
        {
            //create instance of T type object:
            object tmp = Activator.CreateInstance(Type.GetType(type.ToString()));

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                try
                {
                    //get the value of property and try to assign it to the property of T type object:
                    tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            //return the T type object:         
            return tmp;
        }

        public static object GetPropValue(this object src, string propName)
        {
            if (src.GetType().GetProperty(propName) != null)
                return src.GetType().GetProperty(propName).GetValue(src, null);
            else
                return null;
        }
        #endregion

        public static string replaceToBeakerNumber(string prmStr, int prmStartNum, int prmEndNum)
        {
            if (prmStr == null) prmStr = "";
            String myStr = "", myStrTemp = "";
            int prevFound = 0;
            int found = 0;
            int vNum = prmStartNum - 1;
            myStrTemp = prmStr.Replace(Environment.NewLine, "<br>");
            //vNum = myStr.Split(new string[] { "<br>" }, StringSplitOptions.None).Length - 1;
            
            found = myStrTemp.IndexOf("<br>");
            while (found >= 0 && vNum < prmEndNum - 1)
            {
                vNum++;
                myStr = myStr + vNum + "&nbsp;" + myStrTemp.Substring(prevFound, found-prevFound) + "<br>";
                prevFound = found + "<br>".Length;
                found = myStrTemp.IndexOf("<br>", prevFound);
            }
            if (vNum == 14)
            {
                myStr = myStr + (vNum + 1) + "&nbsp;" + myStrTemp.Substring(prevFound);
            }
            else
            {
                myStr = myStr + (vNum + 1) + "&nbsp;" + myStrTemp.Substring(prevFound);
                for (found = vNum + 1; found < prmEndNum; found++)
                {
                    myStr = myStr + "<br>\n" + (found + 1) + "&nbsp;";
                }
            }
            return myStr;
        }

        public static object ModelBindingDebug(HttpRequest reqest)
        {
            string content;
            using (StreamReader reader = new StreamReader(reqest.Body))
            {
                content = reader.ReadToEndAsync().Result;
                //...
            }
            var resp = JsonConvert.DeserializeObject(content);

            var jData = JsonConvert.DeserializeObject<JObject>(content);
            foreach (JProperty property in jData.Properties())
            {
                Console.WriteLine(property.Name + " - " + property.Name);
            }

            return resp;
        }

    }
}