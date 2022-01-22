using Newtonsoft.Json.Linq;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Limbo.Umbraco.BlockList.PropertyEditors {

    /// <summary>
    /// Class representing the configuration of <see cref="LimboBlockListPropertyEditor"/> data type.
    /// </summary>
    public class LimboBlockListConfiguration : BlockListConfiguration {

        /// <summary>
        /// Gets a reference to a <see cref="JObject"/> with information about the selected type converter.
        /// </summary>
        [ConfigurationField("typeConverter", "Type Converter", "/App_Plugins/Limbo.Umbraco.BlockList/TypeConverter.html", Description = "Select a type converter.")]
        public JObject TypeConverter { get; set; }

        /// <summary>
        /// Gets whether the block list editor is configured as a single picker (if max blocks is set to <c>1</c>).
        /// </summary>
        public bool IsSinglePicker => ValidationLimit.Max == 1;

    }

}