/// Author: Zhicheng Su
using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Text;
using System.Web.Caching;
using System.Collections.Generic;
using System.Threading;
using System.Configuration;
using App.Common.InversionOfControl;
using App.Common.Caching;
using System.Linq;
using System.Xml.Linq;


namespace App.Common
{
    /// <summary>
    /// Report Config.
    /// </summary>
    public class ReportConfig : IReportConfig
    {
        #region Fields
        private Dictionary<string, ReportSection> _sections;
        private string _configFilePath;
        private const string NameAttribute = "Name";
        private const string DescriptionAttribute="Description";
        private const string RootKey = "Reports";
        private const string ItemTag = "Report";
        private const string TemplatePathKey="TemplatePath";
        private const string AutoGenerateHeaderKey = "AutoGenerateHeader";
        private const string OutputPathKey="OutputPath";
        private const string ExcelKey = "Excel";
        private const string RolesKey = "Roles";
        private const string VisibleAttribute = "Visible";
        private const string GroupNameAttribute = "Groupname";
        private const string GroupIndexAttribute = "Groupindex";
        private const string ReportIndexAttribute = "ReportIndex";
        private const string DataStartColumnKey = "DataStartColumn";
        private const string DataStartRowKey = "DataStartRow";
        private const string HeaderRowStyleKey = "HeaderRowStyle";
        private const string EvenRowStyleKey = "EvenRowStyle";
        private const string OddRowStyleKey = "OddRowStyle";
        private const string FontKey = "Font";
        private const string FontColorKey="Color";
        private const string BackGroundColorKey="BackGroundColor";
        private const string FontWeightKey ="Weight";
        private const string FontSizeKey = "Size";

