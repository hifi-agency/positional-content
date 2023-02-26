namespace Hifi.PositionalContent
{
    internal static class Constants
    {
        public const string CacheKey_GetTargetDataTypeDefinition = "PositionalContent_GetTargetDataTypeDefinition_";
        public const string ImageContentDataType = "imageContentDataType";
        public const string ImageSettingsDataType = "imageSettingsDataType";
        public const string ItemContentDataType = "itemContentDataType";
        public const string ItemSettingsDataType = "itemSettingsDataType";
    }

    public enum PositionalContentDataTypes
    {
        ImageContent,
        ImageSettings,
        ItemContent,
        ItemSettings
    }
}
