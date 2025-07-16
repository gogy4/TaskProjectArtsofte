namespace TaskService.Application.Dto;

public class GetAllJobsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string Filter { get; set; } = "";

    public GetAllJobsRequest(int page = 1, int pageSize = 5, string filter = "")
    {
        Page = page;
        PageSize = pageSize;
        Filter = filter;
    }
}