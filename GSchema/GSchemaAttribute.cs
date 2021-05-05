using System;
using System.Collections.Generic;
using System.Text;

namespace GSchema
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class GSchemaAttribute : Attribute
    {
        private string name;
        private string title;
        private bool isRequired;
        private string regularExpression;
        private string type;
        private string defaultValue;
        private string description;
        private string htmlClass;
        private string fieldHtmlClass;
        private string placeholder;
        private string activeClass;
        private string enumVal;
        private bool message;
        private string minimum;
        private string maximum;
        private bool exclusiveMinimum;
        private bool exclusiveMaximum;
        private bool multiple;

        public GSchemaAttribute(string _name, string _title, string _type = "string", bool _isRequired = false, string _description = null, string _defaultValue = null, string _placeholder = null)
        {
            this.name = _name;
            this.title = _title;
            this.type = _type;
            this.isRequired = _isRequired;
            this.defaultValue = _defaultValue;
            this.description = _description;
            this.placeholder = _placeholder;
        }
        public bool getexclusiveMinimum
        {
            get
            {
                return exclusiveMinimum;
            }
            set
            {
                exclusiveMinimum = value;
            }
        }
        public bool getMulitple
        {
            get
            {
                return multiple;
            }
            set
            {
                multiple = value;
            }
        }
        public bool getexclusiveMaximum
        {
            get
            {
                return exclusiveMaximum;
            }
            set
            {
                exclusiveMaximum = value;
            }
        }
        public string getminimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }
        public string getMaximun
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }
        public bool getmessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
        public string getRegularExpression
        {
            get
            {
                return regularExpression;
            }
            set
            {
                regularExpression = value;
            }
        }
        public string getDefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }
        public string getDiscription
        {
            get
            {
                return this.description;
            }
        }
        public string getPlaceHolder
        {
            get
            {
                return this.placeholder;
            }
        }
        public string getName
        {
            get
            {
                return this.name;
            }
        }
        public string getTitle
        {
            get
            {
                return this.title;
            }
        }
        public string getType
        {
            get
            {
                return this.type;
            }
        }
        public bool getIsRequired
        {
            get
            {
                return this.isRequired;
            }
        }

        public string getactiveClass
        {
            get
            {
                return activeClass;
            }
            set
            {
                activeClass = value;
            }
        }
        public string getHtmlClass
        {
            get
            {
                return htmlClass;
            }
            set
            {
                htmlClass = value;
            }
        }
        public string getfieldHtmlClass
        {
            get
            {
                return fieldHtmlClass;
            }
            set
            {
                fieldHtmlClass = value;
            }
        }
        public string getEnumVal
        {
            get { return enumVal; }
            set { enumVal = value; }
        }
    }
}
