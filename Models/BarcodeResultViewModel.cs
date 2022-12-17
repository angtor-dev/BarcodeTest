namespace BarcodeTest.Models
{
    [Serializable]
    public class BarcodeResultViewModel
    {
        public int ResponseCode { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string TimeElapse { get; set; } = string.Empty;
    }
}
