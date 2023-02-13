namespace Model
{
    [Serializable]
    public class LocationModel
    {
        public string type { get; set; }
        public GeometryModel geometry { get; set; }
        public PropertyModel properties { get; set; }
    }
}