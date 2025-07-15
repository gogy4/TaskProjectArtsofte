namespace TaskService.Application.Dto;

public class GetAllJobsRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Filter { get; set; }

    public GetAllJobsRequest(int page = 1, int pageSize = 5, string filter = "")
    {
        Page = page;
        PageSize = pageSize;
        Filter = filter;
    }
}