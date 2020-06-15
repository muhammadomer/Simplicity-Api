using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class Server
    {
        public string version { get; set; }
        public string buildDate { get; set; }
    }

    public class DataSourceLink
    {
        public string fieldName { get; set; }
        public string dataSourceName { get; set; }
    }

    public class OutputSettings
    {
    }

    public class Size
    {
        public double width { get; set; }
        public double height { get; set; }
    }

    public class Bounds
    {
        public int y { get; set; }
        public int x { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class Style
    {
        public int fontSize { get; set; }
        public string alignment { get; set; }
        public string color { get; set; }
    }

    public class Designer
    {
        public string dataSourceName { get; set; }
    }

    public class S4BFormsControl
    {

        public int sequenceNum { get; set; }
        public string fieldValue { get; set; }
        public string displayName { get; set; }
        public string type { get; set; }
        public Bounds bounds { get; set; }
        public string fieldName { get; set; }
        public Style style { get; set; }
        public bool? isReadOnly { get; set; }
        public bool? isLinked { get; set; }
        public Designer designer { get; set; }
        public bool? isMultiLine { get; set; }
        public string formatMask { get; set; }
        public int pageNo { get; set; }
    }

    public class Page
    {
        public int pageNum { get; set; }
        public Size size { get; set; }
        public List<S4BFormsControl> controls { get; set; }
    }

    public class DefaultControlStyle
    {
        public bool isBold { get; set; }
        public string color { get; set; }
        public int fontSize { get; set; }
        public bool isItalic { get; set; }
        public bool isUnderline { get; set; }
        public string fontName { get; set; }
        public string alignment { get; set; }
    }

    public class Designer2
    {
    }

    public class View
    {
        public List<Page> pages { get; set; }
        public string pdfName { get; set; }
        public int templateId { get; set; }
        public string createdDate { get; set; }
        public string modifiedDate { get; set; }
        public DefaultControlStyle defaultControlStyle { get; set; }
        public int version { get; set; }
        public string pdfUploadDate { get; set; }
        public string name { get; set; }
        public Designer2 designer { get; set; }
    }

    public class ValueTypeDef
    {
        public string handwritingType { get; set; }
        public bool allowPunctuation { get; set; }
        public bool allowSymbol { get; set; }
        public bool allowNumber { get; set; }
        public bool allowAlpha { get; set; }
        public bool isMandatory { get; set; }
        public bool? allowDate { get; set; }
    }

    public class Field
    {
        public string valueType { get; set; }
        public string displayName { get; set; }
        public string name { get; set; }
        public ValueTypeDef valueTypeDef { get; set; }
    }

    public class Attachments
    {
        public bool allowSupplementalPhoto { get; set; }
        public bool allowSupplementalAudio { get; set; }
        public bool allowSupplementalLocation { get; set; }
        public List<object> definitions { get; set; }
    }

    public class Model
    {
        public List<Field> fields { get; set; }
        public int templateId { get; set; }
        public string createdDate { get; set; }
        public string modifiedDate { get; set; }
        public Attachments attachments { get; set; }
        public List<object> flags { get; set; }
        public string name { get; set; }
    }

    public class RootObject
    {
        public Server server { get; set; }
        public List<DataSourceLink> dataSourceLinks { get; set; }
        public List<object> behavior { get; set; }
        public OutputSettings outputSettings { get; set; }
        public View view { get; set; }
        public string modelHash { get; set; }
        public string pdfHash { get; set; }
        public List<object> voResources { get; set; }
        public string behaviorHash { get; set; }
        public string viewHash { get; set; }
        public Model model { get; set; }
        public string exportHash { get; set; }
    }
}
