﻿using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace WpfW3cSvgTestSuite
{
    [Serializable]
    public sealed class SvgTestSuite : ICloneable
    {
        #region Public Constants

        public const int TestSuiteCount     = 3;

        public const string LocalDirBase    = @"..\..\..\W3cSvgTestSuites\";
        public const string WebDirBase      = "https://github.com/ElinamLLC/SharpVectors-TestSuites/raw/master/";

        public const string FileExtXml      = ".xml";
        public const string FileExtZip      = ".zip";

        public const string W3CDirPrefix    = "Svg";
        public const string W3CTestPrefix   = "SvgTestSuite";
        public const string W3CResultPrefix = "SvgTestResults";

        public static readonly string[] Descriptions = {
            "SVG 1.1 First Edition Test Suite: 13 December 2006",
            "SVG 1.1 Second Edition Test Suite: 16 August 2011",
            "SVG 1.2 Tiny Test Suite: 12 September 2008"
        };

        #endregion

        #region Private Fields

        private bool _isDefault;
        private bool _isSelected;

        private string _version;
        private string _description;

        private string _suiteName;
        private string _suiteDirName;

        private string _webSuitePath;
        private string _localSuitePath;

        private string _testFileName;
        private string _resultFileName;

        #endregion

        #region Constructors and Destructor

        public SvgTestSuite()
        {
        }

        public SvgTestSuite(XmlReader reader)
        {
            if (reader != null)
            {
                this.ReadXml(reader);
            }
        }

        public SvgTestSuite(SvgTestSuite source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "The source parameter cannot be null or Nothing.");
            }

            _isDefault      = source._isDefault;
            _isSelected     = source._isSelected;
            _version        = source._version;
            _description    = source._description;
            _suiteName      = source._suiteName;
            _suiteDirName   = source._suiteDirName;
            _webSuitePath   = source._webSuitePath;
            _localSuitePath = source._localSuitePath;
            _testFileName   = source._testFileName;
            _resultFileName = source._resultFileName;
        }

        private SvgTestSuite(int majorVersion, int minorVersion)
        {
            if (majorVersion == 1 && (minorVersion >= 0 && minorVersion <= 2))
            {
                string versionSuffix = string.Format("{0}{1}", majorVersion, minorVersion);

                _isDefault      = (majorVersion == 1 && minorVersion == 0);
                _isSelected     = _isDefault;

                _version        = string.Format("{0}.{1}", majorVersion, minorVersion);
                _description    = Descriptions[minorVersion];

                _suiteName      = W3CTestPrefix + versionSuffix;
                _suiteDirName   = W3CDirPrefix  + versionSuffix;

                _testFileName   = W3CTestPrefix   + versionSuffix + FileExtXml;
                _resultFileName = W3CResultPrefix + versionSuffix + FileExtXml;

                _webSuitePath   = WebDirBase + _suiteDirName + FileExtZip;
                _localSuitePath = Path.GetFullPath(Path.Combine(LocalDirBase, _suiteDirName));

                if (!Directory.Exists(_localSuitePath))
                {
                    Directory.CreateDirectory(_localSuitePath);
                }
            }
        }

        #endregion

        #region Public Properties

        public bool IsDefault
        {
            get {
                return _isDefault;
            }
            private set {
                _isDefault = value;
            }
        }

        public bool IsSelected
        {
            get {
                return _isSelected;
            }
            private set {
                _isSelected = value;
            }
        }

        public string Version
        {
            get {
                return _version;
            }
            private set {
                _version = value;
            }
        }

        public string Description
        {
            get {
                return _description;
            }
            private set {
                _description = value;
            }
        }

        public string SuiteName
        {
            get {
                return _suiteName;
            }
            private set {
                _suiteName = value;
            }
        }

        public string SuiteDirName
        {
            get {
                return _suiteDirName;
            }
            private set {
                _suiteDirName = value;
            }
        }

        public string TestFileName
        {
            get {
                return _testFileName;
            }
            private set {
                _testFileName = value;
            }
        }

        public string ResultFileName
        {
            get {
                return _resultFileName;
            }
            private set {
                _resultFileName = value;
            }
        }

        public string WebSuitePath
        {
            get {
                return _webSuitePath;
            }
            set {
                _webSuitePath = value;
            }
        }

        public string LocalSuitePath
        {
            get {
                return _localSuitePath;
            }
            set {
                _localSuitePath = value;
            }
        }

        #endregion

        #region Public Methods

        public static IList<SvgTestSuite> Create()
        {
            List<SvgTestSuite> testSuites = new List<SvgTestSuite>(3)
            {
                new SvgTestSuite(1, 0),
                new SvgTestSuite(1, 1),
                new SvgTestSuite(1, 2)
            };

            return testSuites;
        }

        public static SvgTestSuite GetDefault(IList<SvgTestSuite> testSuites)
        {
            SvgTestSuite defaultTestSuite = null;

            if (testSuites != null && testSuites.Count != 0)
            {
                foreach (var testSuite in testSuites)
                {
                    if (testSuite.IsDefault)
                    {
                        defaultTestSuite = testSuite;
                        break;
                    }
                }
            }
            return defaultTestSuite;
        }

        public static SvgTestSuite GetSelected(IList<SvgTestSuite> testSuites)
        {
            SvgTestSuite selectedTestSuite = null;

            if (testSuites != null && testSuites.Count != 0)
            {
                int selectedCount = 0;
                foreach (var testSuite in testSuites)
                {
                    if (testSuite.IsSelected)
                    {
                        selectedTestSuite = testSuite;
                        selectedCount++;
                    }
                }

                if (selectedCount != 1)
                {
                    return null;
                }
            }

            return selectedTestSuite;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(_suiteName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_suiteDirName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_testFileName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_resultFileName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_webSuitePath))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_localSuitePath))
            {
                return false;
            }
            return true;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
        }
        public void SetSelected(IList<SvgTestSuite> testSuites)
        {
            if (testSuites != null && testSuites.Count != 0)
            {
                foreach (var testSuite in testSuites)
                {
                    testSuite.IsSelected = false;
                }
            }
            _isSelected = true;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null || reader.NodeType != XmlNodeType.Element)
            {
                return;
            }
            if (!string.Equals(reader.Name, "testSuite", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string attribute = reader.GetAttribute("isDefault");
            if (!string.IsNullOrWhiteSpace(attribute))
            {
                _isDefault = XmlConvert.ToBoolean(attribute);
            }
            attribute = reader.GetAttribute("isSelected");
            if (!string.IsNullOrWhiteSpace(attribute))
            {
                _isSelected = XmlConvert.ToBoolean(attribute);
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    string.Equals(reader.Name, "property", StringComparison.OrdinalIgnoreCase))
                {
                    // <property name="Version">1.0</property>
                    string propertName   = reader.GetAttribute("name");
                    string propertyValue = reader.ReadElementContentAsString();
                    if (!string.IsNullOrWhiteSpace(propertName) && !string.IsNullOrWhiteSpace(propertyValue))
                    {
                        switch (propertName)
                        {
                            case "Version":
                                _version = propertyValue;
                                break;
                            case "Description":
                                _description = propertyValue;
                                break;
                            case "SuiteName":
                                _suiteName = propertyValue;
                                break;
                            case "SuiteDirName":
                                _suiteDirName = propertyValue;
                                break;
                            case "TestFileName":
                                _testFileName = propertyValue;
                                break;
                            case "ResultFileName":
                                _resultFileName = propertyValue;
                                break;
                            case "WebSuitePath":
                                _webSuitePath = propertyValue;
                                break;
                            case "LocalSuitePath":
                                _localSuitePath = propertyValue;
                                break;
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(reader.Name, "testSuite", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            writer.WriteStartElement("testSuite");
            writer.WriteAttributeString("isDefault", _isDefault ? "true" : "false");
            writer.WriteAttributeString("isSelected", _isSelected ? "true" : "false");

            this.WriteProperty(writer, "Version", _version);
            this.WriteProperty(writer, "Description", _description);

            this.WriteProperty(writer, "SuiteName", _suiteName);
            this.WriteProperty(writer, "SuiteDirName", _suiteDirName);

            this.WriteProperty(writer, "TestFileName", _testFileName);
            this.WriteProperty(writer, "ResultFileName", _resultFileName);

            this.WriteProperty(writer, "WebSuitePath", _webSuitePath);
            this.WriteProperty(writer, "LocalSuitePath", _localSuitePath);

            writer.WriteEndElement();
        }

        #endregion

        #region Private Methods

        private void WriteProperty(XmlWriter writer, string name, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            writer.WriteStartElement("property");
            writer.WriteAttributeString("name", name);
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        #endregion

        #region ICloneable Members

        public SvgTestSuite Clone()
        {
            SvgTestSuite clonedSuite = new SvgTestSuite(this);

            if (_version != null)
            {
                clonedSuite._version = string.Copy(_version);
            }
            if (_description != null)
            {
                clonedSuite._description = string.Copy(_description);
            }

            if (_suiteName != null)
            {
                clonedSuite._suiteName = string.Copy(_suiteName);
            }
            if (_suiteDirName != null)
            {
                clonedSuite._suiteDirName = string.Copy(_suiteDirName);
            }

            if (_testFileName != null)
            {
                clonedSuite._testFileName = string.Copy(_testFileName);
            }
            if (_resultFileName != null)
            {
                clonedSuite._resultFileName = string.Copy(_resultFileName);
            }

            if (_webSuitePath != null)
            {
                clonedSuite._webSuitePath = string.Copy(_webSuitePath);
            }
            if (_localSuitePath != null)
            {
                clonedSuite._localSuitePath = string.Copy(_localSuitePath);
            }

            return clonedSuite;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}
