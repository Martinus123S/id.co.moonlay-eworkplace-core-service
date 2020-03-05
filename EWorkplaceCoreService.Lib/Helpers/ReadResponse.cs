using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Lib.Helpers
{
    public class ReadResponse<TModel>
    {
        public List<TModel> Data { get; set; }
        public int Count { get; set; }
        public Dictionary<string, string> Order { get; set; }
        public List<string> Selected { get; set; }
        public ReadResponse(List<TModel> data, int count, Dictionary<string, string> order, List<string> selected)
        {
            Data = data;
            Count = count;
            Order = order;
            Selected = selected;
        }
    }
}
