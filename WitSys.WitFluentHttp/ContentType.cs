namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// Indicates the type of content being sent with the request
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// The content is pure text
        /// </summary>
        Text = 0,
        /// <summary>
        /// The content is in json format
        /// </summary>
        ApplicationJson = 1,
        /// <summary>
        /// The content is Javascript
        /// </summary>
        JavaScript = 2,
        /// <summary>
        /// The content is in XML format
        /// </summary>
        ApplicationXML = 3,
        /// <summary>
        /// The content is in XML format
        /// </summary>
        TextXML = 4,
        /// <summary>
        /// The content is in HTML format
        /// </summary>
        HTML = 5
    }
}