        #endregion
        #region Properties
        public string ConfigFilePath { get { return _configFilePath; } }
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilePath"></param>
        public ReportConfig(string configFilePath)
        {
            Check.IsNotEmpty(configFilePath, "configFilePath");
            _configFilePath = configFilePath;
            Config();
        }
        /// <summary>
        /// Return the specific section config
        /// </summary>
        public ReportSection this[string name]
        {
            get
            {
                return _sections[name] as ReportSection;
            }
        }
        public void Config()
        {
            ReportSection defaultSection;
            _sections = new Dictionary<string, ReportSection>();
            #region Default Settings
            XElement doc=XElement.Load(ConfigFilePath);
            defaultSection = doc.DescendantsAndSelf(RootKey).Select(
                r => new ReportSection
                {
                    OutputPath = (string)r.Element(OutputPathKey),
                    Excel = r.Elements(ExcelKey).Select(e => new ExcelSection
                    {
                        AutoGenerateHeader = e.Element(AutoGenerateHeaderKey) == null || string.IsNullOrWhiteSpace((string)e.Element(AutoGenerateHeaderKey)) ? true : bool.Parse((string)e.Element(AutoGenerateHeaderKey)),
                        DataStartColumn = e.Element(DataStartColumnKey) == null ? -1 : (int)e.Element(DataStartColumnKey),
                        DataStartRow = e.Element(DataStartRowKey) == null ? -1 : (int)e.Element(DataStartRowKey),
                        EvenRowStyle = e.Element(EvenRowStyleKey) == null ? new Style { Font = new FontStyle() } :
                        new Style
                        {
                            BackGroundColor = (string)e.Element(EvenRowStyleKey).Element(BackGroundColorKey),
                            Font = e.Element(EvenRowStyleKey).Element(FontKey) == null ? new FontStyle() : 
                            new FontStyle
                            {
                                Weight = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                Color = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontColorKey),
                                Size = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontSizeKey)
                            }
                        },
                        OddRowStyle = e.Element(OddRowStyleKey) == null ? new Style { Font = new FontStyle() } :
                        new Style
                        {
                            BackGroundColor = (string)e.Element(OddRowStyleKey).Element(BackGroundColorKey),
                            Font = e.Element(OddRowStyleKey).Element(FontKey) == null ? new FontStyle() :
                            new FontStyle
                            {
                                Weight = e.Element(OddRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                Color = e.Element(OddRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontColorKey),
                                Size = e.Element(OddRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontSizeKey)
                            }
                        },
                        HeaderRowStyle = e.Element(HeaderRowStyleKey) == null ? new Style { Font = new FontStyle() } :
                        new Style
                        {
                            BackGroundColor = (string)e.Element(HeaderRowStyleKey).Element(BackGroundColorKey),
                            Font = e.Element(HeaderRowStyleKey).Element(FontKey) == null ? new FontStyle() :
                            new FontStyle
                            {
                                Weight = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                Color = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontColorKey),
                                Size = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontSizeKey)
                            }
                        }
                    }).SingleOrDefault()
                }).FirstOrDefault();
            if (defaultSection == null)
            {
                defaultSection = new ReportSection();
            }
            if (defaultSection.Excel == null)
                defaultSection.Excel = new ExcelSection();
            //if (defaultSection.Excel.EvenRowStyle == null)
            //    defaultSection.Excel.EvenRowStyle = new Style();
            //if (defaultSection.Excel.OddRowStyle == null)
            //    defaultSection.Excel.OddRowStyle = new Style();
            //if (defaultSection.Excel.HeaderRowStyle == null)
            //    defaultSection.Excel.HeaderRowStyle = new Style();

            #endregion Default Settings
            _sections = doc.Descendants(ItemTag).ToDictionary(
                e => (string)e.Attribute(NameAttribute),
                r => new ReportSection
                           {
                               SectionId = (string)r.Attribute(NameAttribute),
                               Description = (string)r.Attribute(DescriptionAttribute),
                               OutputPath = (string)r.Element(OutputPathKey)??defaultSection.OutputPath,
                               Excel = r.Elements(ExcelKey).Select(e => new ExcelSection
                               {
                                   TemplatePath = (string)e.Element(TemplatePathKey),
                                   AutoGenerateHeader=e.Element(AutoGenerateHeaderKey)==null || string.IsNullOrWhiteSpace((string)e.Element(AutoGenerateHeaderKey))?defaultSection.Excel.AutoGenerateHeader:bool.Parse((string)e.Element(AutoGenerateHeaderKey)),
                                   DataStartColumn = e.Element(DataStartColumnKey) == null ? defaultSection.Excel.DataStartColumn : (int)e.Element(DataStartColumnKey),
                                   DataStartRow = e.Element(DataStartRowKey) == null ? defaultSection.Excel.DataStartRow : (int)e.Element(DataStartRowKey),
                                   EvenRowStyle = e.Element(EvenRowStyleKey) == null ? defaultSection.Excel.EvenRowStyle : 
                                   new Style
                                   {
                                       BackGroundColor = (string)e.Element(EvenRowStyleKey).Element(BackGroundColorKey) ?? defaultSection.Excel.EvenRowStyle.BackGroundColor,
                                       Font = e.Element(EvenRowStyleKey).Element(FontKey) == null ? defaultSection.Excel.EvenRowStyle.Font :
                                       new FontStyle
                                       {
                                           Weight = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                           Color = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontColorKey),
                                           Size = e.Element(EvenRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(EvenRowStyleKey).Element(FontKey).Element(FontSizeKey)
                                       }
                                   },
                                   OddRowStyle = e.Element(OddRowStyleKey) == null ? defaultSection.Excel.OddRowStyle : 
                                   new Style
                                   {
                                       BackGroundColor = (string)e.Element(OddRowStyleKey).Element(BackGroundColorKey) ?? defaultSection.Excel.OddRowStyle.BackGroundColor,
                                       Font = e.Element(OddRowStyleKey).Element(FontKey) == null ? defaultSection.Excel.EvenRowStyle.Font :
                                       new FontStyle
                                       {
                                           Weight = e.Element(OddRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                           Color = e.Element(OddRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontColorKey),
                                           Size = e.Element(OddRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(OddRowStyleKey).Element(FontKey).Element(FontSizeKey)
                                       }
                                   },
                                   HeaderRowStyle = e.Element(HeaderRowStyleKey) == null ? defaultSection.Excel.HeaderRowStyle
 : 
                                   new Style
                                   {
                                       BackGroundColor = (string)e.Element(HeaderRowStyleKey).Element(BackGroundColorKey) ?? defaultSection.Excel.HeaderRowStyle.BackGroundColor,
                                       Font = e.Element(HeaderRowStyleKey).Element(FontKey) == null ? defaultSection.Excel.EvenRowStyle.Font :
                                       new FontStyle
                                       {
                                           Weight = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontWeightKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontWeightKey),
                                           Color = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontColorKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontColorKey),
                                           Size = e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontSizeKey) == null ? null : (string)e.Element(HeaderRowStyleKey).Element(FontKey).Element(FontSizeKey)
                                       }
                                   }
                               }).SingleOrDefault()
                           });
        }

        /// <summary>
        /// </summary>
        public Dictionary<string,ReportSection> ItemSections
        {
            get { return _sections; }
        }
    }

    public class ReportSection
    {
        #region Properties
        public virtual string SectionId { get; set; }
        public virtual string Roles { get; set; }
        public virtual string Description { get; set; }
        public virtual string OutputPath { get; set; }
        public virtual bool Visible { get; set; }
        public virtual string GroupName { get; set; }
        public virtual string GroupIndex { get; set; }
        public virtual string ReportIndex { get; set; }
        public virtual ExcelSection Excel { get; set; }
        #endregion
    }
    public class ExcelSection
    {
        public virtual Style HeaderRowStyle { get; set; }
        public virtual Style EvenRowStyle { get; set; }
        public virtual Style OddRowStyle { get; set; }
        public virtual int DataStartRow { get; set; }
        public virtual int DataStartColumn { get; set; }
        public virtual string TemplatePath { get; set; }
        public virtual bool AutoGenerateHeader { get; set; }
    }
    public class Style
    {
        public virtual string BackGroundColor { get; set; }
        public virtual FontStyle Font {get;set;}
    }
    public class FontStyle
    {
        public virtual string Color { get; set; }
        public virtual string Weight { get; set; }
        public virtual string Size { get; set; }
    }
}
