namespace Hyperspan.Settings.Shared.Responses
{
    public class SettingResponse
    {
        public SettingResponse(Guid id, string value, string label)
        {
            Id = id;
            Value = value;
            Label = label;
        }

        public Guid Id { get; }
        public string Value { get; }
        public string Label { get; }
        public Guid? ParentId { get; set; }

    }
}
