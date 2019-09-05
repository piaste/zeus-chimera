using System.ComponentModel.DataAnnotations;

namespace Common.Contracts.Requests
{
    public class FetchPagedRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int? StartRowIndex { get; set; }

        [Required]
        [Range(0, 100)]
        public int? MaximumRows { get; set; }
    }
}
