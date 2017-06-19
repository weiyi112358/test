using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Mvc
{
    public class WebConfigSerializerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="WebConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the Web.config file into an
        /// <see cref="AuthorizeConfig"/> instance.</remarks>
        /// <returns>An instance of the <see cref="WebConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(WebConfiguration));
            // Return the Deserialized object from the Web.config XML
            return ser.Deserialize(new XmlNodeReader(section));
        }
    }
}
