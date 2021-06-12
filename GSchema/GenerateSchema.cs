using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace GSchema
{
    public class GSgenerator
    {
        private readonly getEnumList _enumList;
        public GSgenerator(getEnumList enumList)
        {
            _enumList = enumList;
        }
        public async  Task<string> GenerateSchema<T>(string zone = "", string maxVal = "")
        {
            var obj = typeof(T).Name;
            StringBuilder schema = new StringBuilder();
            StringBuilder form = new StringBuilder();

            schema.Append("{");
            form.Append("[");
            System.Attribute[] topattrs = System.Attribute.GetCustomAttributes(typeof(T));
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int x = 0;
            foreach (PropertyInfo prop in Props)
            {

                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    GSchemaAttribute schemaAttr = attr as GSchemaAttribute;
                    if (schemaAttr != null)
                    {
                        string propName = prop.Name;
                        string name = schemaAttr.getName;
                        string title = schemaAttr.getTitle;
                        bool isrequired = schemaAttr.getIsRequired;
                        string regularExpression = schemaAttr.getRegularExpression;
                        string propType = schemaAttr.getType;
                        string defaultValue = schemaAttr.getDefaultValue;
                        string description = schemaAttr.getDiscription;
                        string htmlClass = schemaAttr.getHtmlClass;
                        string fieldHtmlClass = schemaAttr.getfieldHtmlClass;
                        string placeholder = schemaAttr.getPlaceHolder;
                        string activeClass = schemaAttr.getactiveClass;
                        bool message = schemaAttr.getmessage;
                        bool exclusiveMaximum = schemaAttr.getexclusiveMaximum;
                        bool exclusiveMinimum = schemaAttr.getexclusiveMinimum;
                        string EnumVal = schemaAttr.getEnumVal;
                        string minimum = schemaAttr.getminimum;
                        string maximum = schemaAttr.getMaximun;
                        string nameval = string.IsNullOrWhiteSpace(name) ? propName : name;
                        bool multiple = schemaAttr.getMulitple;
                        if (x == 0 && propType != "Array")
                        {
                            schema.Append("\"" + nameval + "\":{\"type\":\"string\"");
                            form.Append("{\"key\":\"" + nameval + "\"");
                        }
                        else if (propType != "Array")
                        {
                            schema.Append(",\"" + nameval + "\":{\"type\":\"string\"");
                            form.Append(",{\"key\":\"" + nameval + "\"");
                        }
                        else if (x == 0 && propType == "Array")
                        {
                            schema.Append("\"" + nameval + "\":{\"type\":\"array\"");
                            schema.Append(",\"items\": {\"type\": \"object\",\"title\": \"" + title + "\",\"properties\": {");
                            var sch = await generateInnerSchema(prop, zone);
                            schema.Append(sch.schema);
                            schema.Append("}}");
                            form.Append("{\"key\":\"" + nameval + "\"");
                            form.Append(",\"type\": \"tabarray\",\"items\": {\"type\": \"section\",\"items\": [");
                            form.Append(sch.form);
                            form.Append("]}");

                        }
                        else if (propType == "Array")
                        {
                            schema.Append(",\"" + nameval + "\":{\"type\":\"array\"");
                            schema.Append(",\"items\": {\"type\": \"object\",\"title\": \"" + title + "\",\"properties\": {");
                            var sch = await generateInnerSchema(prop, zone);
                            schema.Append(sch.schema);
                            schema.Append("}}");
                            form.Append(",{\"key\":\"" + nameval + "\"");
                            form.Append(",\"type\": \"tabarray\",\"htmlClass\": \"col-md-12\",\"items\": {\"type\": \"section\",\"items\": [");
                            form.Append(sch.form);
                            form.Append("]}");
                        }
                        if (!string.IsNullOrEmpty(title) && propType != "Array")
                        {
                            schema.Append(",\"title\":\"" + title + "\"");
                        }
                        if (!string.IsNullOrEmpty(defaultValue) && propType != "Array")
                        {
                            schema.Append(",\"default\":\"" + defaultValue + "\"");
                        }
                        if (!string.IsNullOrEmpty(description) && propType != "Array")
                        {
                            schema.Append(",\"description\":\"" + description + "\"");
                        }
                        if (isrequired && propType != "Array")
                        {
                            schema.Append(",\"required\":true");
                        }
                        if (!string.IsNullOrEmpty(EnumVal) && propType != "Array")
                        {
                            schema.Append(",\"enum\":" + await _enumList.getEnumRecords(EnumVal, zone));
                        }
                        if (!string.IsNullOrEmpty(regularExpression) && propType != "Array")
                        {
                            schema.Append(",\"pattern\":\"" + regularExpression + "\"");
                        }
                        if (message && propType != "Array")
                        {
                            schema.Append(",\"messages\":" + await _enumList.getVlidationMessage(nameval));
                        }
                        if (!string.IsNullOrEmpty(minimum) && propType != "Array")
                        {
                            schema.Append(",\"minimum\":" + minimum);
                        }
                        if (!string.IsNullOrEmpty(maximum) && propType != "Array")
                        {
                            schema.Append(",\"maximum\":" + maximum);
                        }
                        if (!string.IsNullOrWhiteSpace(maxVal) && propType != "Array")
                        {
                            schema.Append(",\"maximum\":" + maxVal);
                        }
                        if (exclusiveMinimum && propType != "Array")
                        {
                            schema.Append(",\"exclusiveMinimum\":" + exclusiveMinimum);
                        }
                        if (exclusiveMaximum && propType != "Array")
                        {
                            schema.Append(",\"exclusiveMaximum\":" + exclusiveMaximum);
                        }
                        //if(multiple && propType != "Array")
                        //{
                        //    schema.Append(",\"multiple\":" + multiple);
                        //}

                        if (!string.IsNullOrEmpty(propType) && !propType.Equals("string") && propType != "Array")
                        {
                            form.Append(",\"type\":\"" + propType + "\"");
                        }

                        if (!string.IsNullOrEmpty(placeholder) && propType != "Array")
                        {
                            form.Append(",\"placeholder\":\"" + placeholder + "\"");
                        }
                        if (!string.IsNullOrEmpty(htmlClass) && propType != "Array")
                        {
                            form.Append(",\"htmlClass\":\"" + htmlClass + "\"");
                        }
                        if (!string.IsNullOrEmpty(fieldHtmlClass) && propType != "Array")
                        {
                            form.Append(",\"fieldHtmlClass\":\"" + fieldHtmlClass + "\"");
                        }
                        if (!string.IsNullOrEmpty(activeClass) && propType != "Array")
                        {
                            form.Append(",\"activeClass\":\"" + activeClass + "\"");
                        }
                        schema.Append("}");
                        form.Append("}");
                        x++;
                    }
                }
            }
            schema.Append("}");
            form.Append(",{\"type\":\"submit\",\"title\":\"Submit\"}]");
            string schemaFile = "{\"schema\":" + schema + ",\"form\":" + form + "}";
            return schemaFile;
        }
        public async  Task<schemaFile> generateInnerSchema(PropertyInfo prop, string zone = "")
        {
            var nam = prop.Name;
            StringBuilder schema = new StringBuilder();
            StringBuilder form = new StringBuilder();
            var pp = prop.PropertyType.GetGenericArguments()[0].Name;
            PropertyInfo[] Props1 = prop.PropertyType.GetGenericArguments()[0].GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int y = 0;
            foreach (PropertyInfo prop1 in Props1)
            {
                object[] attrs = prop1.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    GSchemaAttribute schemaAttr = attr as GSchemaAttribute;
                    if (schemaAttr != null)
                    {
                        string propName = prop1.Name;
                        string name = schemaAttr.getName;
                        string title = schemaAttr.getTitle;
                        bool isrequired = schemaAttr.getIsRequired;
                        string regularExpression = schemaAttr.getRegularExpression;
                        string propType = schemaAttr.getType;
                        string defaultValue = schemaAttr.getDefaultValue;
                        string description = schemaAttr.getDiscription;
                        string htmlClass = schemaAttr.getHtmlClass;
                        string fieldHtmlClass = schemaAttr.getfieldHtmlClass;
                        string placeholder = schemaAttr.getPlaceHolder;
                        string activeClass = schemaAttr.getactiveClass;
                        bool message = schemaAttr.getmessage;
                        bool exclusiveMaximum = schemaAttr.getexclusiveMaximum;
                        bool exclusiveMinimum = schemaAttr.getexclusiveMinimum;
                        string EnumVal = schemaAttr.getEnumVal;
                        string minimum = schemaAttr.getminimum;
                        string maximum = schemaAttr.getMaximun;
                        string nameval = string.IsNullOrWhiteSpace(name) ? propName : name;
                        if (y == 0 && propType != "Array")
                        {
                            schema.Append("\"" + nameval + "\":{\"type\":\"string\"");
                            // form.Append("{\"key\":\"" + nameval + "\"");
                        }
                        else if (propType != "Array")
                        {
                            schema.Append(",\"" + nameval + "\":{\"type\":\"string\"");
                            //form.Append(",{\"key\":\"" + nameval + "\"");
                        }

                        if (!string.IsNullOrEmpty(title) && propType != "Array")
                        {
                            schema.Append(",\"title\":\"" + title + "\"");
                        }
                        if (!string.IsNullOrEmpty(defaultValue) && propType != "Array")
                        {
                            schema.Append(",\"default\":\"" + defaultValue + "\"");
                        }
                        if (!string.IsNullOrEmpty(description) && propType != "Array")
                        {
                            schema.Append(",\"description\":\"" + description + "\"");
                        }
                        if (isrequired && propType != "Array")
                        {
                            schema.Append(",\"required\":true");
                        }
                        if (!string.IsNullOrEmpty(EnumVal) && propType != "Array")
                        {
                            schema.Append(",\"enum\":" + await _enumList.getEnumRecords(EnumVal, zone));
                        }
                        if (!string.IsNullOrEmpty(regularExpression) && propType != "Array")
                        {
                            schema.Append(",\"pattern\":\"" + regularExpression + "\"");
                        }
                        if (message && propType != "Array")
                        {
                            schema.Append(",\"messages\":" + await _enumList.getVlidationMessage(nameval));
                        }
                        if (!string.IsNullOrEmpty(minimum) && propType != "Array")
                        {
                            schema.Append(",\"minimum\":" + minimum);
                        }
                        if (!string.IsNullOrEmpty(maximum) && propType != "Array")
                        {
                            schema.Append(",\"maximum\":" + maximum);
                        }
                        if (exclusiveMinimum && propType != "Array")
                        {
                            schema.Append(",\"exclusiveMinimum\":" + exclusiveMinimum);
                        }
                        if (exclusiveMaximum && propType != "Array")
                        {
                            schema.Append(",\"exclusiveMaximum\":" + exclusiveMaximum);
                        }
                        if (y == 0)
                        {
                            form.Append("\"" + nam + "[]." + propName + "\"");
                        }
                        else
                        {
                            form.Append(",\"" + nam + "[]." + propName + "\"");
                        }


                        schema.Append("}");
                        //form.Append("}");
                        y++;
                    }
                }
            }

            return new schemaFile()
            {
                form = form.ToString(),
                schema = schema.ToString()
            };
        }
    }

    public class schemaFile
    {
        public string schema { get; set; }
        public string form { get; set; }
    }
}
